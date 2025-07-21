using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class MonsterType
{
    public string Name;
    public Monster EnemyPrefab;
    public int DifficultyCost;
    public bool CanSpawn = true;
}

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float patrolSpeed;
    [SerializeField] protected float chaseDetectionRadius;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float distanceBetweenEnemyToStop;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] int patrolRadius = 10;
    [SerializeField] int ExcludeRangeAbs;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField]protected List<Transform> adventurersInRange = new List<Transform>();
    List<Transform> allAdventerurers=new List<Transform>();
    protected Transform target;
    protected Collider2D floorThatWasSpawnedOn;
    protected bool isAggro;
    protected bool isAbilityCycleActive = false;
    protected Coroutine abilityCycle;

    void Start()
    {
        Init();
    }

    virtual public void Init()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        transform.position = (Vector2)transform.position;
        Debug.Log(transform.position);
    }

    public void Activate()
    {
        Debug.Log((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        floorThatWasSpawnedOn = Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, int.MaxValue, 1<<6).collider;
        Debug.Log(floorThatWasSpawnedOn.name);
        navMeshAgent.enabled = true;
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        isAggro = false;
        while (true)
        {
            while (!isAggro)
            {
                MovePatrol();
                yield return new WaitUntil(() => (navMeshAgent.remainingDistance < distanceBetweenEnemyToStop) || isAggro);
            }
            while (isAggro)
            {
                MoveAggro();
                if (!isAbilityCycleActive)
                {
                    abilityCycle = StartCoroutine(AttackCycle());
                    isAbilityCycleActive = true;
                }
                yield return null;
            }
        }
    }
    void OnEnable()
    {
        WaveSpawner.RAIDACTIVE += FindAllAdventerers;
    }

    void OnDisable()
    {
        WaveSpawner.RAIDACTIVE -= FindAllAdventerers;
    }
    void FindAllAdventerers(GameObject enemy)
    {
        allAdventerurers.Add(enemy.transform);
    }
    void FixedUpdate()
    {
        CleanAndSortAdventurers();
        allAdventerurers.RemoveAll(t => t == null);
        foreach (Transform adventurer in allAdventerurers)
        {
            float adventurerDistance = Vector3.Distance(transform.position, adventurer.position);

            if (adventurerDistance < chaseDetectionRadius)
            {
                AddAdventurer(adventurer);
            }
            else
            {
                adventurersInRange.Remove(adventurer);
            }
        }
        
        NoTargetCheck();
    }

    void CleanAndSortAdventurers()
    {
        // Remove nulls
        adventurersInRange.RemoveAll(t => t == null);
        // Sort by distance to this object
        adventurersInRange.Sort((a, b) =>
        {
            float distA = Vector3.Distance(transform.position, a.position);
            float distB = Vector3.Distance(transform.position, b.position);
            return distA.CompareTo(distB);
        });
    }
    protected void AddAdventurer(Transform adventurer)
    {
        if (adventurer.CompareTag("Enemy"))
        {
            if (adventurersInRange.Count == 0)
            {
                navMeshAgent.autoBraking = false;
                navMeshAgent.stoppingDistance = distanceBetweenEnemyToStop;
                isAggro = true;
                target = adventurer;
                navMeshAgent.speed = chaseSpeed;
            }

            if (!adventurersInRange.Contains(adventurer))
                adventurersInRange.Add(adventurer);
        }
    }

    protected void NoTargetCheck()
    {

            if (adventurersInRange.Count == 0)
            {
                if (isAbilityCycleActive)
                {
                    StopCoroutine(abilityCycle);
                    isAbilityCycleActive = false;
                    abilityCycle = null;
                }


                navMeshAgent.autoBraking = true;
                navMeshAgent.speed = patrolSpeed;
                ;
                isAggro = false;
                target = null;
            }
            else
            {
                target = adventurersInRange.First();
            }
        
    }

    Vector3 GetPatrolPoint()
    {
        NavMeshHit hit;

        var randomOffsets = Enumerable.Range(-patrolRadius, patrolRadius * 2 + 1)
            .Where(x => Mathf.Abs(x) >= ExcludeRangeAbs)
            .ToArray();

        if (randomOffsets.Length == 0)
            return transform.position;

        int offsetX = randomOffsets[UnityEngine.Random.Range(0, randomOffsets.Length)];
        int offsetY = randomOffsets[UnityEngine.Random.Range(0, randomOffsets.Length)];

        Vector3 samplePoint = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, 0);

        if (NavMesh.SamplePosition(samplePoint, out hit, 5f, NavMesh.AllAreas))
            return floorThatWasSpawnedOn.ClosestPoint(hit.position);
        else
            return transform.position; // fallback
    }
    virtual protected void MovePatrol()
    {
        Vector3 point = GetPatrolPoint();
        SetDestination(point);
        animator.SetBool("IsWalking", true);
    }
    virtual protected void MoveAggro()
    {
        NoTargetCheck();
        if (target == null)
            return;
        if ((transform.position - target.position).magnitude > distanceBetweenEnemyToStop)
            {
                Debug.Log("IsWalking");
                animator.SetBool("IsWalking", true);
                SetDestination(target.position);
            }
            else
            {
                animator.SetBool("IsWalking", false);
                Debug.Log("nuhuh");
            }
        //SetDestination(target.position + ((transform.position - target.position).normalized * distanceBetweenEnemyToStop));
    }



    public void DisableAndDestroy()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected void SetDestination(Vector3 targetPos) // NavMeshPlus x issue
    {
        if (Mathf.Abs(transform.position.x - targetPos.x) < 0.0001f)
        {
            Vector3 driftPos;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos + new Vector3(0.0001f, 0f, 0f), out hit, 0, NavMesh.AllAreas))
            {
                driftPos = targetPos + new Vector3(0.0001f, 0f, 0f);
            }
            else if (NavMesh.SamplePosition(targetPos + new Vector3(-0.0001f, 0f, 0f), out hit, 0, NavMesh.AllAreas))
            {
                driftPos = targetPos + new Vector3(-0.0001f, 0f, 0f);
            }
            else
            {
                driftPos = GetPatrolPoint();
            }
            navMeshAgent.SetDestination(driftPos);
        }
        else if (gameObject)
        {
            navMeshAgent.SetDestination(targetPos);
        }
        if (targetPos.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    virtual protected void Attack()
    {
        if(target!=null)
        target.GetComponent<EnemyHealth>().TakeDamage(damage);
        
    }

    virtual protected IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);
            yield return new WaitUntil(() => target != null && (transform.position - target.position).magnitude <= attackDistance);
            animator.SetTrigger("OnAttacking");
            Attack();
        }
    }
}

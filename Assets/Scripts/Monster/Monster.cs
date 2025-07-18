using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] protected float patrolDetectionRadius;
    [SerializeField] protected float chaseDetectionRadius;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float distanceBetweenEnemyToStop;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] CircleCollider2D detectionCollider;
    [SerializeField] int ExcludeRangeAbs;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected List<Transform> adventurersInRange = new List<Transform>();
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
        detectionCollider.radius = patrolDetectionRadius;
        navMeshAgent.enabled = true;
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
                yield return new WaitUntil(() => (navMeshAgent.remainingDistance < 0.3f) || isAggro);
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

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            if (adventurersInRange.Count == 0)
            {
                navMeshAgent.autoBraking = false;
                navMeshAgent.stoppingDistance = distanceBetweenEnemyToStop;
                isAggro = true;
                target = other.transform;
                navMeshAgent.speed = chaseSpeed;
                detectionCollider.radius = chaseDetectionRadius;
            }
            adventurersInRange.Add(other.transform);
            //Debug.Log($"Entered {other.name}");
        }
    }

    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            adventurersInRange.Remove(other.transform);
            //Debug.Log($"Removed {other.name}");
            if (adventurersInRange.Count == 0)
            {
                if (isAbilityCycleActive)
                {
                    StopCoroutine(abilityCycle);
                    isAbilityCycleActive = false;
                    abilityCycle = null;
                }
                SetDestination(other.transform.position);
                navMeshAgent.autoBraking = true;
                navMeshAgent.speed = patrolSpeed;
                navMeshAgent.stoppingDistance = 0;
                isAggro = false;
                detectionCollider.radius = patrolDetectionRadius;
                target = null;
            }
            else
            {
                target = adventurersInRange.First().transform;
            }
        }
    }

    Vector3 GetPatrolPoint()
    {
        NavMeshHit hit;
        var randomNum = Enumerable.Range(-2, 5).Where(x => (x <= -ExcludeRangeAbs) || (ExcludeRangeAbs <= x)).ToArray();
        NavMesh.SamplePosition(new Vector3(transform.position.x + randomNum[UnityEngine.Random.Range(0, randomNum.Length)], transform.position.y + randomNum[UnityEngine.Random.Range(0, randomNum.Length)]), out hit, 5f, NavMesh.AllAreas);
        return floorThatWasSpawnedOn.ClosestPoint(hit.position);
    }
    virtual protected void MovePatrol()
    {
        Vector3 point = GetPatrolPoint();
        SetDestination(point);
        animator.SetBool("IsWalking", true);
    }
    virtual protected void MoveAggro()
    {
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
        else
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
        target.GetComponent<EnemyHealth>().TakeDamage(damage);
        animator.SetTrigger("OnAttacking");
    }

    virtual protected IEnumerator AttackCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackCooldown);
            yield return new WaitUntil(() => (transform.position - target.position).magnitude <= attackDistance);
            Attack();
        }
    }
}

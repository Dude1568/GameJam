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
    [SerializeField] protected float abilityCooldown;
    [SerializeField] protected float distanceBetweenEnemyToStop;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] CircleCollider2D detectionCollider;
    [SerializeField] int ExcludeRangeAbs;
    protected List<Adventerer> adventurersInRange = new List<Adventerer>();
    bool isAggro;
    Transform target;

    void Start()
    {
        Init();
    }

    virtual public void Init()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        detectionCollider.radius = patrolDetectionRadius;
    }

    public void Activate()
    {
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        isAggro = false;
        while (true)
        {
            while (!isAggro)
            {
                yield return new WaitUntil(() => navMeshAgent.remainingDistance < 0.3f || isAggro);
                MovePatrol();
            }
            while (isAggro)
            {
                MoveAggro();
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
                isAggro = true;
                target = other.transform;
                navMeshAgent.speed = chaseSpeed;
                detectionCollider.radius = chaseDetectionRadius;
            }
            Adventerer adventurer = other.GetComponent<Adventerer>();
            adventurersInRange.Add(adventurer);
        }
    }

    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Adventerer adventurer = other.GetComponent<Adventerer>();
            adventurersInRange.Remove(adventurer);
            if (adventurersInRange.Count == 0)
            {
                SetDestination(other.transform.position);
                navMeshAgent.autoBraking = true;
                navMeshAgent.speed = patrolSpeed;
                isAggro = false;
                detectionCollider.radius = patrolDetectionRadius;
            }
        }
    }

    Vector3 GetPatrolPoint()
    {
        NavMeshHit hit;
        var randomNum = Enumerable.Range(-2, 5).Where(x => (x <= -ExcludeRangeAbs) || (ExcludeRangeAbs <= x)).ToArray();
        NavMesh.SamplePosition(new Vector3(transform.position.x + randomNum[UnityEngine.Random.Range(0, randomNum.Length)], transform.position.y + randomNum[UnityEngine.Random.Range(0, randomNum.Length)]), out hit, 5f, NavMesh.AllAreas);
        return hit.position;
    }
    virtual protected void MovePatrol()
    {
        Vector3 point = GetPatrolPoint();
        SetDestination(point);
    }
    virtual protected void MoveAggro()
    {
        SetDestination(target.position + ((transform.position - target.position).normalized * distanceBetweenEnemyToStop));
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
    }

    virtual protected void ActivateAbility()
    {

    }

    virtual protected IEnumerator AbilityCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(abilityCooldown);
            ActivateAbility();
        }
    }
}

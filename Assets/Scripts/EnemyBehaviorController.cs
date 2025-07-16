using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyBehaviorController : MonoBehaviour
{
    private EnemyStateController stateController;
    public GameObject player;
    private GameObject treasure;
    private bool isAttacking;
    public bool isInRange;
    public Transform target;
    NavMeshAgent agent;
    bool SEARCHING = true;
    
    public float searchRadius = 5f;
    public float exploreInterval = 2f;
    private float exploreTimer;
    private List<Vector3> visitedPoints = new List<Vector3>();
    float detectionDistance = 1.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<EnemyStateController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        treasure = GameObject.FindGameObjectWithTag("Treasure");
        target = treasure.transform;

    }

   void Update()
{
    if (!stateController.IsDead)
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        float treasureDistance = Vector3.Distance(transform.position, treasure.transform.position);
            
            // Priority: Player > Treasure
            if (playerDistance < detectionDistance) // <-- Adjust detection range
            {
                SEARCHING = false;
                SetTarget(player.transform.position);
            }
            else if (playerDistance > detectionDistance && treasureDistance > detectionDistance)
            {
                SEARCHING = true;
                SearchForPath();
            }
            else if (treasureDistance < detectionDistance)
            {
                SEARCHING = false;
                SetTarget(treasure.transform.position);
            }

            else if (SEARCHING)
            {
                SearchForPath();
            }
    }
}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entered");
        if (collision.gameObject == player)
        {
            isInRange = true;
            stateController.SetState(EnemyState.ATTACKING);
            
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player && !isAttacking)
        {
            isInRange = true;

            stateController.SetState(EnemyState.ATTACKING);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exited");
        if (collision.gameObject == player)
        {
            isInRange = false;
            if (!stateController.IsDead)
            {
                stateController.SetState(EnemyState.WALKING);
                agent.isStopped = false;
                SetTarget(target.position);

            }
            
        }
        
    }
void SearchForPath()
{
    exploreTimer += Time.deltaTime;
    if (exploreTimer < exploreInterval) return;

    exploreTimer = 0f;

    const int maxAttempts = 10;
    for (int i = 0; i < maxAttempts; i++)
    {
        Vector3 randomDirection = Random.insideUnitCircle * searchRadius;
        Vector3 candidate = transform.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(candidate, out hit, 2f, NavMesh.AllAreas))
        {
            Vector3 destination = hit.position;

            bool tooClose = false;
            foreach (var visited in visitedPoints)
            {
                if (Vector3.Distance(destination, visited) < detectionDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                visitedPoints.Add(destination);
                SetTarget(destination);
                return; 
            }
        }
    }

    
    Debug.LogWarning("Enemy failed to find valid exploration point after max attempts.");
}
    void SetTarget(Vector3 target)
    {
        agent.ResetPath();
        agent.SetDestination(target);
    }
}
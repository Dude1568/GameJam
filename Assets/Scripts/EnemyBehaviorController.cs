using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;

public class EnemyBehaviorController : MonoBehaviour
{
    private EnemyStateController stateController;
    public GameObject player;
    private GameObject treasure;
    private bool isAttacking;
    public bool isInRange;
    public Vector3 target;
    NavMeshAgent agent;
    bool SEARCHING = true;
    
    public float searchRadius = 5f;
    public float exploreInterval = 2f;
    private float exploreTimer;
    private List<Vector3> visitedPoints = new List<Vector3>();
    float detectionDistance = 1.5f;
    bool pathBlocked=false;
    EnemyAttack attack;
    private void Awake()
    {
        attack = GetComponent<EnemyAttack>();
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<EnemyStateController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        treasure = GameObject.FindGameObjectWithTag("Treasure");
        target = treasure.transform.position;

    }

   void Update()
{

        if (!stateController.IsDead)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            float treasureDistance = Vector3.Distance(transform.position, treasure.transform.position);
            NavMeshPath pathToTreasure;
            if (!HasPathToTreasure(out pathToTreasure))
            {
                AttackBlockingBarricade();
            }
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

            if (!stateController.IsDead && !stateController.IsAttacking)
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        stateController.SetState(EnemyState.IDLE);
                }
                else
                {
                    stateController.SetState(EnemyState.WALKING);
                }
            }

            
        }
}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entered");
        if (collision.gameObject == player)
        {
            isInRange = true;

        }
        else if (collision.gameObject.CompareTag("Barricade"))
        {
            stateController.SetState(EnemyState.ATTACKING);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player && !isAttacking)
        {
            isInRange = true;

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
                agent.isStopped = false;
                SetTarget(treasure.transform.position);

            }

        }else if (collision.gameObject.CompareTag("Barricade"))
        {
            stateController.SetState(EnemyState.IDLE);
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
                    target = destination;
                    SetTarget(target);
                    return;
                }
            }
        }


        Debug.LogWarning("Enemy failed to find valid exploration point after max attempts.");
    }
    bool HasPathToTreasure(out NavMeshPath path)
    {
        path = new NavMeshPath();
        return NavMesh.CalculatePath(transform.position, treasure.transform.position, NavMesh.AllAreas, path)
            && path.status == NavMeshPathStatus.PathComplete;
    }

    void AttackBlockingBarricade()
    {
        List<GameObject> obstacles = FindBarricades();
        Vector3 nearest = new Vector3(100000f, 100000f, 0.2f);
        GameObject attackTarget=null;
        foreach (GameObject barricade in obstacles)
        {
            if (Vector3.Distance(barricade.transform.position, gameObject.transform.position) < Vector3.Distance(nearest, gameObject.transform.position))
            {
                nearest = barricade.transform.position;
                if (nearest == barricade.transform.position)
                {
                    attackTarget = barricade;
                    setAttackTarget(attackTarget);
                }
            }
        }
        agent.SetDestination(nearest);


    }
    void setAttackTarget(GameObject attackTarget)
    {
        attack.SetTarget(attackTarget);
    }
    List<GameObject> FindBarricades()
    {
        List<GameObject> obstacles = new List<GameObject>();

        GameObject[] allBarricades = GameObject.FindGameObjectsWithTag("Barricade");

        foreach (GameObject barricade in allBarricades)
        {
            obstacles.Add(barricade);
        }

        return obstacles;
    }
    void SetTarget(Vector3 target)
    {
        agent.ResetPath();
        agent.SetDestination(target);
        
    }
}
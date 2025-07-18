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
    public bool SEARCHING = true;

   [SerializeField] float searchRadius = 5f;
    float currentSearchRadius;
    [SerializeField] float exploreInterval = 2f;
    private float exploreTimer;
    private List<Vector3> visitedPoints = new List<Vector3>();
    [SerializeField] float detectionDistance = 1.5f;
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
            // Priority: Player > Treasure 
            if (playerDistance < detectionDistance) // <-- Adjust detection range
            {
                SEARCHING = false;
                SetTarget(player.transform.position);
                agent.SetDestination(player.transform.position);
            }
            else if (treasureDistance < detectionDistance)
            {
                SEARCHING = false;
                SetTarget(treasure.transform.position);
                agent.SetDestination(treasure.transform.position);
            }
            else if (!HasPathToTreasure(out pathToTreasure))
            {
                AttackBlockingBarricade();
            }
            else if (playerDistance > detectionDistance && treasureDistance > detectionDistance && SEARCHING)
            {
                
                SearchForPath();
            }
            if (!SEARCHING && !isInRange )
            {
                // If both are out of detection range, resume searching
                if (playerDistance > detectionDistance && treasureDistance > detectionDistance)
                {
                    SEARCHING = true;
                    SetTarget(treasure.transform.position);
                    Debug.Log("[Fallback] Target lost. Resuming search.");
                }
            }




            //handles state control
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
            else if (stateController.IsAttacking)
            {
                if (attack.currentTarget == null)
                {
                    stateController.SetState(EnemyState.IDLE);
                }
            }


        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == player)
        {
            isInRange = true;

        }
        else if (collision.gameObject.CompareTag("Barricade"))
        {
            attack.SetTarget(collision.gameObject);
            stateController.SetState(EnemyState.ATTACKING);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player && !isAttacking)
        {
            isInRange = true;

        }
        else if (collision.gameObject.CompareTag("Barricade"))
        {
            attack.attack(collision.gameObject);
            stateController.SetState(EnemyState.ATTACKING);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player||collision.gameObject.CompareTag("Barricade"))
        {
            
            SEARCHING = true;
            isInRange = false;
            if (!stateController.IsDead)
            {
                SetTarget(treasure.transform.position);
                

            }
        }
    }

    bool HasPathToTreasure(out NavMeshPath path)
    {
        path = new NavMeshPath();
        bool success = NavMesh.CalculatePath(new Vector3 (transform.position.x,transform.position.y,0), treasure.transform.position, NavMesh.AllAreas, path);

        //Debug.Log($"[PathToTreasure] Success: {success}, Status: {path.status}, " +
         //       $"From: {transform.position}, To: {treasure.transform.position}");
        bool DoesPath = success && path.status == NavMeshPathStatus.PathComplete;
        //Debug.Log(DoesPath);
        return DoesPath;
    }
    void SearchForPath()
    {
        Debug.Log("searching" + this);
        exploreTimer += Time.deltaTime;
        if (exploreTimer < exploreInterval) return;

       if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance*2)
    return;
        exploreTimer = 2f; // reset timer

        int maxAttempts = 100;
        bool foundDestination = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 random2D = Random.insideUnitCircle * currentSearchRadius;
            Vector3 candidate = new Vector3(transform.position.x + random2D.x, transform.position.y + random2D.y, transform.position.z);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, 5f, NavMesh.AllAreas))
            {
                Vector3 destination = hit.position;


                bool tooClose = false;
                foreach (var visited in visitedPoints)
                {
                    if (Vector3.Distance(destination, visited) < searchRadius)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                   // Debug.Log($"[Explore] Found point: {destination}");
                    visitedPoints.Add(destination);
                    SetTarget(destination);
                    agent.SetDestination(destination);
                    foundDestination = true;
                    break;
                }
            }
            else
            {
                //Debug.Log($"[Explore] Failed to find NavMesh near: {candidate}");
            }
        }
   
            if (!foundDestination&&currentSearchRadius < 80)
            {
                currentSearchRadius *= 1.5f;

            }
            else
            {
                currentSearchRadius = searchRadius; // Reset to default 
            }
        
        
    }



    void AttackBlockingBarricade()
    {
        Debug.Log("destroy the obstacles");
        List<GameObject> obstacles = FindBarricades();
        Vector3 nearest = treasure.transform.position;
        GameObject attackTarget = null;
        foreach (GameObject barricade in obstacles)
        {
            if (Vector3.Distance(barricade.transform.position, gameObject.transform.position) < Vector3.Distance(nearest, gameObject.transform.position))
            {
                nearest = barricade.transform.position;
                if (nearest == barricade.transform.position)
                {
                    attackTarget = barricade;
                    setAttackTarget(attackTarget);
                    target = attackTarget.transform.position;
                    agent.SetDestination(attackTarget.transform.position);
                }
            }
        }



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
        this.target = target;
        //Debug.Log("setting target " + target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentSearchRadius);


    }
}
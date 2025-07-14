using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviorController : MonoBehaviour
{
    private EnemyStateController stateController;
    public GameObject player;
    private GameObject treasure;
    private bool isAttacking;
    public bool isInRange;
    public Transform target;
    NavMeshAgent agent;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<EnemyStateController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        treasure = GameObject.FindGameObjectWithTag("Treasure");
        target = treasure.transform;
        SetTarget(target);

    }

    void Update()
    {
        
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
                SetTarget(target);

            }
            
        }
        
    }
    void SetTarget(Transform target)
    {
        agent.ResetPath();
        agent.SetDestination(target.position);
    }
}
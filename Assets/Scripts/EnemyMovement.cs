using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStateController stateController;
    private EnemyBehaviorController behavior;
    private GameObject player;
    public bool isFrozen = false;
    NavMeshAgent agent;

    private void Awake()
    {
        behavior = GetComponent<EnemyBehaviorController>();
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<EnemyStateController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (stateController.IsAttacking && !isFrozen)
            freeze(true);
        else if (isFrozen && !stateController.IsDead && !stateController.IsAttacking)
            freeze(false);

        if (!stateController.IsDead)
            movingDirection();
    }

    void movingDirection()
    {
        Vector3 currentTarget = behavior.target;  
        Vector2 moveDirection = currentTarget - transform.position;
        UpdateFacing(moveDirection);
    }

    public void UpdateFacing(Vector3 moveDirection)
    {
        if (moveDirection.magnitude > 0.01f)
            GetComponent<SpriteRenderer>().flipX = moveDirection.x < 0;
    }

    public void freeze(bool Freeze)
    {
        Debug.Log(Freeze.ToString());
        agent.isStopped = Freeze;
        agent.Warp(transform.position);  
        agent.SetDestination(behavior.target);
        isFrozen = Freeze;
    }
}

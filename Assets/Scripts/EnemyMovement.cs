using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStateController stateController;
    EnemyBehaviorController behavior;
    private GameObject player;
    public bool isFrozen = false;
    NavMeshAgent agent;
    Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<EnemyStateController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
            if (stateController.IsAttacking&&isFrozen==false)
                freeze(true);
            else if(isFrozen==true&&!stateController.IsDead&&!stateController.IsAttacking)
                freeze(false);
        if (!stateController.IsDead)
            movingDirection();
    }
    void movingDirection()
    {
        if (!stateController.IsDead)
        {
            Vector2 moveDirection = player.transform.position - transform.position;
            UpdateFacing(moveDirection);
        }
    }

    public void UpdateFacing(Vector3 moveDirection)
    {
        GetComponent<SpriteRenderer>().flipX = moveDirection.x < 0;
    }

    public void freeze(bool Freeze)
    {
        Debug.Log(Freeze.ToString());
        agent.isStopped = Freeze;
        agent.Warp(transform.position);
        agent.SetDestination(behavior.target.position);
        isFrozen = Freeze;
        
    }
}
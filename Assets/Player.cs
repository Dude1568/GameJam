
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 movement;

    [SerializeField] float attackRadius;
    [SerializeField] CircleCollider2D attackCollider;
    [SerializeField] int damage;
    [SerializeField] float attackCooldown;
    [SerializeField] Animator playerAnimator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<Transform> enemiesInRange = new List<Transform>();
    public NavMeshAgent agent;
    bool isAttackReady = true;
    Coroutine attackCoroutine;

    void Awake()
    {

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void Update()
    {
       // transform.rotation = Quaternion.identity;

        movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) movement.y += 1;
        if (Input.GetKey(KeyCode.S)) movement.y -= 1;
        if (Input.GetKey(KeyCode.D)) movement.x += 1;
        if (Input.GetKey(KeyCode.A)) movement.x -= 1;

        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            Vector3 currentNavPos = new Vector3(transform.position.x, transform.position.y, 0);

            Vector3 targetNavPos = currentNavPos + new Vector3(movement.x,movement.y, 0 );

            agent.SetDestination(targetNavPos);
            playerAnimator.SetBool("IsWalking", true);
            if(isAttackReady)
            spriteRenderer.flipX = movement.x <= 0;
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
            agent.ResetPath();
        }


    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Count == 0)
                attackCoroutine = StartCoroutine(AttackCycle());

            enemiesInRange.Add(other.transform);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
            if (enemiesInRange.Count == 0 && attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }
    void Attack()
    {
        enemiesInRange[0].GetComponent<EnemyHealth>().TakeDamage(damage);
        if (enemiesInRange[0].transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        playerAnimator.SetTrigger("OnAttacking");
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitUntil(() => isAttackReady);
            isAttackReady = false;
            Attack();
        }
    }
}

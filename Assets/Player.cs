using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] float attackRadius;
    [SerializeField] CircleCollider2D attackCollider;
    [SerializeField] int damage;
    [SerializeField] float attackCooldown;
    [SerializeField] Animator playerAnimator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<Transform> enemiesInRange = new List<Transform>();
    bool isAttackReady = true;
    Coroutine attackCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movement.y += 1;
        if (Input.GetKey(KeyCode.S))
            movement.y -= 1;
        if (Input.GetKey(KeyCode.D))
            movement.x += 1;
        if (Input.GetKey(KeyCode.A))
            movement.x -= 1;

        movement.Normalize(); // keep speed consistent diagonally
        if (movement != Vector2.zero)
        {
            playerAnimator.SetBool("IsWalking", true);
            if (movement.x <= 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
        else
            playerAnimator.SetBool("IsWalking", false);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            {
                if (enemiesInRange.Count == 0)
                {
                    attackCoroutine = StartCoroutine(AttackCycle());
                }
                enemiesInRange.Add(other.transform);
            }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
            if (enemiesInRange.Count == 0)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    void Attack()
    {
        if (enemiesInRange.Count == 0)
            return;
        if (enemiesInRange[0].transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        enemiesInRange[0].GetComponent<EnemyHealth>().TakeDamage(damage);
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

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
}

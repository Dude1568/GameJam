
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
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
    EnemyStateController state;

    bool isInitialized;
    [SerializeField] Vector2 footStepPitch = new Vector2(0.85f, 1.1f);
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip[] footStepClipsLeft;
    [SerializeField] AudioClip[] footStepClipsRight;
    void Awake()
    {
        state =GetComponent<EnemyStateController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.4f); isInitialized = true;
    }

    void Update()
    {
        if(state.IsDead || !isInitialized) return;
       // transform.rotation = Quaternion.identity;

        //movement = Vector2.zero;
        //if (Input.GetKey(KeyCode.W)) movement.y += 1;
        //if (Input.GetKey(KeyCode.S)) movement.y -= 1;
        //if (Input.GetKey(KeyCode.D)) movement.x += 1;
        //if (Input.GetKey(KeyCode.A)) movement.x -= 1;
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //movement = movement.normalized;

        //if (movement != Vector2.zero && !state.IsDead)
        //{
        //    Vector3 currentNavPos = new Vector3(transform.position.x, transform.position.y, 0);

        //    Vector3 targetNavPos = currentNavPos + new Vector3(movement.x, movement.y, 0);

        //    agent.SetDestination(targetNavPos);
        //    playerAnimator.SetBool("IsWalking", true);
        //    if (isAttackReady)
        //        spriteRenderer.flipX = movement.x <= 0;
        //}
        //else
        //{
        //    playerAnimator.SetBool("IsWalking", false);
        //    agent.ResetPath();
        //}

        agent.SetDestination(transform.position + (Vector3)movement);
        if (isAttackReady && movement != Vector2.zero)
            spriteRenderer.flipX = movement.x <= 0;
        playerAnimator.SetBool("IsWalking", agent.velocity.sqrMagnitude > 0.1f);
    }
    
 void FixedUpdate()
    {
        if (state.IsDead) return;

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesInRange.Clear();

        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRadius)
            {
                enemiesInRange.Add(enemy.transform);
            }
        }

 
        if (enemiesInRange.Count > 0)
        {
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackCycle());
        }
        else
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }
    void Attack()
    {
        if (state.IsDead) return;
        StartCoroutine(StartCooldown());
        if (enemiesInRange.Count == 0)
        {
            StopCoroutine(AttackCycle());

            return;
        }
        if (enemiesInRange[0].transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
            
        else
            spriteRenderer.flipX = false;

        enemiesInRange[0].GetComponent<EnemyHealth>().TakeDamage(damage);
        
        
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
            if (state.IsDead) yield break;
            playerAnimator.SetTrigger("OnAttacking");
            isAttackReady = false;
            
            
        }
    }

    public void PlayFootStepClipLeft()=> Soundmanager.instance.PlaySound(footStepClipsLeft[Random.Range(0, footStepClipsLeft.Length)], footStepPitch,footstepSource);
    public void PlayFootStepClipRight()=> Soundmanager.instance.PlaySound(footStepClipsRight[Random.Range(0, footStepClipsRight.Length)], footStepPitch,footstepSource);
}

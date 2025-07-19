using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeMonster : Monster
{
    [SerializeField] float projectileSpeed;
    [SerializeField] Transform projectilePrefab;
    protected override void MoveAggro()
    {
        NoTargetCheck();
        if (target == null)
            return;
        Debug.Log("test");
        if (Physics2D.Raycast(transform.position, target.position - transform.position, (target.position - transform.position).magnitude, 1 << 7))
        {
            if (isAbilityCycleActive)
            {
                StopCoroutine(abilityCycle);
                isAbilityCycleActive = false;
                abilityCycle = null;
                navMeshAgent.speed = chaseSpeed;
                SetDestination(target.position);
                animator.SetBool("IsWalking", true);
            }
        }
        else
        {
            if (!isAbilityCycleActive)
            {
                abilityCycle = StartCoroutine(AttackCycle());
                isAbilityCycleActive = true;
                navMeshAgent.speed = 0;
                animator.SetBool("IsWalking", false);
            }
        }
    }

    protected override void Attack()
    {
        StartCoroutine(RangeAttack());
        if (target.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    IEnumerator RangeAttack()
    {
        animator.SetTrigger("OnAttacking");
        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, int.MaxValue, 1 << 7);
        while (((Vector2)projectile.transform.position - hit.point).magnitude > 0.1f)
        {
            projectile.transform.position += ((Vector3)hit.point - projectile.transform.position).normalized * projectileSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(projectile.gameObject);
    }
}

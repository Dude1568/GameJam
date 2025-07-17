using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeMonster : Monster
{
    bool isAbilityCycleActive = false;
    Coroutine abilityCycle;
    [SerializeField] float projectileSpeed;
    [SerializeField] Transform projectilePrefab;
    protected override void MoveAggro()
    {
        if (Physics2D.Raycast(transform.position, target.position - transform.position, (target.position - transform.position).magnitude, 1 << 7))
        {
            if (isAbilityCycleActive)
            {
                StopCoroutine(abilityCycle);
                isAbilityCycleActive = false;
                abilityCycle = null;
                navMeshAgent.speed = chaseSpeed;
                SetDestination(target.position);
            }
        }
        else
        {
            if (!isAbilityCycleActive)
            {
                abilityCycle = StartCoroutine(AttackCycle());
                isAbilityCycleActive = true;
                navMeshAgent.speed = 0;
            }
        }
    }

    protected override void Attack()
    {
        StartCoroutine(RangeAttack());
    }

    IEnumerator RangeAttack()
    {
        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, GridManager.Instance.GridOrigin);
        Vector3 enemyPosAtTheMoment = target.transform.position;
        while ((projectile.transform.position - enemyPosAtTheMoment).magnitude > 0.1f)
        {
            projectile.transform.position += (enemyPosAtTheMoment - projectile.transform.position).normalized * projectileSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(projectile.gameObject);
        target.GetComponent<EnemyHealth>().TakeDamage(damage);
    }
}

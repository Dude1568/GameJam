using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
     public GameObject currentTarget{ get; private set; }

    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }
    [SerializeField] int damage;
    public virtual void Attack(GameObject attacked)
    {
        attacked = currentTarget;
        EnemyHealth attackedHealth = attacked.GetComponent<EnemyHealth>();
        attackedHealth.TakeDamage(damage);
    }
}

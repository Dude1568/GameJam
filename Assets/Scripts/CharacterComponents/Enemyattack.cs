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
        if(attacked == null) return;

        attacked = currentTarget;
        EnemyHealth attackedHealth = attacked.GetComponent<EnemyHealth>();
        if (attackedHealth!=null)
        attackedHealth.TakeDamage(damage);
    }
}

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
    public void attack(GameObject attacked)
    {
        attacked = currentTarget;
        EnemyHealth attackedHealth = attacked.GetComponent<EnemyHealth>();
        attackedHealth.TakeDamage(damage);
    }
}

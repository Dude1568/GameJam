using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : EnemyAttack
{
    public GameObject projectile;
    public float projectileSpeed = 10f;
    public override void Attack(GameObject attacked)
    {
        attacked = currentTarget;
        if (attacked == null) return;

        
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Projectile is missing a Rigidbody2D component.");
            return;
        }

        Vector2 direction = (attacked.transform.position - transform.position).normalized;

        rb.velocity = direction * projectileSpeed;
    }
}

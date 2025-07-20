using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    enum ProjectileType { FRIENDLY, ENEMY };
    [SerializeField] ProjectileType projectiletype;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem impactPS;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (projectiletype == ProjectileType.ENEMY)
        {
            DamageMonsters(collision);
        }
        else
        {
            DamageAdventerers(collision);
        }
    }
    void DamageMonsters(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("Player") || collision.CompareTag("Barricade"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
        if(impactPS != null) { Instantiate(impactPS,transform.position,Quaternion.identity);}
    }
    void DamageAdventerers(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
        if (impactPS != null) { Instantiate(impactPS, transform.position, Quaternion.identity); }
    }
}

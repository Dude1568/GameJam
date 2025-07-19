using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 5;

    private EnemyStateController stateController;

    private void Awake()
    {
        stateController = GetComponent<EnemyStateController>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0 )
        {if (stateController != null)
            {

                stateController.SetState(EnemyState.DEAD);
                StartCoroutine(Die());
            }
            else
            {
                StartCoroutine(Die());
            }

        }
    }

    IEnumerator Die()
    {
        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        // freeze movement 
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
            movement.freeze(true);

        // Wait for animation to finish 
        yield return new WaitForSeconds(2f);
        if(gameObject.CompareTag("Enemy"))
        GetComponent<AdventurerItems>().SpawnItemsByCost();
        if(!gameObject.CompareTag("Player"))
        Destroy(gameObject);
    }
}
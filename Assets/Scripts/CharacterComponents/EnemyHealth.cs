using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 5;
    private EnemyStateController stateController;
    [SerializeField] ParticleSystem diePS;

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

        // Wait for animation to finish d
        yield return new WaitForSeconds(2f);
        if (gameObject.CompareTag("Enemy"))
        {
            GetComponent<AdventurerItems>().SpawnItemsByCost();
        }
        else if (!gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if(diePS != null) { Instantiate(diePS,transform.position,Quaternion.identity); }
    }
}
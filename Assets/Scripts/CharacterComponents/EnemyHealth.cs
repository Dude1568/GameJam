using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 5;
    private EnemyStateController stateController;
    [SerializeField] ParticleSystem diePS;
    SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public int flashCount = 3;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateController = GetComponent<EnemyStateController>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            if (stateController != null)
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
            Destroy(gameObject);
        }
        else if (!gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (diePS != null)
        {
            Instantiate(diePS, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
        IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

}
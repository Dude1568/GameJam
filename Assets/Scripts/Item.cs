using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public int Value;
    public static event Action KeyReturned;

    void OnTriggerEnter2D(Collider2D other)
    {
       Debug.Log("Other tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            TreasureCheck(other);
            GoldTracker.GainGold(Value);

        }
        if (other.CompareTag("Enemy"))
        {
            TreasureCheck(other);

        }
        Destroy(gameObject);
    }
    void TreasureCheck(Collider2D other)
    {
            if (gameObject.CompareTag("Key"))
            {

                if (other.gameObject.CompareTag("Enemy"))
                {
                    EnemyBehaviorController.KEYHOLDER = other.gameObject;
                    
                    Destroy(gameObject);
                }
                else if (other.CompareTag("Player"))
                {
                    KeyReturned?.Invoke();

                }
                
            }
    }
}

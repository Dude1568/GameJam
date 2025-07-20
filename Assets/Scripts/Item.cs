using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public int Value;
    public static event Action KeyReturned;
    public static event Action KeyTaken;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TreasureCheck(other);
            GoldTracker.GainGold(Value);

        }
        if (other.CompareTag("Enemy"))
        {
            TreasureCheck(other);

        }
        
    }
    void TreasureCheck(Collider2D other)
    {
            if (gameObject.CompareTag("Treasure"))
            {

            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyBehaviorController.KEYHOLDER = other.gameObject;
                KeyTaken.Invoke();
                Destroy(gameObject);
            }
            else if (other.CompareTag("Player"))
            {
                KeyReturned?.Invoke();
                Destroy(gameObject);

            }
                
            }
    }
}

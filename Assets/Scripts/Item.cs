using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int Value;
    public static event System.Action KeyReturned;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TreasureCheck(other);
            GoldTracker.GainGold(Value);
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            TreasureCheck(other);

        }
    }
    void TreasureCheck(Collider2D other)
    {
            if (!gameObject.CompareTag("Treasure"))
            {
                EnemyBehaviorController.KEYFOUND = other.CompareTag("Player");
                if(other.gameObject.CompareTag("Enemy"))
                {
                    EnemyBehaviorController.KEYHOLDER = other.gameObject;
                }
                else if(other.CompareTag("Player"))
                {
                    KeyReturned?.Invoke();
                }
                Destroy(gameObject);
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class TreasureScript : MonoBehaviour
{
    [SerializeField] Sprite OpenedChest;
    [SerializeField] Sprite Closedchest;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        EnemyBehaviorController.KeyStolen += OpenChest;
        Item.KeyReturned += CloseChest;
    }

    private void OnDisable()
    {
        EnemyBehaviorController.KeyStolen -= OpenChest;
        Item.KeyReturned -= CloseChest;
    }

    void OpenChest()
    {
 
        spriteRenderer.sprite = OpenedChest;
        
    }
    void CloseChest()
    {
     
        EnemyBehaviorController.KEYFOUND = false;
        EnemyBehaviorController.KEYHOLDER = null;
        spriteRenderer.sprite = Closedchest;
        
        
    }

}

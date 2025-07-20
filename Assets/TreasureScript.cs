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
    public static event Action ChangeTreasure;
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
        SetToUntagged();
        spriteRenderer.sprite = OpenedChest;
        
    }
    void CloseChest()
    {
        SetToTreasure();
        spriteRenderer.sprite = Closedchest;
        if(GameObject.FindGameObjectsWithTag("Enemy").Length >0)
        ChangeTreasure.Invoke();
    }
    
    public void SetToTreasure()
    {
        gameObject.tag = "Treasure";
    }

    public void SetToUntagged()
    {
        gameObject.tag = "Untagged";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreasureScript : MonoBehaviour
{
    private void OnEnable()
    {
        Item.KeyReturned += SetToTreasure;
    }

    private void OnDisable()
    {
        Item.KeyReturned -= SetToTreasure;
    }

    void OpenChest()
    {

    }
    void CloseChest (){
        
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

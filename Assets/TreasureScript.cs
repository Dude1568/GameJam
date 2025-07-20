using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreasureScript : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnKeyCollected += SetToTreasure();
    }

    private void OnDisable()
    {
        GameEvents.OnKeyCollected -= SetToTreasure();
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

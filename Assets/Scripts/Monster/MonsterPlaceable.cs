using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlaceable : Placeable
{
    public override void OnPlace()
    {
        GetComponent<Monster>().Activate();
    }
}

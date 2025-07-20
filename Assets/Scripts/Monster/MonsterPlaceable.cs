using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlaceable : Placeable
{
    public override void OnPlace()
    {
        base.OnPlace();
        GetComponent<Monster>().Activate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barricade : Trap
{
    [SerializeField] NavMeshObstacle navMeshObstacle;

    protected override void Start()
    {
        base.Start();
        navMeshObstacle.enabled = true;
    }
    public override void Trigger(GameObject target)
    {

    }
    
}

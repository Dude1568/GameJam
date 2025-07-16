using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlaceable : Placeable
{
    bool isTrapInTheWay = false;
    List<Transform> trapsInCollider;
    public override bool CheckPlacmentRequirments()
    {
        return base.CheckPlacmentRequirments();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            trapsInCollider.Add(collision.transform);
            isTrapInTheWay = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            trapsInCollider.Remove(collision.transform);
            if(trapsInCollider.Count == 0)
                isTrapInTheWay = false;
        }
    }
}

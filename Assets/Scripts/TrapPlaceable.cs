using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlaceable : Placeable
{
    bool isTrapInTheWay = false;
    List<Transform> trapsInCollider = new List<Transform>();
    public override bool CheckPlacmentRequirments()
    {
        Debug.Log(isTrapInTheWay);
        return base.CheckPlacmentRequirments() && (!isTrapInTheWay);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap")||collision.CompareTag("Barricade"))
        {
            trapsInCollider.Add(collision.transform);
            isTrapInTheWay = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap")||collision.CompareTag("Barricade"))
        {
            trapsInCollider.Remove(collision.transform);
            if (trapsInCollider.Count == 0)
            {
                isTrapInTheWay = false;
            }
        }
    }
}

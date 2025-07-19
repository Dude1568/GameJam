using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlaceable : Placeable
{
    bool isTrapInTheWay = false;
    List<Transform> trapsInCollider = new List<Transform>();

    public override void OnPlace()
    {
        base.OnPlace();
        GetComponent<Trap>().IsActive = true;
    }
    public override bool CheckPlacmentRequirments()
    {
        bool baseValid = base.CheckPlacmentRequirments();
        bool trapValid = !isTrapInTheWay;
        
        bool finalResult = baseValid && trapValid;

        spriteRenderer.color = finalResult ? Color.white : Color.red;
        return finalResult;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name);
        if (collision.CompareTag("Trap") || collision.CompareTag("Barricade"))
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
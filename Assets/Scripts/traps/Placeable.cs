using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected BoxCollider2D placementCollider;
    public int cost;
    public bool canAfford;
    public virtual void OnPlace()
    {
        GoldTracker.SpendGold(cost);

    }

    public virtual bool CheckPlacmentRequirments()
    {
        if (cost <= GoldTracker.gold)
            canAfford = true;
        else
            canAfford = false;
        if (GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(placementCollider.size.x * placementCollider.transform.localScale.x, placementCollider.size.y * placementCollider.transform.localScale.y) * 0.5f))
                && GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(placementCollider.size.x * placementCollider.transform.localScale.x, -placementCollider.size.y * placementCollider.transform.localScale.y) * 0.5f))
                && GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(-placementCollider.size.x * placementCollider.transform.localScale.x, placementCollider.size.y * placementCollider.transform.localScale.y) * 0.5f))
                && GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(-placementCollider.size.x * placementCollider.transform.localScale.x, -placementCollider.size.y * placementCollider.transform.localScale.y) * 0.5f)))
        {
            spriteRenderer.color = Color.white;
            return true;
        }
        spriteRenderer.color = Color.red;
        return false;
    }
}

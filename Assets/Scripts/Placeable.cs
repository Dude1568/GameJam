using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected BoxCollider2D placementCollider;
    public virtual void OnPlace()
    {

    }

    public virtual bool CheckPlacmentRequirments()
    {
        if (GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(placementCollider.size.x, placementCollider.size.y) * 0.5f))
            && GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(placementCollider.size.x, -placementCollider.size.y) * 0.5f))
            && GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(-placementCollider.size.x, placementCollider.size.y) * 0.5f))
            && GridManager.Instance.DefaultCells.Any(c => c.Floor.bounds.Contains(placementCollider.bounds.center + new Vector3(-placementCollider.size.x, -placementCollider.size.y) * 0.5f)))
        {
            return true;
        }
        return false;
    }
}

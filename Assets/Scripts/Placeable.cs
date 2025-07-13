using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] BoxCollider2D placementCollider;
    public virtual void OnPlace()
    {

    }

    public virtual bool CheckPlacmentRequirments()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, int.MaxValue, 1 << 6);
        if (hit.collider != null
            && hit.collider.bounds.Contains(placementCollider.bounds.center + new Vector3(placementCollider.size.x, placementCollider.size.y) * 0.5f )
            && hit.collider.bounds.Contains(placementCollider.bounds.center + new Vector3(placementCollider.size.x, -placementCollider.size.y) * 0.5f )
            && hit.collider.bounds.Contains(placementCollider.bounds.center + new Vector3(-placementCollider.size.x, placementCollider.size.y) * 0.5f )
            && hit.collider.bounds.Contains(placementCollider.bounds.center + new Vector3(-placementCollider.size.x, -placementCollider.size.y) * 0.5f ))
        {
            return true;
        }
        return false;
    }
}

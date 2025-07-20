using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Placeable : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected BoxCollider2D placementCollider;
    public int cost;
    bool isPlaced = false;

    void Update()
    {
        if(!isPlaced)
            CheckPlacmentRequirments();
    }

    public virtual void OnPlace()
    {
        isPlaced = true;
        spriteRenderer.color = Color.white;
        GoldTracker.SpendGold(cost);
    }

    public virtual bool CheckPlacmentRequirments()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.zero, int.MaxValue, 1 << 7);
        Debug.DrawRay(transform.position, Vector2.zero, Color.magenta, 10f);
        Debug.Log(hit2D.collider == null);
        if (hit2D.collider != null)
        {
            spriteRenderer.color = Color.red;
            return false;
        }
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas))
        {
            spriteRenderer.color = Color.red;
            return false;
        }
        if (cost > GoldTracker.gold)
        {
            spriteRenderer.color = Color.red;
            return false;
        }
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

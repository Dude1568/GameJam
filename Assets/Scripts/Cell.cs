using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsEmpty;
    [SerializeField] Transform leftWall;
    [SerializeField] Transform rightWall;
    [SerializeField] Transform upWall;
    [SerializeField] Transform downWall;
    public Collider2D Floor;
    public Vector2 CoordinatesOnTheGrid;

    public void Init(Vector2 coordinates)
    {
        CoordinatesOnTheGrid = coordinates;
        transform.localPosition = coordinates;
    }

    public bool IsCellUnderAvailable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, int.MaxValue, 1 << 6);
        if (hit.collider != null)
        {
            Cell targetCell = hit.collider.gameObject.GetComponent<Cell>();
            if (targetCell.IsEmpty)
                return true;
        }
        return false;
    }

    public void ReplaceCellUnder()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, int.MaxValue, 1 << 6);
        Cell targetCell = hit.collider.gameObject.GetComponent<Cell>();
        transform.position = targetCell.transform.position;
        targetCell.gameObject.SetActive(false);
    }

    public void TweakWall(Vector2 dir, bool state)
    {
        GetWallByDirection(dir).gameObject.SetActive(state);
    }

    public bool CheckWallState(Vector2 dir, bool state)
    {
        Transform wall = GetWallByDirection(dir);
        if (wall != null)
        {
            return wall.gameObject.activeInHierarchy == state;
        }
        else
            return false;
    }

    Transform GetWallByDirection(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            return upWall;
        }
        if (dir == Vector2.down)
        {
            return downWall;
        }
        if (dir == Vector2.right)
        {
            return rightWall;
        }
        if (dir == Vector2.left)
        {
            return leftWall;
        }
        return null;
    }

    public List<Vector2> GetActiveWalls()
    {
        List<Vector2> directions = new List<Vector2>();
        if ((upWall != null) && upWall.gameObject.activeInHierarchy)
        {
            directions.Add(Vector2.up);
        }
        if ((downWall != null) && downWall.gameObject.activeInHierarchy )
        {
            directions.Add(Vector2.down);
        }
        if ((rightWall != null) && rightWall.gameObject.activeInHierarchy)
        {
            directions.Add(Vector2.right);
        }
        if ((leftWall != null) && leftWall.gameObject.activeInHierarchy)
        {
            directions.Add(Vector2.left);
        }
        return directions;
    }
}

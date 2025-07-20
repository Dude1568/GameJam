using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecisionCell : MonoBehaviour, IPointerClickHandler
{
    public event Action<DecisionCell> OnCellClick;
    public Vector2 Coordinates;
    public List<Vector2> WallsDirections = new List<Vector2>();
    [SerializeField] Transform imagePrefab;
    float spacing;
    public void Init(Vector2 coord, float spacing = 1f)
    {
        this.spacing = spacing;
        Coordinates = coord;
        transform.localPosition = coord * spacing;
    }

    public void AddWallDirection(Vector2 dir)
    {
        Instantiate(imagePrefab, transform.position + (Vector3)(dir * spacing), Quaternion.identity, transform);
        WallsDirections.Add(dir);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCellClick?.Invoke(this);
    }
}

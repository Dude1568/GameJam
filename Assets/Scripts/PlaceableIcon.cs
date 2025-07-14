using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Placeable placeablePrefab;
    Placeable placeable;

    public void OnPointerClick(PointerEventData eventData)
    {
        placeable = Instantiate(placeablePrefab, GridManager.Instance.GridOrigin);
        StartCoroutine(PlacingProcess());

    }

    IEnumerator PlacingProcess()
    {
        bool isDone = false;
        while (!isDone)
        {
            placeable.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && placeable.CheckPlacmentRequirments())
            {
                isDone = true;
                placeable.OnPlace();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(placeable.gameObject);
                isDone = true;
            }
            yield return null;
        }
    }
}

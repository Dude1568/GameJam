using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlaceableIcon : MonoBehaviour, IPointerClickHandler
{
    public Placeable placeablePrefab;
    Placeable placeable;
    static bool itemHeld=false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!itemHeld)
        {
            itemHeld = true;
            placeable = Instantiate(placeablePrefab, GridManager.Instance.GridOrigin);
            StartCoroutine(PlacingProcess(placeable));
        }



    }

    IEnumerator PlacingProcess(Placeable currentPlaceable)
    {
        bool isDone = false;
        while (!isDone)
        {
            Debug.Log("Still running..."+currentPlaceable.name);
            currentPlaceable.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && currentPlaceable.CheckPlacmentRequirments())
            {
                isDone = true;
                currentPlaceable.OnPlace();
                itemHeld = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(currentPlaceable.gameObject);
                isDone = true;
                itemHeld = false;
            }
            yield return null;
        }
    }
}

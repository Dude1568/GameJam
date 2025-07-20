using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System;
public class PlaceableIcon : MonoBehaviour, IPointerClickHandler
{
    public Placeable placeablePrefab;
    Placeable placeable;
    static bool itemHeld=false;
    int cost;
    void Start()
    {
        cost = Int32.Parse(GetComponentInChildren<TextMeshProUGUI>().text);
        Debug.Log("cost "+cost.ToString());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!itemHeld)
        {
            itemHeld = true;
            placeable = Instantiate(placeablePrefab, GridManager.Instance.GridOrigin);
            placeable.cost = cost;
            StartCoroutine(PlacingProcess(placeable));
            
        }



    }

    IEnumerator PlacingProcess(Placeable currentPlaceable)
    {
        while (true)
        {
            currentPlaceable.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && currentPlaceable.CheckPlacmentRequirments())
            {
                currentPlaceable.OnPlace(); // Deduct gold here

                if (GoldTracker.gold >= currentPlaceable.cost)
                {
                    currentPlaceable = Instantiate(placeablePrefab, GridManager.Instance.GridOrigin);
                    currentPlaceable.cost = cost;
                }
                else
                {
                    Debug.Log("Out of gold, exiting placement mode.");
                    itemHeld = false;
                    break;
                }
            }

            if (Input.GetMouseButtonDown(1)) // Right-click cancels
            {
                Destroy(currentPlaceable.gameObject);
                itemHeld = false;
                break;
            }

            yield return null;
        }
    }
}

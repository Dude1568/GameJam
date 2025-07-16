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
        if (!itemHeld && GoldTracker.gold >= cost)
        {
            itemHeld = true;
            placeable = Instantiate(placeablePrefab, GridManager.Instance.GridOrigin);
            StartCoroutine(PlacingProcess(placeable));
            GoldTracker.SpendGold(cost);
        }



    }

    IEnumerator PlacingProcess(Placeable currentPlaceable)
    {
        bool isDone = false;
        while (!isDone)
        {
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

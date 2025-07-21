using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecisionCell : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public event Action<DecisionCell> OnCellClick;
    public Vector2 Coordinates;
    public List<Vector2> WallsDirections = new List<Vector2>();
    [SerializeField] Transform imagePrefab;
    float spacing;

    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip pressClip;
    [SerializeField] Sprite normalImage;
    [SerializeField] Sprite hoverImage;
    [SerializeField] Sprite downImage;
    SpriteRenderer expansionButton;
    public void Init(Vector2 coord, float spacing = 1f)
    {
        this.spacing = spacing;
        Coordinates = coord;
        transform.localPosition = coord * spacing;
    }

    public void AddWallDirection(Vector2 dir)
    {
        Transform expansionImage = Instantiate(imagePrefab, transform.position + (Vector3)(dir * (spacing-2)), Quaternion.identity, transform);
        expansionButton = expansionImage.GetComponent<SpriteRenderer>();
        if (!GridManager.Instance.IsGameStart)
            expansionImage.GetComponentInChildren<TMP_Text>().text = GridManager.Instance.ExpansionCost.ToString();
        WallsDirections.Add(dir);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCellClick?.Invoke(this);
        Soundmanager.instance.PlaySound(pressClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (expansionButton == null) return;
        expansionButton.sprite = hoverImage;
        Soundmanager.instance.PlaySound(hoverClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (expansionButton == null) return;
        expansionButton.sprite = normalImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (expansionButton == null) return;
        expansionButton.sprite = downImage;
    }
}

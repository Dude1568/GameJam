using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopIconTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string descriptionText;  // This comes from your shop data

    [SerializeField] private GameObject tooltipPanel; // Assign the tooltip panel
    [SerializeField] private TMP_Text tooltipText;     // Assign the tooltip text

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = descriptionText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }
}
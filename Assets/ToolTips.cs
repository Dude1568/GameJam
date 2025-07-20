using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    public void Show(string text)
    {
        tooltipText.text = text;
        tooltipPanel.SetActive(true);
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}

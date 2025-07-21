using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] int startingCold = 120;

    void Start()
    {
        GoldTracker.Initialize(goldText,startingCold);
    }
}
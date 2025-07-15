using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    void Start()
    {
        GoldTracker.Initialize(goldText);
    }
}
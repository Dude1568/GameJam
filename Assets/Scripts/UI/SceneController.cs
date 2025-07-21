using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] int startingCold = 120;
    [SerializeField] AudioClip rewardClip;

    void Start()
    {
        GoldTracker.Initialize(goldText,startingCold, rewardClip);
    }
}
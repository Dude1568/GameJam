using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] int startingCold = 120;
    [SerializeField] AudioClip rewardClip;
    GameObject GAMEOVER;

    void Start()
    {
        GAMEOVER = GameObject.FindGameObjectWithTag("SCENES");
        GAMEOVER.SetActive(false);
        GoldTracker.Initialize(goldText, startingCold, rewardClip);
    }
    void OnEnable()
    {
        EnemyBehaviorController.GAMEOVER += restart;
    }
    void OnDisable()
    {
        EnemyBehaviorController.GAMEOVER -= restart;
    }
    void restart() {
        GAMEOVER.SetActive(true);
    }
}
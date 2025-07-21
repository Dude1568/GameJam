using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public static class GoldTracker
{
    public static int gold = 0;
    private static TextMeshProUGUI tracker;
    private static AudioClip rewardClip;
    public static void Initialize(TextMeshProUGUI trackerText,int startingGold,AudioClip reward)
    {
        rewardClip = reward;
        gold = startingGold;
        tracker = trackerText;
        UpdateDisplay();
    }

    public static void GainGold(int goldGained)
    {
        Soundmanager.instance.PlaySound(rewardClip);
        gold += goldGained;
        UpdateDisplay();
    }

    public static void SpendGold(int goldSpent)
    {
        gold -= goldSpent;
        UpdateDisplay();
    }

    private static void UpdateDisplay()
    {
        if (tracker != null)
            tracker.text = gold.ToString();
    }
}
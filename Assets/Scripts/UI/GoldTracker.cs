using TMPro;

public static class GoldTracker
{
    public static int gold = 0;
    private static TextMeshProUGUI tracker;

    public static void Initialize(TextMeshProUGUI trackerText,int startingGold)
    {
        gold = startingGold;
        tracker = trackerText;
        UpdateDisplay();
    }

    public static void GainGold(int goldGained)
    {
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
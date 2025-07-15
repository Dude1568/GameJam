using UnityEngine;

public enum ItemType
{
    Monster,
    Building
}
[CreateAssetMenu(fileName = "NewPurchasableItem", menuName = "Shop/Purchasable Item")]
public class PurchasableItem : ScriptableObject
{
    public Placeable prefab;
    public int cost;
    [TextArea] public string description;
    public ItemType Type;
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int cost;
    public int Demage;
    public Sprite icon;
    public ItemType itemType;
    public bool isUsed;

    public enum ItemType
    {
        Furniture,
        Monster,
        Trap,
        Present
    }
}

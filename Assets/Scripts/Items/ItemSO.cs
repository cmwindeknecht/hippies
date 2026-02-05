using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public InventoryItemType Type;
    public string Name;
    [TextArea] public string Description;
    public Sprite Sprite;
    public float Weight = 0f;
}

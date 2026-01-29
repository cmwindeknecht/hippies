using UnityEngine;

public abstract class ConsumableEffect : ScriptableObject
{
    public abstract void Use(Player player, InventoryItem item);
}

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemEffect")]
public class RestoreHealthEffect : ConsumableEffect
{
    public override void Use(Player player, InventoryItem item)
    {
        player.RestoreHealth(item.ItemSO.HealthRestored, item.ItemSO.HealthIterations, item.ItemSO.HealthTotalTime);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Consumable/RestoreHealth")]
public class RestoreHealthEffect : ConsumableEffect
{
    public override void Use(Player player, InventoryItem item)
    {
        if (item.ItemSO is not ItemConsumableSO)
        {
            throw new System.Exception("RestoreHealthEffect passed a non ItemConsumableSO!");
        }

        ItemConsumableSO itemSO = (ItemConsumableSO)item.ItemSO;
        player.RestoreHealth(itemSO.HealthRestored, itemSO.HealthIterations, itemSO.HealthTotalTime);
    }
}

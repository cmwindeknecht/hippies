using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Consumable/RestoreEnergy")]
public class RestoreEnergyEffect : ConsumableEffect
{
    public override void Use(Player player, InventoryItem item)
    {
        if (item.ItemSO is not ItemConsumableSO)
        {
            throw new System.Exception("RestoreEnergyEffect passed a non ItemConsumableSO!");
        }

        ItemConsumableSO itemSO = (ItemConsumableSO)item.ItemSO;
        player.RestoreEnergy(itemSO.EnergyRestored, itemSO.EnergyIterations, itemSO.EnergyTotalTime);
    }
}

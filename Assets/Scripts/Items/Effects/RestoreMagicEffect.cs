using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Consumable/RestoreMagic")]
public class RestoreMagicEffect : ConsumableEffect
{
    public override void Use(Player player, InventoryItem item)
    {
        if (item.ItemSO is not ItemConsumableSO)
        {
            throw new System.Exception("RestoreMagicEffect passed a non ItemConsumableSO!");
        }

        ItemConsumableSO itemSO = (ItemConsumableSO)item.ItemSO;
        player.RestoreMagic(itemSO.MagicRestored, itemSO.MagicIterations, itemSO.MagicTotalTime);
    }
}

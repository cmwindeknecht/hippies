using System;
using System.Collections.Generic;

public class PlayerInventory : CharacterInventory
{
    public override void EquipWeapon(WeaponSO weapon)
    {
        if (InventoryItems.TryGetValue(InventoryItemType.Weapons, out List<InventoryItem> items)) {
            bool found = false;
            foreach (InventoryItem item in items)
            {
                // Check if the weapon that is trying to be equipped is actually in the inventory
            }
            if (!found)
            {
                // TODO proper exceptions so I can handle shit in the UI right --- though this one is a "Oh fuck" exception, not a "You don't have enough strength" or whatever exception
                throw new System.Exception($"Weapon {weapon.Name}");
            }
        } 
        else
        {
            throw new System.Exception("No InventoryItemType.Weapon list found in Inventory!");
        }

        // TODO stat checks and the like
        _EquippedWeaponSO = weapon;
    }

    public void AddToInventory(ItemSO itemSO)
    {
        if (InventoryItems.TryGetValue(itemSO.Type, out List<InventoryItem> inventoryItems))
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                if (inventoryItem.ItemSO.Name.Equals(itemSO.Name))
                {
                    inventoryItem.IncreaseQuantity();
                    return;
                }
            }
            inventoryItems.Add(new InventoryItem(itemSO, RemoveFromInventory));
        }
        else
        {
            List<InventoryItem> newInventoryItems = new()
            {
                new InventoryItem(itemSO, RemoveFromInventory)
            };
            InventoryItems.Add(itemSO.Type, newInventoryItems);
        }
    }

    public void RemoveFromInventory(ItemSO itemSO)
    {
        if (InventoryItems.TryGetValue(itemSO.Type, out List<InventoryItem> inventoryItems))
        {
            int? indexFound = null;
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].ItemSO.Name.Equals(itemSO.Name))
                {
                    indexFound = i; 
                    break;
                }
            }

            if (indexFound == null)
            {
                throw new Exception($"Somehow trying to remove item that is not in inventory! item=[{itemSO.Name}]");
            }

            inventoryItems.RemoveAt(indexFound.Value);
        }
        else
        {
            throw new Exception($"Somehow trying to remove item that is not in inventory! item=[{itemSO.Name}] type=[{itemSO.Type}]");
        }
    }
}

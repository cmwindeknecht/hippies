using System;
using System.Collections.Generic;

public class PlayerInventory : CharacterInventory
{
    public override void EquipWeapon(WeaponSO weapon, int slot = 1)
    {
        // TODO probably don't do this, seems like overkill, just make it a CharacterInventory function the same as Enemy
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

        if (slot == 1)
        {
            _EquippedWeaponOneSO = weapon;
        }
        else if (slot == 2)
        {
            _EquippedWeaponTwoSO = weapon;
        }
        else
        {
            throw new Exception("Unknown Weapon Slot!");
        }
    }

    
}

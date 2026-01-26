using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // TODO UI / Logic to have a 1-0 means to equip shit
    //      If an attack is in the 1-0 --- changes the attack to that
    //      If an item / healing spell / etc --- automatically uses it
    //      Shit like keys aren't necessary to use, interacting with shit should automatically know if you have the key
    // TODO remove serializefield, just doign this for testing
    [SerializeField] private WeaponSO _EquippedWeaponSO;
    public WeaponSO EquippedWeaponSO => _EquippedWeaponSO;
    // TODO Armor stuff
    public Dictionary<InventoryItemType, List<InventoryItem>> Inventory;

    public void EquipWeapon(WeaponSO weapon)
    {
        if (Inventory.TryGetValue(InventoryItemType.Weapon, out List<InventoryItem> items)) {
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
}

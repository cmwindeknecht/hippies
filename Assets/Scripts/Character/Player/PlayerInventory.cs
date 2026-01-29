using System;
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
    [SerializeField] private ShieldSO _EquippedShieldSO;
    public ShieldSO EquippedShieldSO => _EquippedShieldSO;
    // TODO Armor stuff

    public Dictionary<InventoryItemType, List<InventoryItem>> Inventory;

    private void Awake()
    {
        Inventory = new();
    }

    public void EquipWeapon(WeaponSO weapon)
    {
        if (Inventory.TryGetValue(InventoryItemType.Weapons, out List<InventoryItem> items)) {
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
        if (Inventory.TryGetValue(itemSO.Type, out List<InventoryItem> inventoryItems))
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                if (inventoryItem.ItemSO.Name.Equals(itemSO.Name))
                {
                    inventoryItem.IncreaseQuantity();
                    return;
                }
            }
            inventoryItems.Add(new InventoryItem(itemSO));
        }
        else
        {
            List<InventoryItem> newInventoryItems = new();
            newInventoryItems.Add(new InventoryItem(itemSO));
            Inventory.Add(itemSO.Type, newInventoryItems);
        }
    }
}

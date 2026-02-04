using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public abstract class CharacterInventory : MonoBehaviour
{
    // TODO UI / Logic to have a 1-0 means to equip shit
    //      If an attack is in the 1-0 --- changes the attack to that
    //      If an item / healing spell / etc --- automatically uses it
    //      Shit like keys aren't necessary to use, interacting with shit should automatically know if you have the key
    // TODO remove serializefield, just doign this for testing
    [SerializeField] protected WeaponSO _EquippedWeaponOneSO;
    public WeaponSO EquippedWeaponOneSO => _EquippedWeaponOneSO;

    [SerializeField] protected WeaponSO _EquippedWeaponTwoSO;
    public WeaponSO EquippedWeaponTwoSO => _EquippedWeaponTwoSO;

    public WeaponSO CurrentWeapon = null;

    [SerializeField] protected ArmorSO _EquippedShieldSO;
    public ArmorSO EquippedShieldSO => _EquippedShieldSO;

    [SerializeField] protected ArmorSO _EquippedHeadSO;
    public ArmorSO EquippedHeadSO => _EquippedHeadSO;

    [SerializeField] protected ArmorSO _EquippedShoulderSO;
    public ArmorSO EquippedShoulderSO => _EquippedShoulderSO;

    [SerializeField] protected ArmorSO _EquippedTorsoSO;
    public ArmorSO EquippedTorsoSO => _EquippedTorsoSO;

    [SerializeField] protected ArmorSO _EquippedHandsSO;
    public ArmorSO EquippedHandsSO => _EquippedHandsSO;

    [SerializeField] protected ArmorSO _EquippedLegsSO;
    public ArmorSO EquippedLegsSO => _EquippedLegsSO;

    [SerializeField] protected ArmorSO _EquippedFeetSO;
    public ArmorSO EquippedFeetSO => _EquippedFeetSO;

    [SerializeField] private List<ItemSO> _StartingInventory; // Mostly for testing purposes - maybe has a use for enemies or loading data?
    public Dictionary<InventoryItemType, List<InventoryItem>> InventoryItems;
    public int ShieldResistance => _EquippedShieldSO != null ? _EquippedShieldSO.PierceResistance : 0;
    public int ArmorRating => GetArmorRating();

    private void Awake()
    {
        InventoryItems = new();
        if (_EquippedHeadSO != null) AddToInventory(_EquippedHeadSO);
        if (_EquippedShoulderSO != null) AddToInventory(_EquippedShoulderSO);
        if (_EquippedHandsSO != null) AddToInventory(EquippedHandsSO);
        if (_EquippedTorsoSO != null) AddToInventory(_EquippedTorsoSO);
        if (EquippedLegsSO != null) AddToInventory(EquippedLegsSO);
        if (_EquippedFeetSO != null) AddToInventory(EquippedFeetSO);
        if (_EquippedShieldSO != null) AddToInventory(EquippedShieldSO);
        if (_EquippedWeaponOneSO != null) AddToInventory(_EquippedWeaponOneSO);
        if (_EquippedWeaponTwoSO != null) AddToInventory(EquippedWeaponTwoSO);

        foreach (ItemSO itemSO in _StartingInventory) AddToInventory(itemSO);
    }

    public void SetEquippedWeapon(int slot)
    {
        if (slot == 1 && _EquippedWeaponOneSO != null)
        {
            CurrentWeapon = _EquippedWeaponOneSO;
        }
        else if (slot == 2  && _EquippedWeaponTwoSO != null)
        {
            CurrentWeapon = _EquippedWeaponTwoSO;
        }
    }

    private int GetArmorRating()
    {
        int armorRating = 0;
        if (_EquippedHeadSO != null)
        {
            armorRating += _EquippedHeadSO.PierceResistance;
        }
        if (_EquippedShoulderSO != null)
        {
            armorRating += _EquippedShoulderSO.PierceResistance;
        }
        if (_EquippedTorsoSO != null)
        {
            armorRating += _EquippedTorsoSO.PierceResistance;
        }
        if (_EquippedHandsSO != null)
        {
            armorRating += _EquippedHandsSO.PierceResistance;
        }
        if (_EquippedLegsSO != null)
        {
            armorRating += _EquippedLegsSO.PierceResistance;
        }
        if (_EquippedFeetSO != null)
        {
            armorRating += _EquippedFeetSO.PierceResistance;
        }
        return armorRating;
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

    public ItemSO? TryGetItemFromInventory(InventoryItemType itemType, ArmorSlot? armorSlot, int? weaponSlot)
    {
        if (armorSlot == null && weaponSlot == null) throw new Exception("Both armor slot and weapon slot cannot be null!");
        if (armorSlot != null && weaponSlot != null) throw new Exception("Both armor slot and weapon slot cannot have values!");

        if (itemType.Equals(InventoryItemType.Armor))
        {
            if (armorSlot == null) throw new Exception("Item type of armor provided but no armor slot was provided!");

            if (armorSlot.Equals(ArmorSlot.Head))
            {
                return _EquippedHeadSO;
            }
            else if (armorSlot.Equals(ArmorSlot.Shoulder))
            {
                return _EquippedShoulderSO;
            }
            else if (armorSlot.Equals(ArmorSlot.Hands))
            {
                return _EquippedHandsSO;
            }
            else if (armorSlot.Equals(ArmorSlot.Torso))
            {
                return _EquippedTorsoSO;
            }
            else if (armorSlot.Equals(ArmorSlot.Legs))
            {
                return _EquippedLegsSO;
            }
            else if (armorSlot.Equals(ArmorSlot.Feet))
            {
                return _EquippedFeetSO;
            }
            else
            {
                return _EquippedShieldSO;
            }
        }
        else if (itemType.Equals(InventoryItemType.Weapons))
        {
            if (weaponSlot == null) throw new Exception("Item type of weapons provided but no weapon slot was provided!");

            if (weaponSlot == 1)
            {
                return _EquippedWeaponOneSO;
            }
            else if (weaponSlot == 2) 
            { 
                return _EquippedWeaponTwoSO; 
            }
            else
            {
                throw new Exception($"Invalid weapon slot provided = {weaponSlot}");
            }
        }
        else
        {
            throw new Exception($"Invalid inventory item type to equip = {itemType}");
        }
    }

    public void EquipArmor(ArmorSO armorSO)
    {
        if (armorSO.ArmorSlot.Equals(ArmorSlot.Head)) _EquippedHeadSO = armorSO;
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Shoulder)) _EquippedShoulderSO = armorSO;
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Hands)) _EquippedHandsSO = armorSO;
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Torso)) _EquippedTorsoSO = armorSO;
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Legs)) _EquippedLegsSO = armorSO;
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Feet)) _EquippedFeetSO = armorSO;
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Shield)) _EquippedShieldSO = armorSO;
        else throw new Exception($"Unknown Armor Slot {armorSO.ArmorSlot}");
    }

    public void EquipWeapon(WeaponSO weaponSO, int slot = 1)
    {
        if (slot == 1) _EquippedWeaponOneSO = weaponSO;
        else if (slot == 2) _EquippedWeaponTwoSO = weaponSO;
        else throw new Exception("Unknown Weapon Slot!");
    }
}

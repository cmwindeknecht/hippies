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

    // TODO probably should cache this and then reset it on equip of new piece of armor, but its fine for now
    public int GetArmorResistance(DamageType? attackDamageType, List<ElementalDamage> attackElementalDamages)
    {
        int armorRating = 0;

        armorRating += GetResistance(_EquippedHeadSO, attackDamageType);
        armorRating += GetResistance(_EquippedHeadSO, attackElementalDamages);

        armorRating += GetResistance(_EquippedShoulderSO, attackDamageType);
        armorRating += GetResistance(_EquippedShoulderSO, attackElementalDamages);

        armorRating += GetResistance(_EquippedHandsSO, attackDamageType);
        armorRating += GetResistance(_EquippedHandsSO, attackElementalDamages);

        armorRating += GetResistance(_EquippedTorsoSO, attackDamageType);
        armorRating += GetResistance(_EquippedTorsoSO, attackElementalDamages);

        armorRating += GetResistance(_EquippedLegsSO, attackDamageType);
        armorRating += GetResistance(_EquippedLegsSO, attackElementalDamages);

        armorRating += GetResistance(_EquippedFeetSO, attackDamageType);
        armorRating += GetResistance(_EquippedFeetSO, attackElementalDamages);

        return armorRating;
    }

    public int GetShieldResistance(DamageType? attackDamageType, List<ElementalDamage> attackElementalDamages)
    {
        int resistance = GetResistance(_EquippedShieldSO, attackDamageType);
        resistance += GetResistance(_EquippedShieldSO, attackElementalDamages);
        return resistance;
    }

    private int GetResistance(ArmorSO? armorSO, DamageType? damageType)
    {
        if (armorSO == null) return 0;
        if (damageType == null) return 0;

        return damageType switch
        {
            DamageType.Pierce => armorSO.PierceResistance,
            DamageType.Blunt => armorSO.BluntResistance,
            DamageType.Explosive => armorSO.ExplosiveResistance,
            _ => throw new NotImplementedException()
        };
    }

    private int GetResistance(ArmorSO? armorSO, List<ElementalDamage> elementalDamageTypes)
    {
        if (armorSO == null) return 0;

        int elementalResistance = 0;
        foreach(ElementalDamage elementalDamageType in elementalDamageTypes)
        {
            elementalResistance += GetResistance(armorSO, elementalDamageType);
        }

        return elementalResistance;
    }

    private int GetResistance(ArmorSO? armorSO, ElementalDamage elementalDamage)
    {
        if (armorSO == null) return 0;

        return elementalDamage.Type switch
        {
            ElementalDamageType.Ice => armorSO.IceResistance,
            ElementalDamageType.Fire => armorSO.FireResistance,
            ElementalDamageType.Lightning => armorSO.LightningResistance,
            ElementalDamageType.Earth => armorSO.EarthResistance,
            ElementalDamageType.Void => EquippedShieldSO.VoidResistance,
            _ => throw new NotImplementedException()
        };
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

    public (ArmorSO previouslyEquipped, ArmorSO currentlyEquipped) EquipArmor(ArmorSO armorSO)
    {
        ArmorSO previouslyEquipped;
        if (armorSO.ArmorSlot.Equals(ArmorSlot.Head))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedHeadSO = armorSO;
        }
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Shoulder))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedShoulderSO = armorSO;
        }
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Hands))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedHandsSO = armorSO;
        }
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Torso))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedTorsoSO = armorSO;
        }
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Legs))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedLegsSO = armorSO;
        }
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Feet))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedFeetSO = armorSO;
        }
        else if (armorSO.ArmorSlot.Equals(ArmorSlot.Shield))
        {
            previouslyEquipped = _EquippedHeadSO;
            _EquippedShieldSO = armorSO;
        }
        else throw new Exception($"Unknown Armor Slot {armorSO.ArmorSlot}");

        return (previouslyEquipped, armorSO);
    }

    public (WeaponSO previouslyEquipped, WeaponSO currentlyEquipped) EquipWeapon(WeaponSO weaponSO, int slot = 1)
    {
        WeaponSO previouslyEquipped;
        if (slot == 1)
        {
            previouslyEquipped = _EquippedWeaponOneSO;
            _EquippedWeaponOneSO = weaponSO;
        }
        else if (slot == 2) {
            previouslyEquipped = _EquippedWeaponTwoSO;
            _EquippedWeaponTwoSO = weaponSO;
        }
        else throw new Exception("Unknown Weapon Slot!");
        return (previouslyEquipped, weaponSO);
    }
}

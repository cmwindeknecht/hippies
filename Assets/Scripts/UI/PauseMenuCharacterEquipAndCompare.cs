using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO 
// 1. Stats are all wrong for shit.  No info like weight, agi/str/etc requirement, description, etc.  Clean that up later.
// 2. Need to have a new slot pop up for ranged shit to change the ranged projectile type
// 3. 2H weapons don't take up both slots when equipped
public class PauseMenuEquipAndCompare : MonoBehaviour
{
    [SerializeField] private PauseMenuCharacterEquipAndCompareEquipped _CompareEquipped;
    [SerializeField] private PauseMenuCharacterEquipAndCompareCompare _CompareCompare;
    [SerializeField] private RectTransform _ToCompareContent;

    private ItemSO _EquippedItemSO;
    private ItemSO _CompareItemSO;

    [SerializeField] private PauseMenuCharacterComparableWeapon _ComparablePrefabWeapon;
    [SerializeField] private PauseMenuCharacterComparableArmor _ComparablePrefabArmor;

    private PlayerInventory _Inventory;

    private void OnEnable()
    {
        _CompareEquipped.gameObject.SetActive(false);
        _CompareCompare.gameObject.SetActive(false);
    }

    public void Setup(PlayerInventory inventory)
    {
        _Inventory = inventory;
    }

    public void UpdateEquipped(ItemSO equippedItem)
    {
        _EquippedItemSO = equippedItem;
        _CompareEquipped.gameObject.SetActive(true);

        if (_EquippedItemSO is ArmorSO equippedArmorSO)
        {
            _CompareEquipped.Setup(equippedArmorSO);
        }
        else if (_EquippedItemSO is WeaponSO equippedWeaponSO)
        {
            _CompareEquipped.Setup(equippedWeaponSO);
        }
        else
        {
            throw new System.Exception($"{equippedItem.Type} type is ineligible for EquipAndCompare!");
        }

        _CompareCompare.gameObject.SetActive(false);
        RefreshCompare();
    }

    public void UpdateCompare(ItemSO compareItem)
    {
        _CompareItemSO = compareItem;
        _CompareCompare.gameObject.SetActive(true);

        if (_EquippedItemSO is ArmorSO equippedArmorSO && _CompareItemSO is ArmorSO compareArmorSO)
        {
            _CompareCompare.Setup(compareArmorSO, equippedArmorSO);
        }
        if (_EquippedItemSO is WeaponSO equippedWeaponSO && _CompareItemSO is WeaponSO compareWeaponSO)
        {
            _CompareCompare.Setup(compareWeaponSO, equippedWeaponSO);
        }
        else
        {
            throw new System.Exception($"Equipped {_EquippedItemSO.Type} and Compare {_CompareItemSO.Type} type are ineligible for EquipAndCompare!");
        }

    }

    public void RefreshCompare()
    {
        if (_Inventory == null) return;
        if (_EquippedItemSO == null) return;

        // Clear existing items
        foreach (Transform child in _ToCompareContent)
        {
            Destroy(child.gameObject);
        }

        Dictionary<InventoryItemType, List<InventoryItem>> inventory = _Inventory.InventoryItems;
        foreach (KeyValuePair<InventoryItemType, List<InventoryItem>> kvp in inventory)
        {
            if (!kvp.Key.Equals(_EquippedItemSO.Type)) continue;

            List<InventoryItem> sortedItems = kvp.Value.OrderBy(p => p.ItemSO.Name).ToList();

            foreach (InventoryItem item in sortedItems)
            {
                switch (kvp.Key)
                {
                    case InventoryItemType.Weapons:
                        PauseMenuCharacterComparableWeapon comparabaleWeapon = Instantiate(_ComparablePrefabWeapon, _ToCompareContent);
                        comparabaleWeapon.Setup((WeaponSO)item.ItemSO);
                        break;
                    case InventoryItemType.Armor:
                        if (!((ArmorSO)item.ItemSO).ArmorSlot.Equals(((ArmorSO)_EquippedItemSO).ArmorSlot)) continue;
                        PauseMenuCharacterComparableArmor comparabaleArmor = Instantiate(_ComparablePrefabArmor, _ToCompareContent);
                        comparabaleArmor.Setup((ArmorSO)item.ItemSO);
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
            }
        }
    }
}

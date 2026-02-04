using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseMenuEquipAndCompare : MonoBehaviour
{
    [SerializeField] private PauseMenuCharacterEquipAndCompareEquipped _CompareEquipped;
    [SerializeField] private PauseMenuCharacterEquipAndCompareCompare _CompareCompare;
    [SerializeField] private RectTransform _ToCompareContent;

    private ItemSO _EquippedItemSO;
    private ItemSO _CompareItemSO;

    private PauseMenuCharacterComparableWeapon _ComparablePrefabWeapon;
    private PauseMenuCharacterComparableArmor _ComparablePrefabArmor;

    private PlayerInventory _Inventory;

    public void Setup(PlayerInventory inventory)
    {
        _Inventory = inventory;
    }

    // TODO call after event
    public void UpdateEquipped(ItemSO equippedItem)
    {
        _EquippedItemSO = equippedItem;
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
                        // TODO setup 
                        break;
                    case InventoryItemType.Armor:
                        PauseMenuCharacterComparableArmor comparabaleArmor = Instantiate(_ComparablePrefabArmor, _ToCompareContent);
                        // TODO setup 
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
            }
        }
    }
}

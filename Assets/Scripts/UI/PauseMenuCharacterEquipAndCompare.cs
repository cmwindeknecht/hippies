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
        RefreshCompare();

        PlayerInventory.OnEquipmentChange += PlayerInventory_OnEquipmentChange;
        PauseMenuCharacterComparableAbstract.OnCompareClicked += PauseMenuCharacterComparableAbstract_OnCompareClicked;
    }

    private void OnDisable()
    {
        PlayerInventory.OnEquipmentChange -= PlayerInventory_OnEquipmentChange;
        PauseMenuCharacterComparableAbstract.OnCompareClicked -= PauseMenuCharacterComparableAbstract_OnCompareClicked;
    }

    private void PauseMenuCharacterComparableAbstract_OnCompareClicked(object sender, ItemSO e)
    {
        UpdateCompare(e);
    }

    private void PlayerInventory_OnEquipmentChange(object sender, PlayerInventory.OnEquipmentChangeArgs e)
    {
        UpdateEquipped(e.currentlyEquipped);
        // TODO should probably just be listening in inventory
        if (e.currentlyEquipped.Type.Equals(typeof(ArmorSO)))
        {
            _Inventory.EquipArmor((ArmorSO)e.currentlyEquipped);
        }
        else
        {
            _Inventory.EquipWeapon((WeaponSO)e.currentlyEquipped);
        }
        UpdateCompare(e.previouslyEquipped);
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
        else if (_EquippedItemSO is WeaponSO equippedWeaponSO && _CompareItemSO is WeaponSO compareWeaponSO)
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
        foreach (Transform child in _ToCompareContent)
        {
            Destroy(child.gameObject);
        }

        if (_Inventory == null) return;
        if (_EquippedItemSO == null) return;

        Dictionary<InventoryItemType, List<InventoryItem>> inventory = _Inventory.InventoryItems;
        foreach (KeyValuePair<InventoryItemType, List<InventoryItem>> kvp in inventory)
        {
            if (!kvp.Key.Equals(_EquippedItemSO.Type)) continue;

            List<InventoryItem> sortedItems = kvp.Value.OrderBy(p => p.ItemSO.Name).ToList();

            bool armorFound = false, weaponOneFound = false, weaponTwoFound = false;
            foreach (InventoryItem item in sortedItems)
            {
                if (!armorFound && _EquippedItemSO is ArmorSO)
                {
                    if (item.ItemSO.GetInstanceID().Equals(_EquippedItemSO.GetInstanceID()))
                    {
                        armorFound = true;
                        continue;
                    }
                }
                if ((!weaponOneFound || !weaponTwoFound) && _EquippedItemSO is WeaponSO)
                {
                    if (_Inventory.EquippedWeaponOneSO == null)
                    {
                        weaponOneFound = true;
                    }
                    if (_Inventory.EquippedWeaponTwoSO == null)
                    {
                        weaponTwoFound = true;
                    }
                    
                    if (!weaponOneFound && item.ItemSO.GetInstanceID().Equals(_Inventory.EquippedWeaponOneSO.GetInstanceID())) {
                        weaponOneFound = true;
                        continue;
                    }
                    else if (!weaponTwoFound && item.ItemSO.GetInstanceID().Equals(_Inventory.EquippedWeaponTwoSO.GetInstanceID()))
                    {
                        weaponTwoFound = true;
                        continue;
                    }
                }

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

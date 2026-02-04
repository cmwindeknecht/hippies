using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseMenuEquipAndCompare : MonoBehaviour
{
    [SerializeField] private GameObject _EquippedItem;
    [SerializeField] private GameObject _CompareItem;
    [SerializeField] private RectTransform _ToCompareContent;

    private ItemSO _EquippedItemSO;
    private ItemSO _CompareItemSO;
    // TODO probably need PauseMenuCharacterEquippedArmor/Weapon and PauseMenuCharacterComparableArmor/Weapon
    private PauseMenuCharacterEquipped _EquipAndComparePrefabWeapon;
    private PauseMenuCharacterEquipped _EquipAndComparePrefabArmor;
    private PauseMenuCharacterComparable _ComparePrefabWeapon;
    private PauseMenuCharacterComparable _ComparePrefabArmor;

    private PlayerInventory _Inventory;

    public void Setup(PlayerInventory inventory)
    {
        _Inventory = inventory;
    }

    // TODO call after event
    public void UpdateEquipped(ItemSO equippedItem)
    {
        _EquippedItemSO = equippedItem;
        // TODO update equipped prefab
    }

    public void UpdateCompare(ItemSO compareItem)
    {
        _CompareItemSO = compareItem;
        // TODO update Compare prefab
        UpdateCompareStats();
    }

    private void UpdateCompareStats()
    {
        // TODO show shit like +10 damage or whatever
    }

    public void RefreshCompare()
    {
        if (_Inventory == null) return;
        if (_EquippedItem == null) return;

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
                        PauseMenuCharacterComparable comparabaleWeapon = Instantiate(_ComparePrefabWeapon, _ToCompareContent);
                        // TODO setup 
                        break;
                    case InventoryItemType.Armor:
                        PauseMenuCharacterComparable comparabaleArmor = Instantiate(_ComparePrefabArmor, _ToCompareContent);
                        // TODO setup 
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
            }
        }
    }
}

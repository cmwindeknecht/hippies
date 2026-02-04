using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class PauseMenuCharacterEquipped : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler<ItemSO> OnEquippedItemClicked;
    [SerializeField] private Image _EquipmentIconCenterOrLeft;
    [SerializeField] private Image _EquipmentIconRight;
    private PlayerInventory _Inventory;
    private InventoryItemType _IventoryItemType;
    private ArmorSlot? _ArmorSlot;
    private int? _WeaponSlot;
    private ItemSO? _ItemSO;

    public void Setup(PlayerInventory playerInventory, InventoryItemType itemType, ArmorSlot armorSlot)
    {
        _Inventory = playerInventory;
        _IventoryItemType = itemType;
        _ArmorSlot = armorSlot;
    }

    public void Setup(PlayerInventory playerInventory, InventoryItemType itemType, int weaponSlot)
    {
        _Inventory = playerInventory;
        _IventoryItemType = itemType;
        _WeaponSlot = weaponSlot;
    }

    private void OnEnable()
    {
        if (_Inventory == null) return;

        ItemSO? itemSO = _Inventory.TryGetItemFromInventory(_IventoryItemType, _ArmorSlot, _WeaponSlot);
        if (itemSO != null)
        {
            _ItemSO = itemSO;
            if (_EquipmentIconCenterOrLeft != null)
            {
                _EquipmentIconCenterOrLeft.sprite = itemSO.Sprite;
            }
            if (_EquipmentIconRight != null)
            {
                _EquipmentIconRight.sprite = itemSO.Sprite;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_ItemSO != null)
        {
            Debug.Log($"Emitting event for ItemSO {_ItemSO.Name}");
            OnEquippedItemClicked?.Invoke(this, _ItemSO);   
        }
    }
}

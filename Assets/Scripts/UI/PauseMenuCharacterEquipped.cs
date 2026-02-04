using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class PauseMenuCharacterEquipped : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler<ItemSO> OnEquippedItemClicked;
    [SerializeField] private Image _EquipmentIconCenterOrLeft;
    [SerializeField] private Image EquipmentIconRight;
    private PlayerInventory _Inventory;
    private InventoryItemType _IventoryItemType;
    private ArmorSlot? _ArmorSlot;
    private int? _WeaponSlot;

    // TODO set this shit up in the PauseMenuCharacter screen
    public void Setup(PlayerInventory playerInventory, InventoryItemType itemType, ArmorSlot? armorSlot, int? weaponSlot)
    {
        _Inventory = playerInventory;
        _IventoryItemType = itemType;
        _ArmorSlot = armorSlot;
        _WeaponSlot = weaponSlot;
    }

    private void OnEnable()
    {
        ItemSO? itemSO = _Inventory.TryGetItemFromInventory(_IventoryItemType, _ArmorSlot, _WeaponSlot);
        if (itemSO != null)
        {
            if (_EquipmentIconCenterOrLeft != null)
            {
                _EquipmentIconCenterOrLeft.sprite = itemSO.Sprite;
            }
            if (EquipmentIconRight != null)
            {
                _EquipmentIconCenterOrLeft.sprite = itemSO.Sprite;
            }

            OnEquippedItemClicked?.Invoke(this, itemSO); // TODO listen to these events to populate the details section
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}

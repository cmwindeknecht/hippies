using UnityEngine;

public enum InventoryItemType
{
    Weapon,
    Armor,
    Consumable
}

public class InventoryItem
{
    private InventoryItemType _Type;
    public InventoryItemType Type => _Type;

    private float _Quantity;
    public float Quantity => _Quantity;

    // TODO private InventoryItemSO _InventoryItemSO --- basically every item that exists in the game should have an equivalent InventoryItemSO (I think?  Doesn't make sense to have shit like sale price on the WeaponSO)
}

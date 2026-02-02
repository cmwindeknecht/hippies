using System;

public enum InventoryItemType
{
    None, // No item should ever be assigned this, used in inventory UI
    Weapons,
    Armor,
    Consumable,
    Others
}

public class InventoryItem
{
    private ItemSO _ItemSO;
    public ItemSO ItemSO => _ItemSO;

    private Action<ItemSO> _RemoveFromInventory;

    private float _Quantity;
    public float Quantity => _Quantity;
    public void IncreaseQuantity() => _Quantity++;
    public void DecreaseQuantity() {
        _Quantity--;
        if (_Quantity < 0 )
        {
            _RemoveFromInventory(_ItemSO);
        }
    }

    public InventoryItem(ItemSO itemSO, Action<ItemSO> removeFromInventory)
    {
        _ItemSO = itemSO;
        _Quantity = 1;
        _RemoveFromInventory = removeFromInventory;
    }
}

using UnityEngine;

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

    private float _Quantity;
    public float Quantity => _Quantity;
    public void IncreaseQuantity() => _Quantity++;
    public void DecreaseQuantity() => _Quantity--;

    public InventoryItem(ItemSO itemSO)
    {
        _ItemSO = itemSO;
        _Quantity = 1;
    }
}

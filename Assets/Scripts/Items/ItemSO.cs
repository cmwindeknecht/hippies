using UnityEngine;

// TODO should obviously have different drop types and this should be abstract, but just doing this for now for enemy drop / pickup mechanics.
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Item")]
public class ItemSO : ScriptableObject
{
    public InventoryItemType Type => InventoryItemType.Food;
    public string Name;
    public int Health;
    public int Magic;
}

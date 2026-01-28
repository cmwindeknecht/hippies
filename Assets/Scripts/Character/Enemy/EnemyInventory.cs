using System.Collections.Generic;
using UnityEngine;

public class EnemyInventory : MonoBehaviour
{
    // TODO UI / Logic to have a 1-0 means to equip shit
    //      If an attack is in the 1-0 --- changes the attack to that
    //      If an item / healing spell / etc --- automatically uses it
    //      Shit like keys aren't necessary to use, interacting with shit should automatically know if you have the key
    // TODO remove serializefield, just doign this for testing
    [SerializeField] private WeaponSO _EquippedWeaponSO;
    public WeaponSO EquippedWeaponSO => _EquippedWeaponSO;
    // TODO Armor stuff
    public Dictionary<InventoryItemType, List<InventoryItem>> Inventory;

    public void EquipWeapon(WeaponSO weapon)
    {
        _EquippedWeaponSO = weapon;
    }
}

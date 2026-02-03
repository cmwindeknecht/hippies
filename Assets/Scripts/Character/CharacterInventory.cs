using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CharacterInventory : MonoBehaviour
{
    // TODO UI / Logic to have a 1-0 means to equip shit
    //      If an attack is in the 1-0 --- changes the attack to that
    //      If an item / healing spell / etc --- automatically uses it
    //      Shit like keys aren't necessary to use, interacting with shit should automatically know if you have the key
    // TODO remove serializefield, just doign this for testing
    [SerializeField] protected WeaponSO _EquippedWeaponSO;
    public WeaponSO EquippedWeaponSO => _EquippedWeaponSO;

    [SerializeField] protected ArmorSO _EquippedShieldSO;
    public ArmorSO EquippedShieldSO => _EquippedShieldSO;

    [SerializeField] protected ArmorSO _EquippedHeadSO;
    public ArmorSO EquippedHeadSO => _EquippedHeadSO;

    [SerializeField] protected ArmorSO _EquippedShoulderSO;
    public ArmorSO EquippedShoulderSO => _EquippedShoulderSO;

    [SerializeField] protected ArmorSO _EquippedTorsoSO;
    public ArmorSO EquippedTorsoSO => _EquippedTorsoSO;

    [SerializeField] protected ArmorSO _EquippedHandsSO;
    public ArmorSO EquippedHandsSO => _EquippedHandsSO;

    [SerializeField] protected ArmorSO _EquippedLegsSO;
    public ArmorSO EquippedLegsSO => _EquippedLegsSO;

    [SerializeField] protected ArmorSO _EquippedFeetSO;
    public ArmorSO EquippedFeetSO => _EquippedFeetSO;

    public Dictionary<InventoryItemType, List<InventoryItem>> InventoryItems;
    public int ShieldResistance => _EquippedShieldSO != null ? _EquippedShieldSO.DamageResistance : 0;
    public int ArmorRating => GetArmorRating();
    public int WeaponDamage => Utilities.GetRandomInt(_EquippedWeaponSO.DamageMin, _EquippedWeaponSO.DamageMax);

    private int GetArmorRating()
    {
        int armorRating = 0;
        if (_EquippedHeadSO != null)
        {
            armorRating += _EquippedHeadSO.DamageResistance;
        }
        if (_EquippedShoulderSO != null)
        {
            armorRating += _EquippedShoulderSO.DamageResistance;
        }
        if (_EquippedTorsoSO != null)
        {
            armorRating += _EquippedTorsoSO.DamageResistance;
        }
        if (_EquippedHandsSO != null)
        {
            armorRating += _EquippedHandsSO.DamageResistance;
        }
        if (_EquippedLegsSO != null)
        {
            armorRating += _EquippedLegsSO.DamageResistance;
        }
        if (_EquippedFeetSO != null)
        {
            armorRating += _EquippedFeetSO.DamageResistance;
        }
        return armorRating;
    }

    private void Awake()
    {
        InventoryItems = new();
    }

    public abstract void EquipWeapon(WeaponSO weapon);
}

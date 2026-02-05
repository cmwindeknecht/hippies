using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCharacter : MonoBehaviour
{
    [Header("CharacterStats")]
    [SerializeField] private PauseMenuCharacterStatDisplay _Strength;
    [SerializeField] private PauseMenuCharacterStatDisplay _Agility;
    [SerializeField] private PauseMenuCharacterStatDisplay _Intelligence;
    [SerializeField] private PauseMenuCharacterStatDisplay _Vitality;
    [SerializeField] private PauseMenuCharacterStatDisplay _Stamina;
    [SerializeField] private PauseMenuCharacterStatDisplay _Luck;

    [Header("Derived Stats - Damage")]
    [SerializeField] private PauseMenuCharacterStatDisplay _DamageOne;
    [SerializeField] private PauseMenuCharacterStatDisplay _DamageTwo;

    [Header("Derived Stats - Defense")]
    [SerializeField] private PauseMenuCharacterStatDisplay _BluntDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _PiercingDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _ExplosiveDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _FireDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _IceDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _LightningDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _EarthDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _VoidDefense;

    [Header("Equipped Items")]
    [SerializeField] private PauseMenuCharacterEquipped _EquippedHead;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedShoulders;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedHands;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedTorso;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedLegs;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedFeet;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedWeaponOne;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedWeaponTwo;
    [SerializeField] private PauseMenuCharacterEquipped _EquippedShield;

    [Header("EquipmentCompare")]
    [SerializeField] private PauseMenuEquipAndCompare _EquipAndCompare;

    private Player _Player;

    public void Setup(Player player)
    {
        _Player = player;

        _Strength.Setup(player);
        _Agility.Setup(player);
        _Intelligence.Setup(player);
        _Vitality.Setup(player);
        _Stamina.Setup(player);
        _Luck.Setup(player);

        _EquipAndCompare.Setup(player.Inventory);

        _EquippedHead.Setup(player.Inventory, itemType:InventoryItemType.Armor, armorSlot:ArmorSlot.Head);
        _EquippedHead.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedShoulders.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Shoulder);
        _EquippedShoulders.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedHands.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Hands);
        _EquippedHands.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedTorso.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Torso);
        _EquippedTorso.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedLegs.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Legs);
        _EquippedLegs.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedFeet.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Feet);
        _EquippedFeet.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedWeaponOne.Setup(player.Inventory, itemType: InventoryItemType.Weapons, weaponSlot:1);
        _EquippedWeaponOne.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedWeaponTwo.Setup(player.Inventory, itemType: InventoryItemType.Weapons, weaponSlot: 2);
        _EquippedWeaponTwo.OnEquippedItemClicked += OnEquippedItemClicked;

        _EquippedShield.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Shield);
        _EquippedShield.OnEquippedItemClicked += OnEquippedItemClicked;
    }

    private void OnEquippedItemClicked(object sender, ItemSO e)
    {
        _EquipAndCompare.UpdateEquipped(e);
    }
}

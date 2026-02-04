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

    public void Setup(Player player)
    {
        _Strength.Setup(player);
        _Agility.Setup(player);
        _Intelligence.Setup(player);
        _Vitality.Setup(player);
        _Stamina.Setup(player);
        _Luck.Setup(player);

        _EquippedHead.Setup(player.Inventory, itemType:InventoryItemType.Armor, armorSlot:ArmorSlot.Head);
        _EquippedShoulders.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Shoulder);
        _EquippedHands.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Hands);
        _EquippedTorso.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Torso);
        _EquippedLegs.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Legs);
        _EquippedFeet.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Feet);
        _EquippedWeaponOne.Setup(player.Inventory, itemType: InventoryItemType.Weapons, weaponSlot:1);
        _EquippedWeaponTwo.Setup(player.Inventory, itemType: InventoryItemType.Weapons, weaponSlot: 2);
        _EquippedShield.Setup(player.Inventory, itemType: InventoryItemType.Armor, armorSlot: ArmorSlot.Shield);
    }
}

using System;
using System.Collections.Generic;

public class PlayerInventory : CharacterInventory
{
    private void Start()
    {
        PauseMenuCharacterComparableAbstract.OnEquipClicked += PauseMenuCharacterComparableAbstract_OnEquipClicked;
    }

    private void PauseMenuCharacterComparableAbstract_OnEquipClicked(object sender, ItemSO e)
    {
        if (e is ArmorSO armorSO) EquipArmor(armorSO);
        else if (e is WeaponSO weaponSO) EquipWeapon(weaponSO);
        else throw new Exception($"Unexpected type on equip clicked {e.Type}");
    }
}

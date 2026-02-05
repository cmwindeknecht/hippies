using System;
using System.Collections.Generic;

public class PlayerInventory : CharacterInventory
{
    public class OnEquipmentChangeArgs : EventArgs
    {
        public ItemSO previouslyEquipped;
        public ItemSO currentlyEquipped;
    }
    public static event EventHandler<OnEquipmentChangeArgs> OnEquipmentChange;

    private void Start()
    {
        PauseMenuCharacterComparableAbstract.OnEquipClicked += PauseMenuCharacterComparableAbstract_OnEquipClicked;
    }

    private void PauseMenuCharacterComparableAbstract_OnEquipClicked(object sender, ItemSO e)
    {
        ItemSO previouslyEquipped, currentlyEquipped;
        if (e is ArmorSO armorSO)
        {
            (previouslyEquipped, currentlyEquipped) = EquipArmor(armorSO);
        }
        else if (e is WeaponSO weaponSO)
        {
            (previouslyEquipped, currentlyEquipped) = EquipWeapon(weaponSO);
        }
        else throw new Exception($"Unexpected type on equip clicked {e.Type}");
        OnEquipmentChange?.Invoke(this, new OnEquipmentChangeArgs { previouslyEquipped = previouslyEquipped, currentlyEquipped = currentlyEquipped });
    }
}

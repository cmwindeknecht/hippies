using System;
using TMPro;
using UnityEngine;

public class PauseMenuCharacterComparableWeapon : PauseMenuCharacterComparableAbstract
{
    [SerializeField] private TextMeshProUGUI _HandsRequirement;

    public void Setup(WeaponSO weaponSO)
    {
        base.Setup(weaponSO);

        _HandsRequirement.text = weaponSO.HandsRequirement.ToString();

        switch (weaponSO.DamageType)
        {
            case DamageType.Blunt: SetupBlunt(weaponSO.DamageMin, weaponSO.DamageMax); break;
            case DamageType.Pierce: SetupPiercing(weaponSO.DamageMin, weaponSO.DamageMax); break;
            case DamageType.Explosive: SetupBlunt(weaponSO.DamageMin, weaponSO.DamageMax); break;
        }

        foreach (ElementalDamage elementalDamage in weaponSO.ElementalDamages)
        {
            switch (elementalDamage.Type)
            {
                case ElementalDamageType.None: break;
                case ElementalDamageType.Fire: SetupFire(elementalDamage.DamageMin, elementalDamage.DamageMax); break;
                case ElementalDamageType.Ice: SetupIce(elementalDamage.DamageMin, elementalDamage.DamageMax); break;
                case ElementalDamageType.Lightning: SetupLightning(elementalDamage.DamageMin, elementalDamage.DamageMax); break;
                case ElementalDamageType.Earth: SetupEarth(elementalDamage.DamageMin, elementalDamage.DamageMax); break;
                case ElementalDamageType.Void: SetupVoid(elementalDamage.DamageMin, elementalDamage.DamageMax); break;
            }
        }
    }
}

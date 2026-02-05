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

        switch (weaponSO.ElementalDamageType)
        {
            case ElementalDamageType.None: break;
            case ElementalDamageType.Fire: SetupFire(weaponSO.ElementalMin, weaponSO.ElementalMax); break;
            case ElementalDamageType.Ice: SetupFire(weaponSO.ElementalMin, weaponSO.ElementalMax); break;
            case ElementalDamageType.Lightning: SetupFire(weaponSO.ElementalMin, weaponSO.ElementalMax); break;
            case ElementalDamageType.Earth: SetupFire(weaponSO.ElementalMin, weaponSO.ElementalMax); break;
            case ElementalDamageType.Void: SetupFire(weaponSO.ElementalMin, weaponSO.ElementalMax); break;
        }
    }
}

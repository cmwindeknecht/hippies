using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class PauseMenuCharacterEquipAndCompareEquipped : MonoBehaviour
{
    [SerializeField] private Image _ItemIcon;
    [SerializeField] private TextMeshProUGUI _Name;

    [SerializeField] protected WeaponArmorStat _ItemWeightPrefab;
    [SerializeField] protected WeaponArmorStat _FirePrefab;
    [SerializeField] protected WeaponArmorStat _IcePrefab;
    [SerializeField] protected WeaponArmorStat _LightningPrefab;
    [SerializeField] protected WeaponArmorStat _EarthPrefab;
    [SerializeField] protected WeaponArmorStat _VoidPrefab;
    [SerializeField] protected WeaponArmorStat _PiercingPrefab;
    [SerializeField] protected WeaponArmorStat _BluntPrefab;
    [SerializeField] protected WeaponArmorStat _ExplosivePrefab;

    public void Setup(ArmorSO armorSO)
    {
        SetupPrefab(_ItemWeightPrefab, armorSO.Weight);

        SetupPrefab(_FirePrefab, armorSO.FireResistance);
        SetupPrefab(_IcePrefab, armorSO.IceResistance);
        SetupPrefab(_LightningPrefab, armorSO.LightningResistance);
        SetupPrefab(_EarthPrefab, armorSO.EarthResistance);
        SetupPrefab(_VoidPrefab, armorSO.VoidResistance);
        SetupPrefab(_PiercingPrefab, armorSO.PierceResistance);
        SetupPrefab(_BluntPrefab, armorSO.BluntResistance);
        SetupPrefab(_ExplosivePrefab, armorSO.ExplosiveResistance);
    }

    public void Setup(WeaponSO weaponSO)
    {
        SetupPrefab(_ItemWeightPrefab, weaponSO.Weight);

        WeaponArmorStat prefab = GetPrefabForWeaponDamageType(weaponSO.DamageType);
        SetupPrefab(prefab, weaponSO.DamageMin, weaponSO.DamageMax);

        WeaponArmorStat? elementalPrefab = GetPrefabForElementalDamageType(weaponSO.ElementalDamageType);
        if (elementalPrefab != null)
        {
            SetupPrefab(elementalPrefab, weaponSO.ElementalMin, weaponSO.ElementalMax);
        }
    }

    // Use for defense / weight
    private void SetupPrefab(WeaponArmorStat prefab, float current)
    {
        prefab.Setup(Color.white, $"{current} (=)");
    }

    // Used for damages
    private void SetupPrefab(WeaponArmorStat prefab, int currentMin, int currentMax)
    {
        prefab.Setup(Color.white, $"{currentMin} (=)", Color.white, $"{currentMax} (=)");
    }

    private WeaponArmorStat GetPrefabForWeaponDamageType(DamageType damageType)
    {
        return (damageType) switch
        {
            DamageType.Blunt => _BluntPrefab,
            DamageType.Pierce => _PiercingPrefab,
            DamageType.Explosive => _ExplosivePrefab,
            _ => throw new System.Exception($"Unknown DamageType {damageType}")
        };
    }

    private WeaponArmorStat? GetPrefabForElementalDamageType(ElementalDamageType elementalDamageType)
    {
        return elementalDamageType switch
        {
            ElementalDamageType.None => null,
            ElementalDamageType.Fire => _FirePrefab,
            ElementalDamageType.Ice => _IcePrefab,
            ElementalDamageType.Lightning => _LightningPrefab,
            ElementalDamageType.Earth => _EarthPrefab,
            ElementalDamageType.Void => _VoidPrefab,
            _ => throw new System.Exception($"Unknown ElementalDamageType {elementalDamageType}")
        };
    }
}

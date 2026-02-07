using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class PauseMenuCharacterEquipAndCompareCompare : MonoBehaviour
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

    public void Setup(ArmorSO armorSO, ArmorSO toCompare)
    {
        _ItemIcon.sprite = armorSO.Sprite;
        _Name.text = armorSO.Name;

        SetupPrefab(_ItemWeightPrefab, armorSO.Weight, toCompare.Weight, true);

        SetupPrefab(_FirePrefab, armorSO.FireResistance, toCompare.FireResistance);
        SetupPrefab(_IcePrefab, armorSO.IceResistance, toCompare.IceResistance);
        SetupPrefab(_LightningPrefab, armorSO.LightningResistance, toCompare.LightningResistance);
        SetupPrefab(_EarthPrefab, armorSO.EarthResistance, toCompare.EarthResistance);
        SetupPrefab(_VoidPrefab, armorSO.VoidResistance, toCompare.VoidResistance);
        SetupPrefab(_PiercingPrefab, armorSO.PierceResistance, toCompare.PierceResistance);
        SetupPrefab(_BluntPrefab, armorSO.BluntResistance, toCompare.BluntResistance);
        SetupPrefab(_ExplosivePrefab, armorSO.ExplosiveResistance, toCompare.ExplosiveResistance);
    }

    public void Setup(WeaponSO weaponSO, WeaponSO toCompare)
    {
        _ItemIcon.sprite = weaponSO.Sprite;
        _Name.text = weaponSO.Name;

        SetupPrefab(_ItemWeightPrefab, weaponSO.Weight, toCompare.Weight, true);

        if (weaponSO.DamageType.Equals(toCompare.Type)) {
            WeaponArmorStat prefab = GetPrefabForWeaponDamageType(weaponSO.DamageType);
            SetupPrefab(prefab, weaponSO.DamageMin, weaponSO.DamageMax, toCompare.DamageMin, toCompare.DamageMax);
        } 
        else
        {
            WeaponArmorStat prefabCurrent = GetPrefabForWeaponDamageType(weaponSO.DamageType);
            SetupPrefab(prefabCurrent, weaponSO.DamageMin, weaponSO.DamageMax, 0, 0);

            WeaponArmorStat prefabToCompare = GetPrefabForWeaponDamageType(toCompare.DamageType);
            SetupPrefab(prefabCurrent, 0, 0, toCompare.DamageMin, toCompare.DamageMax);
        }


        var weaponOneElementalEffects = weaponSO.ElementalDamages.ToDictionary(e => e.Type, e => e);
        var weaponTwoElementalEffects = toCompare.ElementalDamages.ToDictionary(e => e.Type, e => e);
        List<ElementalDamageType> elementalEffectsUnion = weaponOneElementalEffects.Keys.Union(weaponTwoElementalEffects.Keys).ToList();

        foreach (ElementalDamageType type in elementalEffectsUnion)
        {
            weaponOneElementalEffects.TryGetValue(type, out var effect1);
            weaponTwoElementalEffects.TryGetValue(type, out var effect2);

            var currentMin = effect1?.DamageMin ?? 0;
            var currentMax = effect1?.DamageMax ?? 0;
            var newMin = effect2?.DamageMin ?? 0;
            var newMax = effect2?.DamageMax ?? 0;

            WeaponArmorStat? prefab = GetPrefabForElementalDamageType(type);
            if (prefab != null)
            {
                SetupPrefab(prefab, currentMin, currentMax, newMin, newMax);
            }
        }

        //if (weaponSO.ElementalDamageType.Equals(toCompare.ElementalDamageType))
        //{
        //    WeaponArmorStat? prefab = GetPrefabForElementalDamageType(weaponSO.ElementalDamageType);
        //    if (prefab != null)
        //    {
        //        SetupPrefab(prefab, weaponSO.ElementalMin, weaponSO.ElementalMax, toCompare.ElementalMin, toCompare.ElementalMax);
        //    }
        //}
        //else
        //{
        //    WeaponArmorStat? prefabCurrent = GetPrefabForElementalDamageType(weaponSO.ElementalDamageType);
        //    if (prefabCurrent != null)
        //    {
        //        SetupPrefab(prefabCurrent, weaponSO.ElementalMin, weaponSO.ElementalMax, 0, 0);
        //    }

        //    WeaponArmorStat? prefabToCompare = GetPrefabForElementalDamageType(toCompare.ElementalDamageType);
        //    if (prefabToCompare != null)
        //    {
        //        SetupPrefab(prefabToCompare, 0, 0, toCompare.ElementalMin, toCompare.ElementalMax);
        //    }
        //}
    }

    // Used for damage resistance / weight (and moreIsWorse only read for weight)
    private void SetupPrefab(WeaponArmorStat prefab, float current, float toCompare, bool moreIsWorse = false)
    {
        if (current.Equals(toCompare))
        {
            prefab.Setup(Color.white, $"{current} (=)");
        }
        else
        {
            (Color color, string value) = GetColorValue(current, toCompare, moreIsWorse);

            prefab.Setup(color, value);
        }
    }

    // Used for damages
    private void SetupPrefab(WeaponArmorStat prefab, int currentMin, int currentMax, int toCompareMin, int toCompareMax, bool moreIsWorse = false)
    {
        if (currentMin.Equals(toCompareMin) && currentMax.Equals(toCompareMax))
        {
            prefab.Setup(Color.white, $"{currentMin} (=)", Color.white, $"{currentMax} (=)");
        }
        else
        {
            (Color colorMin, string valueMin) = GetColorValue(currentMin, toCompareMin, moreIsWorse);
            (Color colorMax, string valueMax) = GetColorValue(currentMax, toCompareMax, moreIsWorse);

            prefab.Setup(colorMin, valueMin, colorMax, valueMax);
        }
    }

    private (Color, string) GetColorValue(float current, float toCompare, bool moreIsWorse)
    {
        bool isMore = current > toCompare;
        string value = isMore ? $"{current} (+{current - toCompare})" : $"{current} (-{current - toCompare})";
        Color color = GetColor(isMore, moreIsWorse);
        return (color, value);
    }

    private Color GetColor(bool isMore, bool moreIsWorse)
    {
        if (moreIsWorse)
        {
            return isMore ? Color.red : Color.green;
        }
        else
        {
            return isMore ? Color.green : Color.red;
        }
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

using UnityEngine;

public enum MeleeWeaponType
{
    Stab, // Straight out
    Swing // Arc
}

[CreateAssetMenu( menuName = "ScriptableObjects/Attacks/MeleeWeapon")]
public class MeleeWeaponSO : WeaponSO
{
    public float Reach; // How far the hitbox extends
    public MeleeWeaponType WeaponType;
}

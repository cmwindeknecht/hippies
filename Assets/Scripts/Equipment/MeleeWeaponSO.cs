using UnityEngine;

public enum MeleeSwingType
{
    Stab, // Straight out
    Swing // Arc
}

[CreateAssetMenu( menuName = "ScriptableObjects/Equipment/MeleeWeapon")]
public class MeleeWeaponSO : WeaponSO
{
    public float Reach; // How far the hitbox extends
    public MeleeSwingType SwingType;
}

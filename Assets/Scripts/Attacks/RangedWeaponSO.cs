using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/RangedWeapon")]
public class RangedWeaponSO : WeaponSO
{
    public float Range; // How far the projectiles travel
    public Projectile ProjectilePrefab;
}

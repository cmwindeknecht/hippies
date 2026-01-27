using UnityEngine;

// Unsure if I'm going to do this (so its not used yet) --- probably should, I prefer to have manual or automatic weapons, but a twin stick makes more sense to have hold down regardless
public enum RangedWeaponType
{
    Manual,
    Automatic
}

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/RangedWeapon")]
public class RangedWeaponSO : WeaponSO
{
    public float Range; // How far the projectiles travel, 0 = infinite (think everything is infinite?)
    public Projectile ProjectilePrefab;
    public int BulletCost; // 1 for single shot, 2 for 2 bullets, 5 or whatever for shotguns, etc
}

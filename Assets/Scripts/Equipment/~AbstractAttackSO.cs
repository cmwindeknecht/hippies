using UnityEngine;

public enum DamageType
{
    Pierce, // Swords, arrows, etc
    Blunt, // Mauls, bullets, etc
    Explosive
}

public enum ElementalDamageType
{
    None,
    Fire,
    Ice,
    Lightning,
    Earth,
    Void
}

public abstract class AttackSO : ItemSO
{
    public DamageType DamageType = DamageType.Pierce;
    public ElementalDamageType ElementalDamageType = ElementalDamageType.None;

    public int DamageMin; // BaseDamageMin (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)
    public int DamageMax; // BaseDamageMax (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)

    public int ElementalMin = 0; // BaseDamageMin (in the event of ranged, projectiles modify this along with intelligence/etc just like melee)
    public int ElementalMax = 0; // BaseDamageMax (in the event of ranged, projectiles modify this along with intelligence/etc just like melee)

    public float Knockback; // Anything can have knockback --- the weapon itself, the projectile (explosive), the melee weapon, a spell
}

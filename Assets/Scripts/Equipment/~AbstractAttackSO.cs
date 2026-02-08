using System.Collections.Generic;
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

[System.Serializable]
public class ElementalDamage
{
    public ElementalDamageType Type;
    public int DamageMin;
    public int DamageMax;
}

public abstract class AttackSO : ItemSO
{
    public DamageType DamageType = DamageType.Pierce;
    public List<ElementalDamage> ElementalDamages = new();

    public int DamageMin; // BaseDamageMin (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)
    public int DamageMax; // BaseDamageMax (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)

    public float Knockback; // Anything can have knockback --- the weapon itself, the projectile (explosive), the melee weapon, a spell
}

using UnityEngine;

public abstract class AttackSO : ItemSO
{
    public int DamageMin; // BaseDamageMin (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)
    public int DamageMax; // BaseDamageMax (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)

    public float Knockback; // Anything can have knockback --- the weapon itself, the projectile (explosive), the melee weapon, a spell
}

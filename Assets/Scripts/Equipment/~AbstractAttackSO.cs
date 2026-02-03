using UnityEngine;

public abstract class AttackSO : ScriptableObject
{
    public string Name;
    //public Sprite Sprite; // Not sure I need this?  Unsure depending on how I do animations
    //public AnimationClip AnimationClip; // Maybet this makes more sense than the sprite?  Again... really depends on how I do shit later
    public int DamageMin; // BaseDamageMin (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)
    public int DamageMax; // BaseDamageMax (in the event of ranged, projectiles modify this along with strength/agility/etc just like melee)

    public float Knockback; // Anything can have knockback --- the weapon itself, the projectile (explosive), the melee weapon, a spell
}

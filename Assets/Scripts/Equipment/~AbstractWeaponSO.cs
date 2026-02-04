using UnityEngine;



public abstract class WeaponSO : AttackSO
{
    public int HandsRequirement = 1;

    public float AttackRate = .25f; // Speed of attack (how fast a gun fires, a sword swings, etc)
    public float AttackCooldown = 1f; // Time between attacks 

    // TODO before equipping a weapon, if requirement is 10+ less than the player stat, block the equip.  
    public int StrengthRequirement = 1;
    public int AgilityRequirement = 1;
    public int IntelligenceRequirement = 1;

    public AudioClip SoundEffect;
}

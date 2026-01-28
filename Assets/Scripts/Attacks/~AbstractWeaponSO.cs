using UnityEngine;

public abstract class WeaponSO : AttackSO
{
    public float AttackRate = .25f; // Speed of attack (how fast a gun fires, a sword swings, etc)
    public float AttackCooldown = 1f; // Time between attacks 
    public AudioClip SoundEffect;
}

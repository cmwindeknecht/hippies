using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Characters/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Stats Behavior")]
    public int Health = 100;
    public float HealthRegenerationRate = .01f;

    public int Energy = 100;
    public float EnergyRegenerationRate = .01f;

    public int Magic = 10;
    public float MagicRegenerationRate = .01f;

    public float MovementSpeed;

    // TODO shit like burst only makes sense for ranged, so should have a MeleeEnemySO and a RangedEnemySO or whatever ultimately makes sense 
    [Header("Attack Behavior")]
    public WeaponSO WeaponSO;
    public float AttackRange = 5f; // When the enemy should start shooting
    public float AttackCooldown = 2f; // Time between attack attempts
    public int BurstCount = 1; // Shots per burst (1 = single shot, 3 = triple burst)
    public float BurstDelay = 0.2f; // Time between shots in a burst

    public int Level = 1;
    public int Strength = 1;
    public int Agility = 1;
    public int Vitality = 1;
    public int Stamina = 1;
    public int Intelligence = 1;
    public int Luck = 1;
    public int Spirit = 1;

    public int ExperienceOnDeath = 1; // Experience gained across all stats on death (so if its the default, you get 1xp for strength, stamina, etc)
}

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Characters/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Stats Behavior")]
    public int Health;
    public float MovementSpeed;

    // TODO shit like burst only makes sense for ranged, so should have a MeleeEnemySO and a RangedEnemySO or whatever ultimately makes sense 
    [Header("Attack Behavior")]
    public WeaponSO WeaponSO;
    public float AttackRange = 5f; // When the enemy should start shooting
    public float AttackCooldown = 2f; // Time between attack attempts
    public int BurstCount = 1; // Shots per burst (1 = single shot, 3 = triple burst)
    public float BurstDelay = 0.2f; // Time between shots in a burst
}

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Characters/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Stats Behavior")]
    public int Health;
    public float MovementSpeed;

    [Header("Attack Behavior")]
    public float attackCooldown = 2f; // Time between attack attempts
    public int burstCount = 1; // Shots per burst (1 = single shot, 3 = triple burst)
    public float burstDelay = 0.2f; // Time between shots in a burst
}

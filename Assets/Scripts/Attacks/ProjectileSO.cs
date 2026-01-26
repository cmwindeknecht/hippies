
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/Projectile")]
public class ProjectileSO : AttackSO
{
    public float Speed = 10f; // A majority should have the same speed
    public float Lifetime = 5f; // A majority should have the same lifetime
    public Sprite Sprite; 
    public bool Hollowpoint = false; // it can go through multiple enemies
}
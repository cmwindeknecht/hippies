using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/MagicAttack")]
public class MagicAttackSO : WeaponSO
{
    public float StatusEffectDuration; // 0 for instant, 1 if you want it to occur every one second, etc
    public float StatusEffectIterations; // 0 for instant, 3 total 
    // TODO public StatusEffectSO StatusEffect; // Optional status applied
}
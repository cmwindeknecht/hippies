using UnityEngine;

public class MeleeWeapon : Weapon
{
    public override bool Attack()
    {
        Debug.Log($"Melee Attack() Called - CanAttack ={CanAttack}");
        if (!CanAttack) return false;
        CanAttack = false;

        AttackCooldown().Forget();
        return true;
    }
}
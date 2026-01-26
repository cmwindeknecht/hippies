using UnityEngine;

public class RangedWeapon : Weapon
{
    public override bool Attack()
    {
        Debug.Log($"Ranged Attack() Called - CanAttack ={CanAttack}");
        if (!CanAttack) return false;
        CanAttack = false;

        AttackCooldown().Forget();
        return true;
    }
}

using UnityEngine;

public class RangedWeapon : Weapon
{
    public override bool Attack()
    {
        if (!CanAttack) return false;
        CanAttack = false;

        AttackCooldown().Forget();
        return true;
    }
}

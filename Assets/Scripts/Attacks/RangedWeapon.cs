using UnityEngine;

public class RangedWeapon : Weapon
{
    public override bool Attack(Vector3 targetPosition)
    {
        if (!CanAttack) return false;

        AttackCooldown().Forget();
        return true;
    }
}

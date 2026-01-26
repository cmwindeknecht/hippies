using Cysharp.Threading.Tasks;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    // Returns false if the attack didn't occur, true if it did
    public override bool Attack(Vector3 targetPosition)
    {
        if (!CanAttack) return false;

        // TODO based on the owner of the weapon, need to getMask Enemy/Player, and then in the foreach getComponent Enemy/Player
        // TODO might make more sense to have a collder on the weapon and do an ontriggerenter so I can just check tags and shit
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, ((MeleeWeaponSO)WeaponSO).Reach, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // TODO way more advanced shit --- need to take into account strength/
                enemy.TakeDamage(Random.Range(WeaponSO.DamageMin, WeaponSO.DamageMax + 1));
            }
        }

        AttackCooldown().Forget();
        return true;
    }
}
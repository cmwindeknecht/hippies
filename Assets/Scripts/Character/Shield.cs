using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Shield : MonoBehaviour
{
    private Character _Owner;
    private ArmorSO _ShieldSO;
    private bool _IsInitialized = false;
    private HashSet<Collider2D> _HitTargets;

    private void Awake()
    {
        _HitTargets = new();
    }

    public void Setup(ArmorSO shieldSO, Character owner)
    {
        _IsInitialized = false;

        _Owner = owner;
        _ShieldSO = shieldSO;

        _IsInitialized = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_IsInitialized) return;   

        // prevent double taps
        if (!_HitTargets.Add(collision.collider))
        {
            return;
        }

        // ignore non hitbox / projectile collisions
        if (collision.collider.GetComponent<MeleeHitBox>() == null && collision.collider.GetComponent<Projectile>() == null)
        {
            return;
        }

        // TODO maybe all of this logic should be in the Projectile/Hitbox code --- otherwise its weird as they check for player/enemy on enter, they should check if shield then if enemy or player
        if (collision.collider.TryGetComponent<MeleeHitBox>(out MeleeHitBox meleeHitBox))
        {
            HandleAttack(meleeHitBox.GetDamage(), meleeHitBox.GetKnockBack(), meleeHitBox.AttackDirection, meleeHitBox.gameObject, meleeHitBox.DamageType, meleeHitBox.ElementalDamageTypes, meleeHitBox.Owner);
        }
        else if (collision.collider.TryGetComponent<Projectile>(out Projectile projectile))
        {
            HandleAttack(projectile.GetDamage(), projectile.GetKnockBack(), projectile.AttackDirection, projectile.gameObject, projectile.DamageType, projectile.ElementalDamageTypes, projectile.Owner);
        }
    }

    private void HandleAttack(int baseDamage, float baseKnockback, Vector2 attackDirection, GameObject hitObject, DamageType? damageType, List<ElementalDamage> elementalDamageTypes, Character attacker)
    {
        int damage = _Owner.GetPossibleDamage(baseDamage, damageType, elementalDamageTypes, isShielding: true);
        float modifiedKnockback = Mathf.Max(0, baseKnockback - _ShieldSO.KnockbackResistance);

        if (damage > 0 || modifiedKnockback > 0)
        {
            _Owner.TakeDamage(damage, attacker, modifiedKnockback > 0 ? attackDirection : null, modifiedKnockback);
        }

        Destroy(hitObject);
    }
}

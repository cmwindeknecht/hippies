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
            HandleAttack(meleeHitBox.Damage, meleeHitBox.Knockback, meleeHitBox.AttackDirection, collision.gameObject);
        }
        else if (collision.collider.TryGetComponent<Projectile>(out Projectile projectile))
        {
            HandleAttack(projectile.Damage, projectile.Knockback, projectile.AttackDirection, collision.gameObject);
        }
    }

    private void HandleAttack(int baseDamage, float baseKnockback, Vector2 attackDirection, GameObject hitObject)
    {
        int damage = _Owner.GetPossibleDamage(baseDamage, isShielding: true);
        float modifiedKnockback = Mathf.Max(0, baseKnockback - _ShieldSO.KnockbackResistance);

        if (damage > 0 || modifiedKnockback > 0)
        {
            _Owner.TakeDamage(damage, modifiedKnockback > 0 ? attackDirection : null, modifiedKnockback);
        }

        Destroy(hitObject);
    }
}

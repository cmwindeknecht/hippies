using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Character _Owner;
    private ShieldSO _ShieldSO;
    private bool _IsInitialized = false;
    private HashSet<Collider2D> _HitTargets;

    private void Awake()
    {
        _HitTargets = new();
    }

    public void Setup(ShieldSO shieldSO, Character owner)
    {
        _IsInitialized = false;

        _Owner = owner;
        _ShieldSO = shieldSO;

        _IsInitialized = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_IsInitialized) { return; }    

        // prevent double taps
        if (!_HitTargets.Add(collision.collider))
        {
            return;
        }

        // ignore non hitbox / projectile collisions
        if (collision.collider.GetComponent<MeleeHitBox>() == null || collision.collider.GetComponent<Projectile>() == null)
        {
            return;
        }

        if (_Owner is Player)
        {
            Player player = _Owner as Player;
            if (collision.collider.TryGetComponent<MeleeHitBox>(out MeleeHitBox meleeHitBox))
            {
                int damage = Mathf.Max(0, meleeHitBox.Damage - Random.Range(_ShieldSO.DamageNegationMin, _ShieldSO.DamageNegationMax + 1));
                float knockback = Mathf.Max(0, meleeHitBox.Knockback - _ShieldSO.KnockbackResistance);
               

                if (damage > 0 || knockback > 0)
                {
                    player.TakeDamage(damage, knockback > 0 ? meleeHitBox.AttackDirection : null, knockback);
                }

                Destroy(collision.gameObject);
            }
            else if (collision.collider.TryGetComponent<Projectile>(out Projectile projectile))
            {
                int damage = Mathf.Max(0, projectile.Damage - Random.Range(_ShieldSO.DamageNegationMin, _ShieldSO.DamageNegationMax + 1));
                float knockback = Mathf.Max(0, projectile.Knockback - _ShieldSO.KnockbackResistance);
                Debug.Log($"Projectile hit shield for damage {damage} and knockback {knockback}");

                if (damage > 0 || knockback > 0)
                {
                    player.TakeDamage(damage, knockback > 0 ? projectile.AttackDirection : null, knockback);
                }

                Destroy(collision.gameObject);
            }
        }
        else if (_Owner is Enemy)
        {
            Enemy enemy = _Owner as Enemy;
            if (collision.collider.TryGetComponent<MeleeHitBox>(out MeleeHitBox meleeHitBox))
            {
                int damage = Mathf.Max(0, meleeHitBox.Damage - Random.Range(_ShieldSO.DamageNegationMin, _ShieldSO.DamageNegationMax + 1));
                float knockback = Mathf.Max(0, meleeHitBox.Knockback - _ShieldSO.KnockbackResistance);

                if (damage > 0 || knockback > 0)
                {
                    enemy.TakeDamage(damage, knockback > 0 ? meleeHitBox.AttackDirection : null, knockback);
                }

                Destroy(collision.gameObject);
            }
            else if (collision.collider.TryGetComponent<Projectile>(out Projectile projectile))
            {
                int damage = Mathf.Max(0, projectile.Damage - Random.Range(_ShieldSO.DamageNegationMin, _ShieldSO.DamageNegationMax + 1));
                float knockback = Mathf.Max(0, projectile.Knockback - _ShieldSO.KnockbackResistance);

                if (damage > 0 || knockback > 0)
                {
                    enemy.TakeDamage(damage, knockback > 0 ? projectile.AttackDirection : null, knockback);
                }

                Destroy(collision.gameObject);
            }
        }
    }
}

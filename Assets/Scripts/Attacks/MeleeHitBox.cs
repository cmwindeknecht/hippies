using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    private Character _Owner;
    private Rigidbody2D _Rigidbody;
    public float _Radius;
    public float _Duration;
    
    private float _AttackTimer;
    private MeleeSwingType _WeaponType;
    
    private int _Damage;
    public int Damage => _Damage;

    private float _Knockback;
    public float Knockback => _Knockback;

    private Vector3 _AttackDirection;
    public Vector3 AttackDirection => _AttackDirection;

    private bool _IsInitialized;

    private HashSet<Collider2D> _HitTargets;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _AttackTimer = 0f;
        _HitTargets = new();
        _IsInitialized = false;
    }

    public void Initialize(Character owner, Vector3 attackDirection, MeleeWeaponSO weaponSO)
    {
        _Owner = owner;
        _AttackDirection = attackDirection.normalized;
        _Radius = weaponSO.Reach;
        _WeaponType = weaponSO.SwingType;
        _Duration = GetDuration(weaponSO);
        _Damage = GetDamage(weaponSO);
        _Knockback = GetKnockBack(weaponSO); ;

        _IsInitialized = true;        
    }

    private int GetDamage(MeleeWeaponSO weaponSO)
    {
        // TODO factor in weight of weapon vs strength, etc
        return Utilities.GetRandomInt(weaponSO.DamageMin, weaponSO.DamageMax);
    }

    private float GetDuration(MeleeWeaponSO weaponSO)
    {
        // TODO factor in agility in weapon speed, etc
        return weaponSO.AttackRate;
    }

    private float GetKnockBack(MeleeWeaponSO weaponSO)
    {
        // TODO factor in strength, swing speed, etc
        return weaponSO.Knockback;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_IsInitialized) return;
        if (!_HitTargets.Add(collision))
        {
            Destroy(gameObject);
            return;
        }

        if (_Owner is Player)
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                HandleAdversaryHit(enemy);
            }
            
        }
        else if (_Owner is Enemy)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                HandleAdversaryHit(player);
            }
        }
    }

    private void HandleAdversaryHit(Character adversaryHit)
    {
        int damage = adversaryHit.GetPossibleDamage(_Damage, isShielding: false);

        if (damage > 0 || _Knockback < 0)
        {
            adversaryHit.TakeDamage(_Damage, _Knockback > 0 ? _AttackDirection : null, _Knockback);
        }
    }

    private void FixedUpdate()
    {
        if (!_IsInitialized) return;

        if (_WeaponType.Equals(MeleeSwingType.Swing))
        {
            SpawnArcingHitBox();
        }
        else
        {
            throw new System.Exception($"Unknown weapon type {_WeaponType}");
        }
    }

    private void SpawnArcingHitBox()
    {
        _AttackTimer += Time.fixedDeltaTime; // Changed from Time.deltaTime
        float normalizedDuration = Mathf.Clamp01(_AttackTimer / _Duration);
        // Lerp = Sweep 180 degrees, left to right, Atan2 = rotate in 2D
        float angle = Mathf.Lerp(90f, -90f, normalizedDuration) + Mathf.Atan2(_AttackDirection.y, _AttackDirection.x) * Mathf.Rad2Deg;
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        _Rigidbody.MovePosition((Vector3) _Owner.Position + direction * _Radius);
        if (normalizedDuration >= 1f)
        {
            Destroy(gameObject);
        }
    }
}

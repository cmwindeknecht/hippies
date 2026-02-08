using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    private MeleeWeaponSO _WeaponSO;
    public DamageType? DamageType => _WeaponSO.DamageType;
    public List<ElementalDamage> ElementalDamageTypes => _WeaponSO.ElementalDamages;
    public Character Owner { get; private set; }

    private Rigidbody2D _Rigidbody;
    
    private Vector3 _AttackDirection;
    public Vector3 AttackDirection => _AttackDirection;

    private float _AttackTimer;
    private bool _IsInitialized;

    private HashSet<Collider2D> _HitTargets;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _AttackTimer = 0f;
        _HitTargets = new();
        _IsInitialized = false;
    }

    private void FixedUpdate()
    {
        if (!_IsInitialized) return;

        // Destroy if the owner dies
        if (Owner == null)
        {
            Destroy(gameObject);
            return;
        }

        if (_WeaponSO.SwingType.Equals(MeleeSwingType.Swing))
        {
            SpawnArcingHitBox();
        }
        else
        {
            throw new System.Exception($"Unknown weapon type {_WeaponSO.SwingType}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_IsInitialized) return;
        if (!_HitTargets.Add(collision))
        {
            Destroy(gameObject);
            return;
        }

        if (Owner is Player)
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                HandleAdversaryHit(enemy);
            }

        }
        else if (Owner is Enemy)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                HandleAdversaryHit(player);
            }
        }
    }

    public void Initialize(Character owner, Vector3 attackDirection, MeleeWeaponSO weaponSO)
    {
        Owner = owner;
        _WeaponSO = weaponSO;
        _AttackDirection = attackDirection.normalized;
        _IsInitialized = true;        
    }

    public int GetDamage()
    {
        // TODO factor in weight of weapon vs strength, etc
        return Utilities.GetRandomInt(_WeaponSO.DamageMin, _WeaponSO.DamageMax);
    }

    public float GetKnockBack()
    {
        // TODO factor in strength, swing speed, etc
        return _WeaponSO.Knockback;
    }

    private void HandleAdversaryHit(Character adversaryHit)
    {
        int damage = adversaryHit.GetPossibleDamage(GetDamage(), _WeaponSO.DamageType, _WeaponSO.ElementalDamages, isShielding: false);
        // TODO did critical hit occur
        float knockback = GetKnockBack();

        if (damage > 0 || knockback < 0)
        {
            adversaryHit.TakeDamage(damage, adversaryHit, knockback > 0 ? _AttackDirection : null, knockback);
            if (Owner is Player player)
            {
                // TODO need to refactor all kinds of shit for elemental damage, but since its not implemented at all right now, I'm ignoring it
                player.GainExperienceOnMelee(_WeaponSO, damage, 0);
            }
        }
    }

    private void SpawnArcingHitBox()
    {
        _AttackTimer += Time.fixedDeltaTime; // Changed from Time.deltaTime
        float normalizedDuration = Mathf.Clamp01(_AttackTimer / GetDuration(_WeaponSO));
        // Lerp = Sweep 180 degrees, left to right, Atan2 = rotate in 2D
        float angle = Mathf.Lerp(90f, -90f, normalizedDuration) + Mathf.Atan2(_AttackDirection.y, _AttackDirection.x) * Mathf.Rad2Deg;
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        _Rigidbody.MovePosition((Vector3) Owner.Position + direction * _WeaponSO.Reach);
        if (normalizedDuration >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private float GetDuration(MeleeWeaponSO weaponSO)
    {
        // TODO factor in agility in weapon speed, etc
        return weaponSO.AttackRate;
    }
}

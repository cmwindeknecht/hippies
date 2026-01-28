using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    private Character _Character;
    private Rigidbody2D _Rigidbody;
    public float _Radius;
    public float _Duration;
    private Vector3 _StartPosition;
    
    private float _AttackTimer;
    private MeleeWeaponType _WeaponType;
    
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

    public void Initialize(Character character, Vector3 attackDirection, MeleeWeaponSO weaponSO)
    {
        _Character = character;
        _StartPosition = _Character.Position;
        _AttackDirection = attackDirection.normalized;
        _Radius = weaponSO.Reach;
        _Duration = weaponSO.AttackRate;
        _WeaponType = weaponSO.WeaponType;
        _Damage = Random.Range(weaponSO.DamageMin, weaponSO.DamageMax + 1);
        _Knockback = weaponSO.Knockback;

        _IsInitialized = true;        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_IsInitialized) return;

        if (_Character is Player)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (!collision.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    throw new System.Exception("Enemy tag does not have Enemy Component!");
                }
                enemy.TakeDamage(_Damage, _Knockback > 0 ? _AttackDirection : null, _Knockback);
            }
        }
        else if (_Character is Enemy)
        {
            if (collision.CompareTag("Player"))
            {
                if (!collision.TryGetComponent<Player>(out Player player))
                {
                    throw new System.Exception("Player tag does not have Player Component!");
                }
                player.TakeDamage(_Damage, _Knockback > 0 ? _AttackDirection : null, _Knockback);
            }
        }
    }

    private void FixedUpdate() // Changed from Update
    {
        if (!_IsInitialized) return;
        if (_WeaponType.Equals(MeleeWeaponType.Swing))
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
        _Rigidbody.MovePosition((Vector3) _Character.Position + direction * _Radius);
        if (normalizedDuration >= 1f)
        {
            Destroy(gameObject);
        }
    }
}

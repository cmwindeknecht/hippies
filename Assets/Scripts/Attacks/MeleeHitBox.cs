using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    private Rigidbody2D _Rigidbody;
    public float _Radius;
    public float _Duration;
    private Vector3 _StartPosition;
    private Vector3 _AttackDirection;
    private float _AttackTimer;
    private MeleeWeaponType _WeaponType;
    private int _Damage;
    private bool _IsInitialized;

    private HashSet<Collider2D> _HitTargets;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _AttackTimer = 0f;
        _HitTargets = new();
        _IsInitialized = false;
    }

    public void Initialize(Vector3 playerPosition, Vector3 attackDirection, float reach, float attackDuration, MeleeWeaponType meleeWeaponType, int damage)
    {
        _StartPosition = playerPosition;
        _AttackDirection = attackDirection.normalized;
        _Radius = reach;
        _Duration = attackDuration;
        _WeaponType = meleeWeaponType;
        _Damage = damage;

        //_Rigidbody.MovePosition(_StartPosition + _AttackDirection * _Radius);

        _IsInitialized = true;        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_IsInitialized) return;

        // TODO need to know if the owner is player or enemy so I can reuse this shit
        if (collision.CompareTag("Enemy"))
        {
            // Prevent unintended double taps
            if (!_HitTargets.Add(collision))
            {
                return;
            }

            if (!collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                throw new System.Exception("Enemy tag does not have Enemy Component!");
            }
            Debug.Log($"Hit enemy - previous health {enemy.Health}");
            enemy.TakeDamage(_Damage);
            Debug.Log($"Hit enemy - after health {enemy.Health}");
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
        _Rigidbody.MovePosition(_StartPosition + direction * _Radius);
        if (normalizedDuration >= 1f)
        {
            Destroy(gameObject);
        }
    }
}

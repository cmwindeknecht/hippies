using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO _ProjectileSO;
    public ProjectileSO ProjectileSO => _ProjectileSO;

    private Rigidbody2D _Rigidbody;
    private float _AttackTimer;
    private bool _IsInitialized;
    private HashSet<Collider2D> _HitTargets;
    private Character _Owner;
    private Character _Target;

    private int _Damage;
    public int Damage => _Damage;

    private float _Knockback;
    public float Knockback => _Knockback;

    private Vector3 _AttackDirection;
    public Vector3 AttackDirection => _AttackDirection;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _AttackTimer = 0f;
        _IsInitialized = false;
        _HitTargets = new();
    }

    // TODO Enemy Initialization (could also be for like homing missle type shit for the player?)
    //   Current iteration just fires a burst where the player WAS
    //   Should have a second version that fires where the player IS but this requires a new EnemyAttackController and all that, thats future shit
    public void Initialize(Vector3 direction, RangedWeaponSO weaponSO, Character projectileOwner, Character target)
    {
        _Target = target;
        Initialize(direction, weaponSO, projectileOwner);
    }

    public void Initialize(Vector3 direction, RangedWeaponSO weaponSO, Character projectileOwner)
    {
        _Owner = projectileOwner;
        _Damage = Random.Range(weaponSO.DamageMin, weaponSO.DamageMax + 1) + Random.Range(_ProjectileSO.DamageMin, _ProjectileSO.DamageMax + 1);
        _Knockback = weaponSO.Knockback + _ProjectileSO.Knockback;
        _AttackDirection = direction.normalized;
        
        float angle = Mathf.Atan2(_AttackDirection.y, _AttackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // Set velocity instead of moving in FixedUpdate
        _Rigidbody.linearVelocity = _AttackDirection * _ProjectileSO.Speed;

        _IsInitialized = true;
        Destroy(gameObject, _ProjectileSO.Lifetime);
    }

    private void Update()
    {
        if (!_IsInitialized) return;
        _AttackTimer += Time.deltaTime;
        if (_AttackTimer >= _ProjectileSO.Lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent unintended double taps
        if (!_IsInitialized) return;
        if (!_HitTargets.Add(collision.collider))
        {
            Destroy(gameObject);
            return;
        }

        if (_Owner is Player)
        {
            if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                HandleAdversaryHit(enemy);
            }
            
        }
        else if (_Owner is Enemy)
        {
            if (collision.collider.TryGetComponent<Player>(out Player player))
            {
                HandleAdversaryHit(player);
            }
        }

        if (_ProjectileSO.Hollowpoint && collision.collider.CompareTag("Collision"))
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
}
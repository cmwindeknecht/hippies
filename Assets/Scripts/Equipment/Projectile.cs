using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO _ProjectileSO;
    private RangedWeaponSO _RangedWeaponSO;
    public DamageType? DamageType => _ProjectileSO.DamageType;
    public List<ElementalDamage> ElementalDamageTypes => _ProjectileSO.ElementalDamages.Union(_RangedWeaponSO.ElementalDamages).ToList();

    public Character Owner { get; private set; }
    public Character _Target {  get; private set; }

    private Rigidbody2D _Rigidbody;

    private float _AttackTimer;
    private bool _IsInitialized;
    private HashSet<Collider2D> _HitTargets;


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
    public void Initialize(Vector3 direction, RangedWeaponSO weaponSO, Character owner, Character target)
    {
        _Target = target;
        Initialize(direction, weaponSO, owner);
    }

    public void Initialize(Vector3 direction, RangedWeaponSO weaponSO, Character projectileOwner)
    {
        Owner = projectileOwner;
        _RangedWeaponSO = weaponSO;

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

        if (Owner is Player)
        {
            if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                HandleAdversaryHit(enemy);
            }
            
        }
        else if (Owner is Enemy)
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

    public int GetDamage()
    {
        // TODO factor in weight of weapon vs strength, etc
        return Random.Range(_RangedWeaponSO.DamageMin, _RangedWeaponSO.DamageMax + 1) + Random.Range(_ProjectileSO.DamageMin, _ProjectileSO.DamageMax + 1);
    }

    public float GetKnockBack()
    {
        // TODO factor in strength, swing speed, etc
        return _RangedWeaponSO.Knockback + _ProjectileSO.Knockback;
    }

    private void HandleAdversaryHit(Character adversaryHit)
    {
        int damage = adversaryHit.GetPossibleDamage(GetDamage(), _ProjectileSO.DamageType, ElementalDamageTypes, isShielding: false);
        float knockback = GetKnockBack();

        if (damage > 0 || knockback < 0)
        {
            adversaryHit.TakeDamage(damage, adversaryHit, knockback > 0 ? _AttackDirection : null, knockback);
            if (Owner is Player player)
            {
                // TODO need to refactor all kinds of shit for elemental damage, but since its not implemented at all right now, I'm ignoring it
                player.GainExperienceOnRanged(_ProjectileSO, damage, 0);
            }
        }
    }
}
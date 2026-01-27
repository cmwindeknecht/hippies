using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private Enemy _Enemy;
    EnemyInventory _Inventory;

    private Weapon _EquippedWeapon;

    [SerializeField] private GameObject _MeleeHitboxPrefab;

    void Awake()
    {
        _Inventory = GetComponent<EnemyInventory>();
        _Enemy = GetComponent<Enemy>();
    }

    public bool TryAttack(Vector3 targetPosition)
    {
        EnsureWeapon();
        return TryAttackByType(targetPosition);
    }

    private void EnsureWeapon()
    {
        if (_EquippedWeapon == null)
        {
            WeaponSO weaponSO = _Inventory.EquippedWeaponSO;
            if (weaponSO == null)
            {
                throw new System.Exception($"There is no weapon equipped --- this should be impossible!");
            }

            if (weaponSO is MeleeWeaponSO)
            {
                _EquippedWeapon = new MeleeWeapon();
            }
            else if (weaponSO is RangedWeaponSO)
            {
                _EquippedWeapon = new RangedWeapon();
            }
            else if (weaponSO is MagicAttackSO)
            {
                _EquippedWeapon = new MagicAttack();
            }
            else
            {
                throw new System.Exception($"Unknown weaponSO to of {weaponSO.name}");
            }

            _EquippedWeapon.Initialize(weaponSO);
        }
    }

    private bool TryAttackByType(Vector3 targetPosition)
    {
        Vector3 attackDirection = new Vector3(targetPosition.x, targetPosition.y, 0).normalized;
        if (_EquippedWeapon is RangedWeapon)
        {
            if (_EquippedWeapon.Attack())
            {
                RangedWeaponSO rangedWeaponSO = _EquippedWeapon.WeaponSO as RangedWeaponSO;
                Vector3 spawnPos = transform.position + attackDirection.normalized * 1.5f; // spawn in front of the player in the direction of the attack
                Projectile projectile = Instantiate(rangedWeaponSO.ProjectilePrefab, spawnPos, Quaternion.identity);
                projectile.Initialize(attackDirection.normalized, rangedWeaponSO);
                return true;
            }
            return false;
        }

        if (_EquippedWeapon is MeleeWeapon)
        {
            if (_EquippedWeapon.Attack())
            {
                MeleeWeaponSO meleeWeaponSO = _EquippedWeapon.WeaponSO as MeleeWeaponSO;
                GameObject hitBoxInstance = Instantiate(_MeleeHitboxPrefab, transform.position, Quaternion.identity);
                MeleeHitBox meleeHitBox = hitBoxInstance.GetComponent<MeleeHitBox>();
                meleeHitBox.Initialize(_Enemy, attackDirection.normalized, meleeWeaponSO);

                return true;
            }
            return false;
        }

        if (_EquippedWeapon is MagicAttack)
        {
            return _EquippedWeapon.Attack();
        }

        throw new System.Exception($"Unknown attack type for weapon type {_EquippedWeapon.WeaponSO.Name}");
    }

    // TODO Proper way to do this but not now
    //private Vector3 GetBulletSpawnPosition()
    //{
    //    Collider2D playerCol = GetComponent<Collider2D>();
    //    Collider2D projCol = projectilePrefab.GetComponent<Collider2D>();

    //    float playerRadius = playerCol.bounds.extents.magnitude;
    //    float projRadius = projCol.bounds.extents.magnitude;

    //    float spawnOffset = playerRadius + projRadius + 0.05f;

    //    Vector3 spawnPos = transform.position + (Vector3)attackDir.normalized * spawnOffset;
    //}
}

using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    PlayerInventory _Inventory;

    private Weapon _EquippedWeapon;

    void Awake()
    {
        _Inventory = GetComponent<PlayerInventory>();
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
        if (_EquippedWeapon is RangedWeapon)
        {
            if (_EquippedWeapon.Attack(targetPosition))
            {
                RangedWeaponSO rangedWeaponSO = _EquippedWeapon.WeaponSO as RangedWeaponSO;
                Projectile projectileInstance = Instantiate(rangedWeaponSO.ProjectilePrefab, transform.position, Quaternion.identity);
                projectileInstance.Initialize(targetPosition, Random.Range(rangedWeaponSO.DamageMin, rangedWeaponSO.DamageMax + 1));
                return true;
            }
            return false;
        }

        if (_EquippedWeapon is MeleeWeapon)
        {
            return _EquippedWeapon.Attack(targetPosition);
        }

        if (_EquippedWeapon is MagicAttack)
        {
            return _EquippedWeapon.Attack(targetPosition);
        }

        throw new System.Exception($"Unknown attack type for weapon type {_EquippedWeapon.WeaponSO.Name}");
    }
}

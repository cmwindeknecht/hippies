using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    private Player _Player;
    private PlayerInventory _Inventory;
    [SerializeField] private Shield _Shield;

    private Weapon _EquippedWeapon;
    private ArmorSO _EquippedShield;

    [SerializeField] private GameObject _MeleeHitboxPrefab;

    private InputAction _ShieldInputAction;
    private bool _IsShielding = false;

    private void Awake()
    {
        _Player = GetComponent<Player>();
        _Inventory = GetComponent<PlayerInventory>();
        _ShieldInputAction = InputSystem.actions.FindAction("Shield");
    }

    private void Update()
    {
        // If I want shield to be a toggle
        //if (_ShieldInputAction.triggered)
        //{
        //    _IsShielding = !_IsShielding;
        //}
        // If I want shield to be a hold
        _IsShielding = _ShieldInputAction.IsPressed();

        ShowShield();
    }

    private void ShowShield()
    {
        if (_IsShielding)
        {
            EnsureShield();

            if (_EquippedShield == null) return;

            _Shield.gameObject.SetActive(true);
        }
        else
        {
            _Shield.gameObject.SetActive(false);
        }
    }

    public bool TryAttack(Vector3 targetPosition)
    {
        if (_IsShielding) return false;

        EnsureWeapon();
        return TryAttackByType(targetPosition);
    }

    private void EnsureWeapon()
    {
        if (_EquippedWeapon == null || !_EquippedWeapon.WeaponSO.GetInstanceID().Equals(_Inventory.CurrentWeapon.GetInstanceID()))
        {
            WeaponSO weaponSO = _Inventory.CurrentWeapon;
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

    private void EnsureShield()
    {
        if (_EquippedShield == null)
        {
            ArmorSO shieldSO = _Inventory.EquippedShieldSO;

            if (shieldSO != null )
            {
                _EquippedShield = shieldSO;
                _Shield.Setup(shieldSO, _Player);
            }
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
                Vector3 spawnPos = transform.position + attackDirection.normalized * 1.1f; // spawn in front of the player in the direction of the attack
                Projectile projectile = Instantiate(rangedWeaponSO.ProjectilePrefab, spawnPos, Quaternion.identity);
                projectile.Initialize(attackDirection.normalized, rangedWeaponSO, _Player);
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
                // TODO just pass in the weapon SO I think
                meleeHitBox.Initialize(_Player, attackDirection.normalized, meleeWeaponSO);

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

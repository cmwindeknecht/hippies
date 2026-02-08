using System;
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

    private const float MELEE_COST_PER_WEAPON_WEIGHT = .5f;
    private const float RANGED_ENERGY_COST_BY_WEIGHT = .25f;

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
            SetRotationAngle();
        }
        else
        {
            _Shield.gameObject.SetActive(false);
        }
    }

    private void SetRotationAngle()
    {
        if (_Player.FacingDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(_Player.FacingDirection.y, _Player.FacingDirection.x) * Mathf.Rad2Deg;
            float snappedAngle = Mathf.Round(angle / 45f) * 45f;

            // Rotate the shield to face the direction
            _Shield.transform.rotation = Quaternion.Euler(0, 0, snappedAngle);

            // Position it in front of the player
            float distance = .5f;
            Vector3 offset = _Player.FacingDirection.normalized * distance;
            _Shield.transform.position = transform.position + offset;
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
        if (_EquippedWeapon is RangedWeapon rangedWeapon)
        {
            int energyCost = GetRangedEnergyCost((RangedWeaponSO)_EquippedWeapon.WeaponSO);
            if (!_Player.Stats.Energy.CanSpend(energyCost))
            {
                // TODO send event if player to make UI flash the energy to notify the player can't do shit
                return false;
            }

            if (_EquippedWeapon.Attack())
            {
                _Player.SpendEnergy(energyCost);

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
            int energyCost = GetMeleeEnergyCost((MeleeWeaponSO)_EquippedWeapon.WeaponSO);
            if (!_Player.Stats.Energy.CanSpend(energyCost))
            {
                // TODO send event if player to make UI flash the energy to notify the player can't do shit
                return false;
            }

            if (_EquippedWeapon.Attack())
            {
                _Player.SpendEnergy(energyCost);

                MeleeWeaponSO meleeWeaponSO = _EquippedWeapon.WeaponSO as MeleeWeaponSO;
                GameObject hitBoxInstance = Instantiate(_MeleeHitboxPrefab, transform.position, Quaternion.identity);
                MeleeHitBox meleeHitBox = hitBoxInstance.GetComponent<MeleeHitBox>();
                meleeHitBox.Initialize(_Player, attackDirection.normalized, meleeWeaponSO);

                return true;
            }
            return false;
        }

        if (_EquippedWeapon is MagicAttack)
        {
            if (!_Player.Stats.Magic.CanSpend(((MagicAttackSO)_EquippedWeapon.WeaponSO).ManaCost))
            {
                // TODO send event if player to make UI flash the magic to notify the player can't do shit
                return false;
            }

            if (_EquippedWeapon.Attack())
            {
                _Player.Stats.SpendMagic(((MagicAttackSO)_EquippedWeapon.WeaponSO).ManaCost);

                throw new NotImplementedException();
            }
            return false;
        }

        throw new System.Exception($"Unknown attack type for weapon type {_EquippedWeapon.WeaponSO.Name}");
    }

    private int GetMeleeEnergyCost(MeleeWeaponSO meleeWeaponSO)
    {
        return Mathf.Max(1, (int)(MELEE_COST_PER_WEAPON_WEIGHT * meleeWeaponSO.Weight));
    }

    private int GetRangedEnergyCost(RangedWeaponSO rangedWeaponSO)
    {
        return Mathf.Max(1, (int)(RANGED_ENERGY_COST_BY_WEIGHT * (rangedWeaponSO.Weight))); // TODO need access to the projectile so on the ranged weapon, add it to the weapon weight for calculations
    }
}

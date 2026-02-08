using UnityEngine;

public class Player : Character
{
    public CharacterType CharacterType = CharacterType.Player;

    private PlayerController _Controller;
    public Vector2 FacingDirection => _Controller.FacingDirection;
    private PlayerVisual _PlayerVisual;
    private new PlayerInventory _Inventory => (PlayerInventory)base._Inventory;
    public PlayerInventory Inventory => _Inventory;

    public DynamicStat Health => _Stats.Health;
    public DynamicStat Energy => _Stats.Energy;
    public DynamicStat Magic => _Stats.Magic;
    private OverTimeEffect _OverTimeHealth;
    public OverTimeEffect OverTimeHealth => _OverTimeHealth;
    private OverTimeEffect _OverTimeEnergy;
    public OverTimeEffect OverTimeEnergy => _OverTimeEnergy;
    private OverTimeEffect _OverTimeMagic;
    public OverTimeEffect OverTimeMagic => _OverTimeMagic;

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();

        _Controller = GetComponent<PlayerController>();
        base._Inventory = GetComponent<PlayerInventory>();
        
        _Stats = GetComponent<CharacterStats>();
        _Stats.Setup(this);

        _PlayerVisual = GetComponentInChildren<PlayerVisual>();
        _PlayerVisual.Setup(_Controller);
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);

        _OverTimeHealth = new OverTimeEffect(this, _Stats.Health, SendHealthChangeEvent);
        _OverTimeEnergy = new OverTimeEffect(this, _Stats.Energy, SendEnergyChangeEvent);
        _OverTimeMagic = new OverTimeEffect(this, _Stats.Magic, SendMagicChangeEvent);
    }

    public void RestoreHealth(int health, int iterations, float time)
    {
        _OverTimeHealth.Add(health, iterations, time, isDecrement: false);
    }

    public void RestoreMagic(int magic, int iterations, float time)
    {
        _OverTimeMagic.Add(magic, iterations, time, isDecrement: false);
    }

    public void RestoreEnergy(int energy, int iterations, float time)
    {
        _OverTimeEnergy.Add(energy, iterations, time, isDecrement: false);
    }

    public override void TakeDamage(int damage, Character attacker, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _OverTimeHealth.Add(damage, 1, 0f, isDecrement: true);

        if (_Stats.Health.Current <= 0)
        {
            SendDeathEvent();
            Destroy(gameObject);
        }

        // Only gain experience on non-death damages
        _Stats.Vitality.IncreaseExperience(ExperienceCalculator.GetDamageTakenExperience(damage, _Stats.Luck));

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }

    public override void SpendEnergy(int energy)
    {
        if (!_Stats.Energy.CanSpend(energy))
        {
            throw new DynamicStatDepletedException(_Stats.Energy.Name);
        }

        _OverTimeEnergy.Add(energy, 1, 0f, isDecrement: true);
        _Stats.Stamina.IncreaseExperience(ExperienceCalculator.GetEnergyUsedExperience(energy, _Stats.Luck));

        if (_Stats.Energy.IsAtMin())
        {
            SendEnergyDepletedEvent();
        }
    }

    public override void SpendMagic(int magic)
    {
        if (!_Stats.Magic.CanSpend(magic))
        {
            throw new DynamicStatDepletedException(_Stats.Magic.Name);
        }

        _OverTimeMagic.Add(magic, 1, 0f, isDecrement: true);
        _Stats.Intelligence.IncreaseExperience(ExperienceCalculator.GetMeleeDamageDoneExperience(magic, _Stats.Luck));

        if (_Stats.Magic.IsAtMin())
        {
            SendMagicDepletedEvent();
        }
    }

    public void AddToInventory(ItemSO itemSO)
    {
        _Inventory.AddToInventory(itemSO);
    }

    // Used on enemy death, finishing a quest (levelCompare; for enemies = the enemy level, for quests = recommended level to do the quest)
    public void GetExperienceBoost(int baseExperience, int levelCompare)
    {
        int experienceGained = ExperienceCalculator.GetScaledExperienceBoost(levelCompare, _Stats.Level.Current, baseExperience);
        _Stats.Strength.IncreaseExperience(experienceGained);
        _Stats.Agility.IncreaseExperience(experienceGained);
        _Stats.Stamina.IncreaseExperience(experienceGained);
        _Stats.Vitality.IncreaseExperience(experienceGained);
        _Stats.Intelligence.IncreaseExperience(experienceGained);
        _Stats.Luck.IncreaseExperience(experienceGained);
    }

    public void GainExperienceOnMelee(MeleeWeaponSO meleeWeaponSO, int physicalDamage, int elementalDamage)
    {
        if (physicalDamage == 0 && elementalDamage == 0) return;

        if (physicalDamage > 0)
        {
            int physicalExperience = ExperienceCalculator.GetMeleeDamageDoneExperience(physicalDamage, _Stats.Luck);

            if (meleeWeaponSO.DamageType.Equals(DamageType.Pierce))
            {
                _Stats.Agility.IncreaseExperience(physicalExperience);
            }
            if (meleeWeaponSO.DamageType.Equals(DamageType.Blunt))
            {
                _Stats.Strength.IncreaseExperience(physicalExperience);
            }
            if (meleeWeaponSO.DamageType.Equals(DamageType.Explosive))
            {
                Stats.Luck.IncreaseExperience(physicalExperience);
            }
        }
        
        if (elementalDamage > 0)
        {
            int elementalExperience = ExperienceCalculator.GetElementalDamageDoneExperience(elementalDamage, _Stats.Luck);
            _Stats.Intelligence.IncreaseExperience(elementalExperience);
        }
    }

    public void GainExperienceOnRanged(ProjectileSO projectileSO, int physicalDamage, int elementalDamage)
    {
        if (physicalDamage == 0 && elementalDamage == 0) return;

        if (physicalDamage > 0)
        {
            int physicalExperience = ExperienceCalculator.GetRangedDamageDoneExperience(physicalDamage, _Stats.Luck);
            _Stats.Agility.IncreaseExperience((int)(physicalExperience * .5f)); // Half damage experience is for the bow

            // The other half experience is based on the projectile type
            if (projectileSO.DamageType.Equals(DamageType.Pierce))
            {
                _Stats.Agility.IncreaseExperience((int)(physicalExperience * .5f));
            }
            if (projectileSO.DamageType.Equals(DamageType.Blunt))
            {
                _Stats.Strength.IncreaseExperience((int)(physicalExperience * .5f));
            }
            if (projectileSO.DamageType.Equals(DamageType.Explosive))
            {
                Stats.Luck.IncreaseExperience((int)(physicalExperience * .5f));
            }
        }

        if (elementalDamage > 0)
        {
            int elementalExperience = ExperienceCalculator.GetElementalDamageDoneExperience(elementalDamage, _Stats.Luck);
            _Stats.Agility.IncreaseExperience((int)(elementalExperience * .5f));
            _Stats.Intelligence.IncreaseExperience((int)(elementalExperience * .5f));
        }
    }

    public void GainExperienceOnShieldBlock(int energyCost, int damageBlocked)
    {
        _Stats.Stamina.IncreaseExperience(ExperienceCalculator.GetEnergyUsedExperience(energyCost, _Stats.Luck));
        _Stats.Stamina.IncreaseExperience(ExperienceCalculator.GetDamageBlockedExperience(damageBlocked, _Stats.Luck));
    }
}

using UnityEngine;

public class Player : Character
{
    public CharacterType CharacterType = CharacterType.Player;

    private PlayerController _Controller;
    private PlayerVisual _PlayerVisual;
    private new PlayerInventory _Inventory => (PlayerInventory)base._Inventory;
    public PlayerInventory Inventory => _Inventory;

    public DynamicStat Health => _Stats.Health;
    public DynamicStat Energy => _Stats.Energy;
    public DynamicStat Magic => _Stats.Magic;
    private OverTimeEffect _OverTimeHealth;
    private OverTimeEffect _OverTimeEnergy;
    private OverTimeEffect _OverTimeMagic;

    private void Awake()
    {
        _Controller = GetComponent<PlayerController>();
        base._Inventory = GetComponent<PlayerInventory>();
        _Stats = GetComponent<CharacterStats>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        
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
        _Stats.Vitality.IncreaseExperience(ExperienceCalculator.GetDamageTakenExperience(damage));

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }

    public override void SpendEnergy(int energy)
    {
        if (_Stats.Energy.Current <= 0)
        {
            throw new DynamicStatAlreadyAtMaxException();
        }

        _OverTimeEnergy.Add(energy, 1, 0f, isDecrement: true);
        _Stats.Stamina.IncreaseExperience(ExperienceCalculator.GetStaminaActionExperience(energy));

        if (_Stats.Energy.Current <= 0)
        {
            SendEnergyDepletedEvent();
        }
    }

    public override void SpendMagic(int magic)
    {
        if (_Stats.Magic.Current <= 0)
        {
            throw new DynamicStatAlreadyAtMaxException();
        }

        _OverTimeMagic.Add(magic, 1, 0f, isDecrement: true);
        _Stats.Intelligence.IncreaseExperience(ExperienceCalculator.GetSpellCastExperience(magic));

        if (_Stats.Magic.Current <= 0)
        {
            SendMagicDepletedEvent();
        }
    }

    public void AddToInventory(ItemSO itemSO)
    {
        _Inventory.AddToInventory(itemSO);
    }

    // Used on enemy death, finishing a quest, etc
    //   levelCompare; for enemies = their level, for quests = recommended level, etc
    public void GetExperienceBoost(int baseExperience, int levelCompare)
    {
        int experienceGained = ExperienceCalculator.GetScaledExperienceBoost(levelCompare, _Stats.Level.CurrentLevel, baseExperience);
        _Stats.Strength.IncreaseExperience(experienceGained);
        _Stats.Agility.IncreaseExperience(experienceGained);
        _Stats.Stamina.IncreaseExperience(experienceGained);
        _Stats.Vitality.IncreaseExperience(experienceGained);
        _Stats.Intelligence.IncreaseExperience(experienceGained);
        // TODO need to put a "proc" function or whatever on this stat, and when your luck fires, it gets experience
        _Stats.Luck.IncreaseExperience(experienceGained);
    }
}

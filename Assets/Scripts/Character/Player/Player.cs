using UnityEngine;

public class Player : Character
{
    public CharacterType CharacterType = CharacterType.Player;

    private PlayerController _Controller;
    private PlayerVisual _PlayerVisual;
    private new PlayerInventory _Inventory => (PlayerInventory)base._Inventory;

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

        _OverTimeHealth = new OverTimeEffect(this, _Stats.Health, SendHealthChangeEvent, isDamage: false);
        _OverTimeEnergy = new OverTimeEffect(this, _Stats.Energy, SendEnergyChangeEvent, isDamage: false);
        _OverTimeMagic = new OverTimeEffect(this, _Stats.Magic, SendMagicChangeEvent, isDamage: false);
    }

    public void RestoreHealth(int health, int iterations, float time)
    {
        _OverTimeHealth.Add(health, iterations, time);
    }

    public void RestoreMagic(int magic, int iterations, float time)
    {
        _OverTimeMagic.Add(magic, iterations, time);
    }

    public void RestoreEnergy(int energy, int iterations, float time)
    {
        _OverTimeEnergy.Add(energy, iterations, time);
    }

    public override void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Stats.TakeDamage(damage);
        SendHealthChangeEvent();

        if (_Stats.Health.Current <= 0)
        {
            SendDeathEvent();
            Destroy(gameObject);
        }

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }

    public override void SpendEnergy(int energy)
    {
        _Stats.SpendEnergy(energy);
        SendEnergyChangeEvent();

        if (_Stats.Health.Current <= 0)
        {
            SendEnergyDepletedEvent();
        }
    }

    public override void SpendMagic(int magic)
    {
        _Stats.SpendMagic(magic);
        SendMagicChangeEvent();

        if (_Stats.Health.Current <= 0)
        {
            SendMagicDepletedEvent();
        }
    }

    public void AddToInventory(ItemSO itemSO)
    {
        _Inventory.AddToInventory(itemSO);
    }
}

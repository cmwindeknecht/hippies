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

    public override void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _OverTimeHealth.Add(damage, 1, 0f, isDecrement: true);

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
        _OverTimeEnergy.Add(energy, 1, 0f, isDecrement: true);

        if (_Stats.Energy.Current <= 0)
        {
            SendEnergyDepletedEvent();
        }
    }

    public override void SpendMagic(int magic)
    {
        _OverTimeMagic.Add(magic, 1, 0f, isDecrement: true);

        if (_Stats.Magic.Current <= 0)
        {
            SendMagicDepletedEvent();
        }
    }

    public void AddToInventory(ItemSO itemSO)
    {
        _Inventory.AddToInventory(itemSO);
    }
}

using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStat Level { get; private set; }

    public DynamicStat Health {  get; private set; }
    public DynamicStat Energy { get; private set; }
    public DynamicStat Magic { get; private set; }

    public CharacterStat Strength { get; private set; }
    public CharacterStat Agility { get; private set; }
    public CharacterStat Vitality { get; private set; }
    public CharacterStat Intelligence { get; private set; }
    public CharacterStat Luck { get; private set; }

    // TODO temp SerializeField because I don't have a proper character creation screen yet
    [SerializeField] private int _MaxHealth;
    [SerializeField] private int _CurrentHealth;
    [SerializeField] private float _MovementSpeed;
    public float MovementSpeed => _MovementSpeed;

    // TODO temp bullshit for the player
    private void Awake()
    {
        if (_CurrentHealth > 0)
        {
            Health = new DynamicStat(DynamicStatName.Health, _CurrentHealth, _MaxHealth);
            Energy = new DynamicStat(DynamicStatName.Energy, _CurrentHealth, _MaxHealth);
            Magic = new DynamicStat(DynamicStatName.Magic, 0, 0);
        }

        Strength = new CharacterStat(1);
        Agility = new CharacterStat(1);
        Vitality = new CharacterStat(1);
        Intelligence = new CharacterStat(1);
        Luck = new CharacterStat(1);
    }

    // For enemies - populate from SO
    public void Setup(EnemySO enemySO)
    {
        _MovementSpeed = enemySO.MovementSpeed;
        Health = new DynamicStat(DynamicStatName.Health, enemySO.Health, enemySO.Health);
        Energy = new DynamicStat(DynamicStatName.Energy, enemySO.Health, enemySO.Health);
        Magic = new DynamicStat(DynamicStatName.Magic, 0, 0);
    }

    //public void Setup(PlayerSaveData saveData)
    //{
    //    MaxHealth = saveData.maxHealth;
    //    CurrentHealth = saveData.currentHealth;
    //    MoveSpeed = saveData.moveSpeed;
    //}

    public void TakeDamage(int damage)
    {
        try
        {
            Health.Decrease(damage);
        }
        catch (DynamicStaDepletedException exception)
        {
            // TODO play the relevant animation (death or something in the UI or whatever)
        }
    }

    public void RestoreHealth(int health)
    {
        Health.Increase(health);
    }
}
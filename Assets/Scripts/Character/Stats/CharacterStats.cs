using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public DynamicStat Health {  get; private set; }
    public DynamicStat Energy { get; private set; }
    public DynamicStat Magic { get; private set; }

    public CharacterStat Level { get; private set; }
    public CharacterStat Strength { get; private set; }
    public CharacterStat Agility { get; private set; }
    public CharacterStat Vitality { get; private set; }
    public CharacterStat Stamina { get; private set; }
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
        Health = new DynamicStat(DynamicStatName.Health, _CurrentHealth, _MaxHealth);
        Energy = new DynamicStat(DynamicStatName.Energy, _CurrentHealth, _MaxHealth);
        Magic = new DynamicStat(DynamicStatName.Magic, 10, 10);

        Level = new CharacterStat(CharacterStatName.Level, 1,0);

        Strength = new CharacterStat(CharacterStatName.Strength, 1, 0);
        Strength.OnStatLevelUp += OnStatLevelUp;

        Agility = new CharacterStat(CharacterStatName.Agility, 1, 0);
        Agility.OnStatLevelUp += OnStatLevelUp;

        Vitality = new CharacterStat(CharacterStatName.Vitality, 1, 0);
        Vitality.OnStatLevelUp += OnStatLevelUp;

        Stamina = new CharacterStat(CharacterStatName.Stamina, 1, 0);
        Stamina.OnStatLevelUp += OnStatLevelUp;

        Intelligence = new CharacterStat(CharacterStatName.Intelligence, 1, 0);
        Intelligence.OnStatLevelUp += OnStatLevelUp;

        Luck = new CharacterStat(CharacterStatName.Luck, 1, 0);
        Luck.OnStatLevelUp += OnStatLevelUp;
    }

    // TODO split this shit out into another class
    // For enemies - populate from SO
    public void Setup(EnemySO enemySO)
    {
        _MovementSpeed = enemySO.MovementSpeed;

        Health = new DynamicStat(DynamicStatName.Health, enemySO.Health, enemySO.Health);
        Energy = new DynamicStat(DynamicStatName.Energy, enemySO.Health, enemySO.Health);
        Magic = new DynamicStat(DynamicStatName.Magic, 0, 0);

        Level = new CharacterStat(CharacterStatName.Level, enemySO.Level, 0);
        Strength = new CharacterStat(CharacterStatName.Strength, enemySO.Strength, 0);
        Agility = new CharacterStat(CharacterStatName.Agility, enemySO.Agility, 0);
        Vitality = new CharacterStat(CharacterStatName.Vitality, enemySO.Vitality, 0);
        Stamina = new CharacterStat(CharacterStatName.Stamina, enemySO.Stamina, 0);
        Intelligence = new CharacterStat(CharacterStatName.Intelligence, enemySO.Intelligence, 0);
        Luck = new CharacterStat(CharacterStatName.Luck, enemySO.Luck, 0);
    }

    private void OnStatLevelUp(object sender, int e)
    {
        Level.CalculateCurrentLevel(new List<int> { Strength.CurrentLevel, Agility.CurrentLevel, Vitality.CurrentLevel, Stamina.CurrentLevel, Intelligence.CurrentLevel, Luck.CurrentLevel });
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
        catch (DynamicStatDepletedException exception)
        {
            Debug.Log($"DynamicStatDepletedException after TakeDamage - {exception}");
            // TODO play the relevant animation (death or something in the UI or whatever)
        }
    }

    public void SpendEnergy(int energy)
    {
        try
        {
            Energy.Decrease(energy);
        }
        catch (DynamicStatDepletedException exception)
        {
            Debug.Log($"DynamicStatDepletedException after SpendEnergy - {exception}");
            // TODO play the relevant animation (death or something in the UI or whatever)
        }
    }


    public void SpendMagic(int magic)
    {
        try
        {
            Magic.Decrease(magic);
        }
        catch (DynamicStatDepletedException exception)
        {
            Debug.Log($"DynamicStatDepletedException after SpendMagic - {exception}");
            // TODO play the relevant animation (death or something in the UI or whatever)
        }
    }

    public void RestoreHealth(int health)
    {
        Health.Increase(health);
    }

    public void RestoreEnergy(int energy)
    {
        Energy.Increase(energy);
    }

    public void RestoreMagic(int magic)
    {
        Magic.Increase(magic);
    }
}
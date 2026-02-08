using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Level Level { get; private set; }

    public DynamicStat Health {  get; private set; }
    public DynamicStat Energy { get; private set; }
    public DynamicStat Magic { get; private set; }

    public CharacterStat Strength { get; private set; }
    public CharacterStat Agility { get; private set; }
    public CharacterStat Vitality { get; private set; }
    public CharacterStat Stamina { get; private set; }
    public CharacterStat Intelligence { get; private set; }
    public CharacterStat Spirit { get; private set; }
    public CharacterStat Luck { get; private set; }

    // TODO temp SerializeField because I don't have a proper character creation screen yet
    [SerializeField] private int _MaxHealth;
    [SerializeField] private int _CurrentHealth;
    private float _MovementSpeed;
    public float MovementSpeed => _MovementSpeed;

    public void Setup(Player player)
    {
        _MovementSpeed = 6f;

        Strength = new CharacterStat(CharacterStatName.Strength, 1, 0);
        Agility = new CharacterStat(CharacterStatName.Agility, 1, 0);
        Vitality = new CharacterStat(CharacterStatName.Vitality, 1, 0);
        Stamina = new CharacterStat(CharacterStatName.Stamina, 1, 0);
        Intelligence = new CharacterStat(CharacterStatName.Intelligence, 1, 0);
        Spirit = new CharacterStat(CharacterStatName.Spirit, 1, 0);
        Luck = new CharacterStat(CharacterStatName.Luck, 1, 0);

        Level = new Level(_CurrentHealth, Strength, Agility, Vitality, Stamina, Intelligence, Luck);

        CancellationToken token = this.GetCancellationTokenOnDestroy();
        Health = new DynamicStat(DynamicStatName.Health, _CurrentHealth, _MaxHealth, 0, Level, token, player);
        Energy = new DynamicStat(DynamicStatName.Energy, 100, 100, .001f, Level, token, player);
        Magic = new DynamicStat(DynamicStatName.Magic, 10, 10, .001f, Level, token, player);
    }

    // TODO split this shit out into another class
    // For enemies - populate from SO
    public void Setup(Enemy enemy, EnemySO enemySO)
    {
        _MovementSpeed = enemySO.MovementSpeed;

        Strength = new CharacterStat(CharacterStatName.Strength, enemySO.Strength, 0);
        Agility = new CharacterStat(CharacterStatName.Agility, enemySO.Agility, 0);
        Vitality = new CharacterStat(CharacterStatName.Vitality, enemySO.Vitality, 0);
        Stamina = new CharacterStat(CharacterStatName.Stamina, enemySO.Stamina, 0);
        Intelligence = new CharacterStat(CharacterStatName.Intelligence, enemySO.Intelligence, 0);
        Luck = new CharacterStat(CharacterStatName.Luck, enemySO.Luck, 0);
        Spirit = new CharacterStat(CharacterStatName.Spirit, enemySO.Spirit, 0);

        Level = new Level(1, Strength, Agility, Vitality, Stamina, Intelligence, Luck);

        CancellationToken token = this.GetCancellationTokenOnDestroy();
        Health = new DynamicStat(DynamicStatName.Health, enemySO.Health, enemySO.Health, enemySO.HealthRegenerationRate, Level, token, enemy);
        Energy = new DynamicStat(DynamicStatName.Energy, enemySO.Energy, enemySO.Energy, enemySO.EnergyRegenerationRate, Level, token, enemy);
        Magic = new DynamicStat(DynamicStatName.Magic, enemySO.Magic, enemySO.Magic, enemySO.MagicRegenerationRate, Level, token, enemy);
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
            Health.DecreaseCurrent(damage);
        }
        catch (DynamicStatDepletedException exception)
        {
            // TODO handle this differently based on player / enemy (and split this fuckin file for each) --- enemy is expected to die, player is not / has UI and game play effects
            Debug.Log($"DynamicStatDepletedException after TakeDamage - {exception}");
            // TODO play the relevant animation (death or something in the UI or whatever)
        }
    }

    public void SpendEnergy(int energy)
    {
        try
        {
            Energy.DecreaseCurrent(energy);
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
            Magic.DecreaseCurrent(magic);
        }
        catch (DynamicStatDepletedException exception)
        {
            Debug.Log($"DynamicStatDepletedException after SpendMagic - {exception}");
            // TODO play the relevant animation (death or something in the UI or whatever)
        }
    }

    public void RestoreHealth(int health)
    {
        Health.IncreaseCurrent(health);
    }

    public void RestoreEnergy(int energy)
    {
        Energy.IncreaseCurrent(energy);
    }

    public void RestoreMagic(int magic)
    {
        Magic.IncreaseCurrent(magic);
    }
}
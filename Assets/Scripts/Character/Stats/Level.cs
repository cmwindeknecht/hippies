using System;
using System.Collections.Generic;
using System.Linq;

public class Level
{
    public event EventHandler<int> OnPlayerLevelUp;

    private int _Current;
    public int Current => _Current;

    private readonly CharacterStat _Strength;
    private readonly CharacterStat _Agility;
    private readonly CharacterStat _Vitality;
    private readonly CharacterStat _Stamina;
    private readonly CharacterStat _Intelligence;
    private readonly CharacterStat _Luck;

    public Level(int currentLevel, CharacterStat strength, CharacterStat agility, CharacterStat vitality, CharacterStat stamina, CharacterStat intelligence, CharacterStat luck)
    {
        _Current = currentLevel;
        _Strength = strength;
        _Agility = agility;
        _Vitality = vitality;
        _Stamina = stamina;
        _Intelligence = intelligence;
        _Luck = luck;

        _Strength.OnStatLevelUp += OnStatLevelUp;
        _Agility.OnStatLevelUp += OnStatLevelUp;
        _Vitality.OnStatLevelUp += OnStatLevelUp;
        _Stamina.OnStatLevelUp += OnStatLevelUp;
        _Intelligence.OnStatLevelUp += OnStatLevelUp;
        _Luck.OnStatLevelUp += OnStatLevelUp;
    }

    private void OnStatLevelUp(object sender, int e)
    {
        CalculateCurrentLevel(new List<int> { _Strength.CurrentLevel, _Agility.CurrentLevel, _Vitality.CurrentLevel, _Stamina.CurrentLevel, _Intelligence.CurrentLevel, _Luck.CurrentLevel });
    }

    public void CalculateCurrentLevel(List<int> statLevels)
    {
        int newLevel = (int)statLevels.Average();
        if (newLevel > _Current)
        {
            _Current = newLevel;
            OnPlayerLevelUp?.Invoke(this, _Current);
        }
    }
}

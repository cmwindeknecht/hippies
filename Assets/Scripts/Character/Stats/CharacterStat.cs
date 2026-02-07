using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterStatName
{
    Strength,
    Agility,
    Vitality,
    Stamina,
    Intelligence,
    Luck,
    Level
}

public class CharacterStat
{
    public event EventHandler<int> OnStatLevelUp;
    public event EventHandler<int> OnPlayerLevelUp;

    private readonly CharacterStatName _Name;
    public CharacterStatName Name => _Name;

    private int _CurrentLevel;
    public int CurrentLevel => _CurrentLevel;

    private int _CurrentExperience;
    public int CurrentExperience => _CurrentExperience;

    private int _RequiredExperience;
    public int RequiredExperience => _RequiredExperience;

    private const int _MinLevel = 1;
    private const int _LevelRange = 10;
    private const float _MinWeight = -.25f; 
    private const float _MaxWeight = .25f; 

    public CharacterStat(CharacterStatName name, int currentLevel, int currentExperience)
    {
        _Name = name;
        _CurrentLevel = currentLevel;
        _CurrentExperience = currentExperience;
        _RequiredExperience = ExperienceCalculator.GetRequiredExperienceByLevel(_CurrentLevel);
    }

    // TODO should really make another class like LevelStat because this literally only applies to the Level CharacterStat
    public void CalculateCurrentLevel(List<int> statLevels)
    {
        int newLevel = (int)statLevels.Average();
        if (newLevel > _CurrentLevel)
        {
            _CurrentLevel = newLevel;
            OnPlayerLevelUp?.Invoke(this, _CurrentLevel);
        }
    }

    public void IncreaseExperience(int amountToIncrease)
    {
        _CurrentExperience += amountToIncrease;

        if (_CurrentExperience > _RequiredExperience) {
            _CurrentExperience = _CurrentExperience - _RequiredExperience;
            _CurrentLevel++;    
            _RequiredExperience = ExperienceCalculator.GetRequiredExperienceByLevel(_CurrentLevel);
            // TODO
            //  1. Show popup in UI
            //  2. Consume on Level CharacterStat to recalculate average of all stats / determine the level of the player
            OnStatLevelUp?.Invoke(this, _CurrentLevel);
        }

        Debug.Log($"Increased Experience for stat {Name} by {amountToIncrease}");
    }

    // Get the weight the stat has on an effect (taking damage, giving damage, magic resistance, etc)
    // TODO don't use this yet --- unsure how to really do this at the momment.  Also think this should maybe be on the inventory items like armor/weapons and the stat is the input?
    public float GetWeight(int requirement = 1)
    {
        float minPossibleLevel = Mathf.Max(requirement - _LevelRange, _MinLevel);
        float maxPossibleLevel = requirement + _LevelRange;

        float clampedCurrent = Mathf.Clamp(_CurrentLevel, minPossibleLevel, maxPossibleLevel);
        float toInterpolate = (clampedCurrent - minPossibleLevel) / (maxPossibleLevel - minPossibleLevel);

        // Provide a weight between -.2 and .2 depending on how the player's stats relate to weapon/armor/magic requirement
        return Mathf.Lerp(_MinWeight, _MaxWeight, toInterpolate);
    }
} 

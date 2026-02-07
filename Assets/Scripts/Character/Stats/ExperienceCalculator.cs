using UnityEngine;

public class ExperienceCalculator
{
    // Adjust these based on playtesting:
    //  1. Leveling too slow?  Lower the _LevelCurve (1.3f) or _LevelRequirementMultiplier (30)
    //  2. Leveling too fast?  Raise the _LevelCurve (1.7f) or _LevelRequirementMultiplier (70)

    // Linear = 1f (level 5 needs 5x more than level 1)
    // Quadratic = 2f (level 5 needs 25x more than level 1)
    private static readonly float _LevelCurve = 1.5f;
    private static readonly int _LevelRequirementMultiplier = 50;

    // Every action has a base experience gain of 5 and it gets modified in GetExperienceGained
    public const int _ExperiencePerAction = 3;
    private const float SPELL_XP_PER_MANA = 0.5f;
    private const float DAMAGE_XP_MULTIPLIER = 1.0f;
    private const float DAMAGE_TAKEN_MULTIPLIER = 0.8f;
    private const float STAMINA_XP_PER_POINT = 0.3f;

    // On level up (and instantiation) get the amount of XP required to increase a stat
    public static int GetRequiredExperienceByLevel(int level)
    {
        return (int)(Mathf.Pow(level, _LevelCurve) * _LevelRequirementMultiplier);
    }

    // TODO need to steal how WoW makes the enemy placard is grey/white/has a skull/etc so players know if its worthwhile to attack
    // Experience gained scales based on how over/under leveled the enemy is vs the player
    public static int GetScaledExperienceBoost(int enemyLevel, int playerLevel, int baseXP)
    {
        int levelDifference = enemyLevel - playerLevel;
        float modifier = 1f + (levelDifference * 0.1f);

        // Enemy is so low level comparitively that the player gets no experience
        if (levelDifference < -10) return 0;
        // Experience reduced by 10% of level difference below 10 level difference
        else if (levelDifference < 0) return Mathf.Max(1, (int)(baseXP * modifier));
        // Same level, return baseXP
        else if (levelDifference == 0) return baseXP;
        // Experience increased by 10% for every level above the character
        else return (int)(baseXP * modifier);
    }


    public static int GetSpellCastExperience(int manaCost)
    {
        return (int)(manaCost * SPELL_XP_PER_MANA);
    }

    public static int GetDamageDoneExperience(float damageDealt)
    {
        // Scale by damage and optionally weapon tier
        return (int)(damageDealt * DAMAGE_XP_MULTIPLIER);
    }

    public static int GetDamageTakenExperience(float damageTaken)
    {
        return (int)(damageTaken * DAMAGE_TAKEN_MULTIPLIER);
    }

    public static int GetStaminaActionExperience(int staminaCost)
    {
        return (int)(staminaCost * STAMINA_XP_PER_POINT);
    }
}

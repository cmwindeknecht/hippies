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

    private const float SPELL_COST_XP_MULTIPLIER = 0.5f;
    private const float SPELL_DAMAGE_XP_MULTIPLIER = 0.5f;

    private const float STAMINA_COST_XP_MULTIPLIER = 0.05f;

    private const float MELEE_DAMAGE_XP_MULTIPLIER = 0.75f;
    private const float RANGED_DAMAGE_XP_MULTIPLIER = 0.75f;
    private const float ELEMENTAL_DAMAGE_XP_MULTIPLIER = .1f;
    
    private const float DAMAGE_TAKEN_MULTIPLIER = 0.25f;
    private const float DAMAGE_BLOCKED_MULTIPLIER = 0.1f;

    private const float LUCK_AUTO_MULITPLIER = .01f;
    private const float LUCK_PROCCED_MULITPLIER = 1.5f;
    
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

    // TODO if these do the same thing by the time I get to release --- make a single function that takes in the multiplier as an argument
    // Exp for Strength and/or Agility (physical damage)
    public static int GetMeleeDamageDoneExperience(float damageDealt, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(damageDealt * MELEE_DAMAGE_XP_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Exp for Agility (physical damage)
    public static int GetRangedDamageDoneExperience(float damageDealt, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(damageDealt * RANGED_DAMAGE_XP_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Intelligence (magic damage)
    public static int GetSpellDamageDoneExperience(float damageDealt, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(damageDealt * SPELL_DAMAGE_XP_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Intelligence (elemental damage from physical type attack (melee/ranged))
    public static int GetElementalDamageDoneExperience(float damageDealt, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(damageDealt * ELEMENTAL_DAMAGE_XP_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Exp for Vitality (physical damage) or Intelligence (magic damage)
    public static int GetDamageTakenExperience(float damageTaken, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(damageTaken * DAMAGE_TAKEN_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Exp for Stamina on Shield use
    public static int GetDamageBlockedExperience(float damageTaken, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(damageTaken * DAMAGE_BLOCKED_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Intelligence (mana spent)
    public static int GetSpellCastExperience(int manaCost, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(manaCost * SPELL_COST_XP_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Stamina (stamina spent) --- e.g. Shielding, sprinting, sneaking
    public static int GetEnergyUsedExperience(int staminaCost, CharacterStat luckStat)
    {
        int experience = Mathf.Max(1, (int)(staminaCost * STAMINA_COST_XP_MULTIPLIER));
        luckStat.IncreaseExperience(LuckAutoExperience(experience));
        return experience;
    }

    // Whenever lock procs / triggers, it is increased by the amount of experience assoicated with that action cost
    //      Critical hits = damage experience, critical blocks = damage blocked, etc
    public static int LuckProccedExperience(int experience)
    {
        return Mathf.Max(1, (int)(experience * LUCK_PROCCED_MULITPLIER));
    }

    // Should be called after every experience gain
    private static int LuckAutoExperience(int experience)
    {
        return Mathf.Max(1, (int)(experience * LUCK_AUTO_MULITPLIER));
    }
}

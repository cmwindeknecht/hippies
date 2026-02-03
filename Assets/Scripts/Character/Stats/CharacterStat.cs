using UnityEngine;

public enum CharacterStatName
{
    Strength,
    Agility,
    Vitality,
    Intell
}

public class CharacterStat
{
    private int _Current;
    public int Current => _Current;

    private const int _MinLevel = 1;
    private const int _LevelRange = 10;
    private const float _MinWeight = -.25f; 
    private const float _MaxWeight = .25f; 

    public CharacterStat(int current)
    {
        _Current = current;
    }

    public void Setup(int current)
    {
        _Current = current;
    }

    public void Increase(int statIncrease)
    {
        _Current += statIncrease;
    }

    // Get the weight the stat has on an effect (taking damage, giving damage, magic resistance, etc)
    // TODO don't use this yet --- unsure how to really do this at the momment.  Also think this should maybe be on the inventory items like armor/weapons and the stat is the input?
    public float GetWeight(int requirement = 1)
    {
        float minPossibleLevel = Mathf.Max(requirement - _LevelRange, _MinLevel);
        float maxPossibleLevel = requirement + _LevelRange;

        float clampedCurrent = Mathf.Clamp(_Current, minPossibleLevel, maxPossibleLevel);
        float toInterpolate = (clampedCurrent - minPossibleLevel) / (maxPossibleLevel - minPossibleLevel);

        // Provide a weight between -.2 and .2 depending on how the player's stats relate to weapon/armor/magic requirement
        return Mathf.Lerp(_MinWeight, _MaxWeight, toInterpolate);
    }
} 

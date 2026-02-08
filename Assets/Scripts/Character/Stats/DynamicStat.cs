using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public enum DynamicStatName
{
    Health,
    Energy,
    Magic
}

public class DynamicStat
{
    private readonly Level _Level;
    private readonly DynamicStatName _Name;
    public DynamicStatName Name => _Name;
    private int _Current;
    public int Current => _Current;
    private int _Max;
    public int Max => _Max;

    // Regeneration rate of .1 is pretty crazy... so this should be the max you can ever achieve.
    private float _RegenerationRateMaximum = .1f;
    private float _RegenerationRate; // Can be upgraded so I need a function for that when I do upgrades
    private int _RegenerationDelay = 5; // Can be upgraded so I need a function for that when I do upgrades
    private int _RegenerationTimer = 0;

    private const int _Min = 0;

    private Character _Owner;

    public DynamicStat(DynamicStatName name, int current, int max, float regenerationRate, Level playerLevel, CancellationToken cancellationToken, Character Owner)
    {
        _Name = name;
        _Current = current;
        _Max = max;
        _Level = playerLevel;
        _RegenerationRate = Mathf.Min(_RegenerationRateMaximum, regenerationRate);
        _Owner = Owner;

        _Level.OnPlayerLevelUp += _PlayerLevel_OnPlayerLevelUp;

        if (_RegenerationRate > 0)
        {
            Regenerate(cancellationToken).Forget();
        }
    }

    private async UniTaskVoid Regenerate(CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                await UniTask.WaitForSeconds(1f, cancellationToken: cancellationToken);
                
                if (_Current.Equals(_Max))
                {
                    _RegenerationTimer = 0;
                    continue;
                }

                _RegenerationTimer++;

                if (_RegenerationTimer >= _RegenerationDelay)
                {
                    _RegenerationTimer = 0;

                    if (!_Current.Equals(_Max))
                    {
                        float calculation = _Max * _RegenerationRate;
                        int result = (int)calculation;
                        if (_Owner is Player && _Name.Equals(DynamicStatName.Energy))
                        {
                            Debug.Log($"{Name} Regeneration: Calculation: {_Max} * {_RegenerationRate} = {calculation}, cast to int = {result}");
                        }
                        IncreaseCurrent(result);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"The fuck - regeneration died due to exception=[{e}]");
        }
    }

    private void _PlayerLevel_OnPlayerLevelUp(object sender, int e)
    {
        // TODO increase max, regeneration rate, etc based on level
    }

    public void IncreaseCurrent(int toIncrease)
    {
        _Current = Mathf.Min(_Current + toIncrease, _Max);
        // Absolut dogshit --- I need to clean up literally everything, this is so dumb
        if (_Name.Equals(DynamicStatName.Health)) _Owner.SendHealthChangeEvent();
        else if (_Name.Equals(DynamicStatName.Energy)) _Owner.SendEnergyChangeEvent();
        else if (_Name.Equals(DynamicStatName.Magic)) _Owner.SendMagicChangeEvent();
    }

    public void DecreaseCurrent(int toDecrease)
    {
        _Current = Mathf.Max(_Current - toDecrease, _Min);
        if (_Name.Equals(DynamicStatName.Health)) _Owner.SendHealthChangeEvent();
        else if (_Name.Equals(DynamicStatName.Energy)) _Owner.SendEnergyChangeEvent();
        else if (_Name.Equals(DynamicStatName.Magic)) _Owner.SendMagicChangeEvent();
    }

    public bool IsAtMax()
    {
        return _Current.Equals(_Max);
    }

    public bool IsAtMin()
    {
        return _Current.Equals(_Min);
    }

    public bool CanSpend(int amountToDecrement)
    {
        int projectedAmount = _Current - amountToDecrement;
        return projectedAmount >= 0;
    }
}

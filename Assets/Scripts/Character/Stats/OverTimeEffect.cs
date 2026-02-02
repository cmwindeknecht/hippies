using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class OverTimeEffect
{
    private int _RemainingAmount = 0;
    private int _RemainingIterations = 0;
    private float _RemainingTime = 0;

    private CancellationTokenSource _CancellationToken;
    private readonly DynamicStat _Stat;
    private readonly Action<int> _SendChangeEvent;
    private readonly bool _IsDamage;
    private readonly Character _Owner;

    // If the effect is cumulative, then it extends the time/ticks 
    public OverTimeEffect(Character owner, DynamicStat stat, Action<int> sendChangeEvent, bool isDamage)
    {
        _Stat = stat;
        _SendChangeEvent = sendChangeEvent;
        _IsDamage = isDamage;
        _Owner = owner;
    }

    // If the effect is cumulative, then it extends the time/ticks of the effect
    // If its not, then it just increases the remaining amount
    public void Add(int amount, int iterations, float time)
    {
        if (iterations > 1)
        {
            _CancellationToken?.Cancel();
            _CancellationToken = new CancellationTokenSource();

            _RemainingAmount += amount;
            _RemainingIterations += iterations;
            _RemainingTime += time;

            int projectedValue = GetProjectedAmount();

            _SendChangeEvent(projectedValue);
            ApplyOverTime(_CancellationToken.Token).Forget();
        }
        else
        {
            PerformEffect(amount);
            int projectedValue = GetProjectedAmount();
            _SendChangeEvent(projectedValue);
        }
    }

    private async UniTaskVoid ApplyOverTime(CancellationToken cancellationToken)
    {
        float delayBetweenTicks = _RemainingTime / _RemainingIterations;

        while (_RemainingAmount > 0 && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(delayBetweenTicks, cancellationToken: cancellationToken);

            if (_Owner == null) return;

            int tickAmount = Mathf.CeilToInt((float) _RemainingAmount / _RemainingIterations);
            tickAmount = Mathf.Min(tickAmount, _RemainingAmount);

            _RemainingAmount -= tickAmount;
            PerformEffect(tickAmount);

            int projectedValue = GetProjectedAmount();
            _SendChangeEvent(projectedValue);

            // Stop conditions
            if (ShouldStop())
            {
                _RemainingAmount = 0;
                break;
            }
        }
    }

    private bool ShouldStop()
    {
        return (_IsDamage && _Stat.Current <= 0) || (!_IsDamage && _Stat.Current >= _Stat.Max);
    }

    private int GetProjectedAmount()
    {
        if (_IsDamage)
        {
            return Mathf.Max(_Stat.Current - _RemainingAmount, 0);
        }
        else
        {
            return Mathf.Min(_Stat.Current + _RemainingAmount, _Stat.Max);
        }
    }

    private void PerformEffect(int amount)
    {
        if (_IsDamage)
        {
            _Stat.Decrease(amount);
        }
        else
        {
            _Stat.Increase(amount);
        }   
    }

    // Use to stop the over time effect, e.g. if you are poisoned and drink and antidote 
    public void Stop()
    {
        _CancellationToken?.Cancel();
        _RemainingAmount = 0;
    }
}
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class OverTimeEffect
{
    private int _RemainingAmount = 0;
    private int _RemainingIterations = 0;
    private CancellationTokenSource _CancellationToken;
    private readonly DynamicStat _Stat;
    private readonly Action<int> _SendChangeEvent;
    private readonly Character _Owner;

    public OverTimeEffect(Character owner, DynamicStat stat, Action<int> sendChangeEvent)
    {
        _Stat = stat;
        _SendChangeEvent = sendChangeEvent;
        _Owner = owner;
    }

    public void Add(int amount, int iterations, float time, bool isDecrement)
    {
        if (iterations > 1)
        {
            _CancellationToken?.Cancel();
            _CancellationToken = new CancellationTokenSource();

            _RemainingAmount += amount;
            _RemainingIterations += iterations;

            int projectedValue = GetProjectedAmount(isDecrement);
            _SendChangeEvent(projectedValue);

            float delayBetweenTicks = time / iterations;
            ApplyOverTime(delayBetweenTicks, isDecrement, _CancellationToken.Token).Forget();
        }
        else
        {
            PerformEffect(amount, isDecrement);
            int projectedValue = GetProjectedAmount(isDecrement);
            _SendChangeEvent(projectedValue);
        }
    }

    private async UniTaskVoid ApplyOverTime(float delayBetweenTicks, bool isDamage, CancellationToken cancellationToken)
    {
        while (_RemainingAmount > 0 && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(delayBetweenTicks, cancellationToken: cancellationToken);

            if (_Owner == null) return;

            int tickAmount = Mathf.CeilToInt((float)_RemainingAmount / _RemainingIterations);
            tickAmount = Mathf.Min(tickAmount, _RemainingAmount);

            _RemainingAmount -= tickAmount;
            _RemainingIterations--;

            PerformEffect(tickAmount, isDamage);

            int projectedValue = GetProjectedAmount(isDamage);
            _SendChangeEvent(projectedValue);

            if (ShouldStop(isDamage))
            {
                _RemainingAmount = 0;
                _RemainingIterations = 0;
                break;
            }
        }
    }

    private bool ShouldStop(bool isDamage)
    {
        return (isDamage && _Stat.Current <= 0) || (!isDamage && _Stat.Current >= _Stat.Max);
    }

    private int GetProjectedAmount(bool isDecrement)
    {
        return _Stat.Current + _RemainingAmount;
        //if (isDecrement)
        //{
        //    return Mathf.Max(_Stat.Current - _RemainingAmount, 0);
        //}
        //else
        //{
        //    return Mathf.Min(_Stat.Current + _RemainingAmount, _Stat.Max);
        //}
    }

    private void PerformEffect(int amount, bool isDecrement)
    {
        if (isDecrement)
        {
            _Stat.Decrease(amount);
        }
        else
        {
            _Stat.Increase(amount);
        }
    }

    public void Stop()
    {
        _CancellationToken?.Cancel();
        _RemainingAmount = 0;
        _RemainingIterations = 0;
    }
}
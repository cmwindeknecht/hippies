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

    // Current Behavior --- Hitting max health removes over time effect
    //   Possible behavior instead --- Keep healing regardless afterwards.  Just remove the CanPerformEffect and ShouldStop (and handle the exception)
    public void Add(int amount, int iterations, float time, bool isDecrement)
    {
        //if (!CanPerformEffect(isDecrement))
        //{
        //    if (isDecrement) throw new DynamicStatDepletedException(_Stat.Name);
        //    else throw new DynamicStatAtMaxException(_Stat.Name);
        //}

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

    private async UniTaskVoid ApplyOverTime(float delayBetweenTicks, bool isDecrement, CancellationToken cancellationToken)
    {
        while (_RemainingAmount > 0 && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(delayBetweenTicks, cancellationToken: cancellationToken);

            if (ShouldStop(isDecrement))
            {
                _RemainingAmount = 0;
                _RemainingIterations = 0;
                _SendChangeEvent(0);
                break;
            }

            if (_Owner == null) return;

            int tickAmount = Mathf.CeilToInt((float)_RemainingAmount / _RemainingIterations);
            tickAmount = Mathf.Min(tickAmount, _RemainingAmount);

            _RemainingAmount -= tickAmount;
            _RemainingIterations--;

            PerformEffect(tickAmount, isDecrement);

            int projectedValue = GetProjectedAmount(isDecrement);
            _SendChangeEvent(projectedValue);
        }
    }

    private bool ShouldStop(bool isDecrement)
    {
        return CanPerformEffect(isDecrement);
        //return (isDecrement && _Stat.Current <= 0) || (!isDecrement && _Stat.Current >= _Stat.Max);
    }

    private int GetProjectedAmount(bool isDecrement)
    {
        if (ShouldStop(isDecrement))
        {
            _RemainingAmount = 0;
            _RemainingIterations = 0;
            return _Stat.Current;
        }
        return _Stat.Current + _RemainingAmount;
        // Might be necessary for decrementing, unsure for now
        //if (isDecrement)
        //{
        //    return Mathf.Max(_Stat.Current - _RemainingAmount, 0);
        //}
        //else
        //{
        //    return Mathf.Min(_Stat.Current + _RemainingAmount, _Stat.Max);
        //}
    }

    private bool CanPerformEffect(bool isDecrement)
    {
        if (isDecrement)
        {
            return _Stat.IsAtMin();
        }
        else
        {
            return _Stat.IsAtMax();
        }
    }

    private void PerformEffect(int amount, bool isDecrement)
    {
        if (isDecrement)
        {
            _Stat.DecreaseCurrent(amount);
        }
        else
        {
            _Stat.IncreaseCurrent(amount);
        }
    }

    public void Stop()
    {
        _CancellationToken?.Cancel();
        _RemainingAmount = 0;
        _RemainingIterations = 0;
    }
}
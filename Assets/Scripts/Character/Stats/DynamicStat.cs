using UnityEngine;

public enum DynamicStatName
{
    Health,
    Energy,
    Magic,
    Intelligence,
    Luck
}

public class DynamicStat
{
    private DynamicStatName _Name;
    public DynamicStatName Name => _Name;
    private int _Current;
    public int Current => _Current;
    private int _Max;
    public int Max => _Max;

    private const int _Min = 0;

    public DynamicStat(DynamicStatName name, int current, int max)
    {
        _Name = name;
        _Current = current;
        _Max = max;
    }

    public void Increase(int toIncrease)
    {
        if (_Current.Equals(_Max))
        {
            throw new DynamicStatAlreadyAtMaxException();
        }  

        _Current = Mathf.Min(_Current + toIncrease, _Max);
    }

    public void Decrease(int toDecrease)
    {
        _Current = Mathf.Max(_Current - toDecrease, _Min);

        if (_Current.Equals(_Min))
        {
            throw new DynamicStaDepletedException();
        }
    }
}

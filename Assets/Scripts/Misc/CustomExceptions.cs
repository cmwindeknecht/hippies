using System;

public class DynamicStatAlreadyAtMaxException : Exception
{
    public DynamicStatAlreadyAtMaxException() : base() { }
}

public class DynamicStaDepletedException : Exception
{
    public DynamicStaDepletedException() : base() { }
}

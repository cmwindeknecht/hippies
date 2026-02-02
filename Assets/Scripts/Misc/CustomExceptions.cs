using System;

public class DynamicStatAlreadyAtMaxException : Exception
{
    public DynamicStatAlreadyAtMaxException() : base() { }
}

public class DynamicStatDepletedException : Exception
{
    public DynamicStatDepletedException() : base() { }
}

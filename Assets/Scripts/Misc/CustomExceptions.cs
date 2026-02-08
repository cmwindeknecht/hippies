using System;

public class DynamicStatAtMaxException : Exception
{
    public DynamicStatName Name {  get; private set; }
    public DynamicStatAtMaxException(DynamicStatName name) : base() 
    { 
        Name = name;
    }
}

public class DynamicStatDepletedException : Exception
{
    public DynamicStatName Name { get; private set; }
    public DynamicStatDepletedException(DynamicStatName name) : base()
    {
        Name = name;
    }
}

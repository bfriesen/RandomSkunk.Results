namespace RandomSkunk.Results;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Method)]
public class TryCatchAttribute : Attribute
{
    public TryCatchAttribute()
    {
    }

    public TryCatchAttribute(Type tException)
    {
        TException1 = tException;
    }

    public TryCatchAttribute
        (Type tException1, Type tException2)
    {
        TException1 = tException1;
        TException2 = tException2;
    }

    public TryCatchAttribute(Type tException1, Type tException2, Type tException3)
    {
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
    }

    public TryCatchAttribute(Type tException1, Type tException2, Type tException3, Type tException4)
    {
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
        TException4 = tException4;
    }

    public TryCatchAttribute(Type tException1, Type tException2, Type tException3, Type tException4, Type tException5)
    {
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
        TException4 = tException4;
        TException5 = tException5;
    }

    public bool AsMaybe { get; init; }

    public Type? TException1 { get; }

    public Type? TException2 { get; }

    public Type? TException3 { get; }

    public Type? TException4 { get; }

    public Type? TException5 { get; }
}

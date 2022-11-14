namespace RandomSkunk.Results;

[AttributeUsage(AttributeTargets.Assembly)]
public class AssemblyTryCatchAsResultAttribute : Attribute
{
    public AssemblyTryCatchAsResultAttribute(Type targetType)
    {
        TargetType = targetType;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, string methodName)
    {
        TargetType = targetType;
        MethodName = methodName;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, Type tException)
    {
        TargetType = targetType;
        TException1 = tException;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, string methodName, Type tException)
    {
        TargetType = targetType;
        MethodName = methodName;
        TException1 = tException;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, Type tException1, Type tException2)
    {
        TargetType = targetType;
        TException1 = tException1;
        TException2 = tException2;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, string methodName, Type tException1, Type tException2)
    {
        TargetType = targetType;
        MethodName = methodName;
        TException1 = tException1;
        TException2 = tException2;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, Type tException1, Type tException2, Type tException3)
    {
        TargetType = targetType;
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, string methodName, Type tException1, Type tException2, Type tException3)
    {
        TargetType = targetType;
        MethodName = methodName;
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, Type tException1, Type tException2, Type tException3, Type tException4)
    {
        TargetType = targetType;
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
        TException4 = tException4;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, string methodName, Type tException1, Type tException2, Type tException3, Type tException4)
    {
        TargetType = targetType;
        MethodName = methodName;
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
        TException4 = tException4;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, Type tException1, Type tException2, Type tException3, Type tException4, Type tException5)
    {
        TargetType = targetType;
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
        TException4 = tException4;
        TException5 = tException5;
    }

    public AssemblyTryCatchAsResultAttribute(Type targetType, string methodName, Type tException1, Type tException2, Type tException3, Type tException4, Type tException5)
    {
        TargetType = targetType;
        MethodName = methodName;
        TException1 = tException1;
        TException2 = tException2;
        TException3 = tException3;
        TException4 = tException4;
        TException5 = tException5;
    }

    public bool AsMaybe { get; init; }

    public Type TargetType { get; }

    public string? MethodName { get; }

    public Type? TException1 { get; }

    public Type? TException2 { get; }

    public Type? TException3 { get; }

    public Type? TException4 { get; }

    public Type? TException5 { get; }
}

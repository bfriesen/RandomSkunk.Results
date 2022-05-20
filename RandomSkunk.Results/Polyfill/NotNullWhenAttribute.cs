#if NET461 || NETSTANDARD2_0

namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
internal sealed class NotNullWhenAttribute : Attribute
{
    public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

    public bool ReturnValue { get; }
}

#endif

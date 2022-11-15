#if NETSTANDARD2_0

namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
internal class RequiresUnreferencedCodeAttribute : Attribute
{
    public RequiresUnreferencedCodeAttribute(string message)
    {
        Message = message;
    }

    public string Message { get; }

    public string? Url { get; set; }
}

#endif

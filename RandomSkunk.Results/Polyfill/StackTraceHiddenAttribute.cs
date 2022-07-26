#if !NET6_0_OR_GREATER

namespace System.Diagnostics;

/// <summary>
/// Types and Methods attributed with StackTraceHidden will be omitted from the stack trace text shown in
/// <see cref="StackTrace.ToString()"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
public sealed class StackTraceHiddenAttribute : Attribute
{
}

#endif

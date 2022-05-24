#if NET461 || NETSTANDARD2_0

namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class NotNullAttribute : Attribute
{
}

#endif

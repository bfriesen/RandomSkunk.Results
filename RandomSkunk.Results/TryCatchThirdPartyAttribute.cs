#if INCLUDE_ANALYZERS
namespace RandomSkunk.Results;

/// <summary>
/// Generates a "Try Adapter" for the target type, where the try adapter calls methods from the target type inside a try/catch
/// block and returns a <c>Success</c>, <c>Fail</c>, or <c>None</c> result, depending on the outcome of the method call. In other
/// words, a try adapter behaves the same as its target type, except its methods shouldn't throw an exception and instead return
/// a result.
/// <list type="bullet">
///     <item>
///         <description>When <see cref="MethodName"/> <strong>is not specified</strong>, the try adapter exposes all of
///             <see cref="TargetType"/>'s public methods.</description>
///     </item>
///     <item>
///         <description>When <see cref="MethodName"/> <strong>is specified</strong>, the try adapter exposes the specified
///             public method from <see cref="TargetType"/>. If another [assembly: TryCatchThirdParty] attribute targets the same
///             type and <strong>does not</strong> specify <see cref="MethodName"/>, this attribute supersedes it.
///             </description>
///     </item>
///     <item>
///         <description>If <see cref="TargetType"/> is <strong>not a static class</strong>, then a <c>Try()</c> extension method
///             for the target type is also generated.</description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// Create a try adapter for something you don't own with an `assembly: ` attribute:
/// <code>
/// [assembly: TryCatchThirdParty(typeof(System.IO.FileInfo))]
/// </code>
/// Get a try adapter for a <c>FileInfo</c> object by calling the <c>Try()</c> extension method. Then call one of the try
/// adapter's methods, which returns a result.
/// <code>
/// void Example(FileInfo fileInfo)
/// {
///     Result&lt;StreamWriter&gt; result = fileInfo.Try().CreateText();
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class TryCatchThirdPartyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    public TryCatchThirdPartyAttribute(Type targetType)
    {
        TargetType = targetType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="methodName">The name of the method.</param>
    public TryCatchThirdPartyAttribute(Type targetType, string methodName)
    {
        TargetType = targetType;
        MethodName = methodName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="exception">The type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, Type exception)
    {
        TargetType = targetType;
        Exception1 = exception;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="exception">The type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, string methodName, Type exception)
    {
        TargetType = targetType;
        MethodName = methodName;
        Exception1 = exception;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, Type exception1, Type exception2)
    {
        TargetType = targetType;
        Exception1 = exception1;
        Exception2 = exception2;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, string methodName, Type exception1, Type exception2)
    {
        TargetType = targetType;
        MethodName = methodName;
        Exception1 = exception1;
        Exception2 = exception2;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, Type exception1, Type exception2, Type exception3)
    {
        TargetType = targetType;
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, string methodName, Type exception1, Type exception2, Type exception3)
    {
        TargetType = targetType;
        MethodName = methodName;
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    /// <param name="exception4">The fourth type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, Type exception1, Type exception2, Type exception3, Type exception4)
    {
        TargetType = targetType;
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
        Exception4 = exception4;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    /// <param name="exception4">The fourth type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, string methodName, Type exception1, Type exception2, Type exception3, Type exception4)
    {
        TargetType = targetType;
        MethodName = methodName;
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
        Exception4 = exception4;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    /// <param name="exception4">The fourth type of exception to catch.</param>
    /// <param name="exception5">The fifth type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, Type exception1, Type exception2, Type exception3, Type exception4, Type exception5)
    {
        TargetType = targetType;
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
        Exception4 = exception4;
        Exception5 = exception5;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchThirdPartyAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    /// <param name="exception4">The fourth type of exception to catch.</param>
    /// <param name="exception5">The fifth type of exception to catch.</param>
    public TryCatchThirdPartyAttribute(Type targetType, string methodName, Type exception1, Type exception2, Type exception3, Type exception4, Type exception5)
    {
        TargetType = targetType;
        MethodName = methodName;
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
        Exception4 = exception4;
        Exception5 = exception5;
    }

    /// <summary>
    /// Gets the target type.
    /// </summary>
    public Type TargetType { get; }

    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    public string? MethodName { get; }

    /// <summary>
    /// Gets the first type of exception to catch.
    /// </summary>
    public Type? Exception1 { get; }

    /// <summary>
    /// Gets the second type of exception to catch.
    /// </summary>
    public Type? Exception2 { get; }

    /// <summary>
    /// Gets the third type of exception to catch.
    /// </summary>
    public Type? Exception3 { get; }

    /// <summary>
    /// Gets the fourth type of exception to catch.
    /// </summary>
    public Type? Exception4 { get; }

    /// <summary>
    /// Gets the fifth type of exception to catch.
    /// </summary>
    public Type? Exception5 { get; }

    /// <summary>
    /// Gets a value indicating whether methods that return a value will generate a method that returns <see cref="Maybe{T}"/>.
    /// When <see langword="false"/> (the default value), generated methods will return <see cref="Result{T}"/>. Methods that do
    /// not return a value always return <see cref="Result"/> and are not affected by this property.
    /// </summary>
    public bool AsMaybe { get; init; }
}
#endif

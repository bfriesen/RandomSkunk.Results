#if INCLUDE_ANALYZERS
namespace RandomSkunk.Results;

/// <summary>
/// Generates a "Try Adapter" for the target type, where the try adapter calls methods from the target type inside a try/catch
/// block and returns a <c>Success</c>, <c>Fail</c>, or <c>None</c> result, depending on the outcome of the method call. In other
/// words, a try adapter behaves the same as its target type, except its methods shouldn't throw an exception and instead return
/// a result.
/// <list type="bullet">
///     <item>
///         <description>When decorating a <strong>type</strong>, the target type is that type and the try adapter will expose
///             all of its public methods.</description>
///     </item>
///     <item>
///         <description>When decorating a <strong>method</strong>, the target type is the method's declaring type and the try
///             adapter will expose the decorated method. If the target type is <strong>also</strong> decorated with a [TryCatch]
///             attribute, this attribute supersedes it.</description>
///     </item>
///     <item>
///         <description>If the target type is <strong>not a static class</strong>, then a <c>Try()</c> extension method for the
///             target type is also generated.</description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// Create a try adapter by decorating a type or method:
/// <code>
/// [TryCatch]
/// public class MyTargetType
/// {
///     public int MyIntMethod() => 123;
/// }
/// </code>
/// Get a try adapter for a <c>MyTargetType</c> object by calling the <c>Try()</c> extension method. Then call one of the try
/// adapter's methods, which returns a result.
/// <code>
/// void Example(MyTargetType value)
/// {
///     Result&lt;int&gt; result = value.Try().MyIntMethod();
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Method)]
public sealed class TryCatchAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchAttribute"/> class.
    /// </summary>
    public TryCatchAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchAttribute"/> class.
    /// </summary>
    /// <param name="exception">The type of exception to catch.</param>
    public TryCatchAttribute(Type exception)
    {
        Exception1 = exception;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchAttribute"/> class.
    /// </summary>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    public TryCatchAttribute(Type exception1, Type exception2)
    {
        Exception1 = exception1;
        Exception2 = exception2;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchAttribute"/> class.
    /// </summary>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    public TryCatchAttribute(Type exception1, Type exception2, Type exception3)
    {
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchAttribute"/> class.
    /// </summary>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    /// <param name="exception4">The fourth type of exception to catch.</param>
    public TryCatchAttribute(Type exception1, Type exception2, Type exception3, Type exception4)
    {
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
        Exception4 = exception4;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryCatchAttribute"/> class.
    /// </summary>
    /// <param name="exception1">The first type of exception to catch.</param>
    /// <param name="exception2">The second type of exception to catch.</param>
    /// <param name="exception3">The third type of exception to catch.</param>
    /// <param name="exception4">The fourth type of exception to catch.</param>
    /// <param name="exception5">The fifth type of exception to catch.</param>
    public TryCatchAttribute(Type exception1, Type exception2, Type exception3, Type exception4, Type exception5)
    {
        Exception1 = exception1;
        Exception2 = exception2;
        Exception3 = exception3;
        Exception4 = exception4;
        Exception5 = exception5;
    }

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

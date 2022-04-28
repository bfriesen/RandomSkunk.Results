namespace RandomSkunk.Results;

/// <summary>
/// The base class for the result of an operation that has a return value.
/// </summary>
/// <typeparam name="T">The type of the return value of the operation.</typeparam>
public abstract class ResultBase<T> : ResultBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase{T}"/> class.
    /// </summary>
    /// <param name="callSite">Information about the code that created this result.</param>
    protected ResultBase(CallSite callSite)
        : base(callSite)
    {
    }

    /// <summary>
    /// Gets the value of the success result, or throws an
    /// <see cref="InvalidOperationException"/> if <see cref="ResultBase.IsSuccess"/> is false.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If <see cref="ResultBase.IsSuccess"/> is not true.
    /// </exception>
    [NotNull]
    public virtual T Value => throw Exceptions.CannotAccessValueUnlessSuccess;

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <param name="otherValue">The value to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <see cref="ResultBase.IsSuccess"/> is <see langword="true"/>
    /// and <see cref="Value"/> equals <paramref name="otherValue"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool Equals(T otherValue) =>
        Equals(otherValue, EqualityComparer<T>.Default);

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <see cref="ResultBase.IsSuccess"/> is <see langword="true"/>
    /// and <see cref="Value"/> equals <paramref name="otherValue"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    public bool Equals(T otherValue, IEqualityComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return IsSuccess && comparer.Equals(Value, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSuccessValue"/> function.
    /// </summary>
    /// <param name="isSuccessValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <see cref="ResultBase.IsSuccess"/> is <see langword="true"/>
    /// and <paramref name="isSuccessValue"/> evaluates to <see langword="true"/> when
    /// passed <see cref="Value"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSuccessValue"/> is <see langword="null"/>.
    /// </exception>
    public bool Equals(Func<T, bool> isSuccessValue)
    {
        if (isSuccessValue is null) throw new ArgumentNullException(nameof(isSuccessValue));

        return IsSuccess && isSuccessValue(Value);
    }

    /// <summary>
    /// Gets the value of the success result, or the specified default value if
    /// the result type is <c>fail</c>.
    /// </summary>
    /// <param name="fallbackValue">
    /// The fallback value to return if <see cref="ResultBase.IsSuccess"/> is
    /// <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see cref="Value"/> if <see cref="ResultBase.IsSuccess"/> is <see langword="true"/>;
    /// otherwise, <paramref name="fallbackValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    [return: NotNull]
    public T GetValueOr([DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        if (IsSuccess)
            return Value;

        return fallbackValue;
    }

    /// <summary>
    /// Gets the value of the success result, or the specified default value if
    /// <see cref="ResultBase.IsSuccess"/> is <see langword="false"/>.
    /// </summary>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if
    /// <see cref="ResultBase.IsSuccess"/> is <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see cref="Value"/> if <see cref="ResultBase.IsSuccess"/> is <see langword="true"/>;
    /// otherwise, the value returned by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    [return: NotNull]
    public T GetValueOr(Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (IsSuccess)
            return Value;

        return getFallbackValue() ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));
    }
}

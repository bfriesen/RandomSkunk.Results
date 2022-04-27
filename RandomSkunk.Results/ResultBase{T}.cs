using System.Diagnostics.CodeAnalysis;

namespace RandomSkunk.Results
{
    /// <summary>
    /// The base class for the result of an operation that has a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    public abstract class ResultBase<T> : ResultBase
    {
        protected ResultBase(CallSite callSite)
            : base(callSite) { }

        /// <summary>
        /// Gets the value of the success result, or throws an
        /// <see cref="InvalidOperationException"/> if <see cref="ResultBase.IsSuccess"/> is false.
        /// </summary>
        [NotNull] public virtual T Value => throw new InvalidOperationException("Value cannot be accessed if IsSuccess is false.");

        /// <summary>
        /// Determines whether the <see cref="Value"/> of the result equals the
        /// <paramref name="otherValue"/>.
        /// </summary>
        /// <param name="otherValue">The value to compare.</param>
        public bool Equals(T otherValue) =>
            Equals(otherValue, EqualityComparer<T>.Default);

        /// <summary>
        /// Determines whether the <see cref="Value"/> of the result equals the
        /// <paramref name="otherValue"/>.
        /// </summary>
        /// <param name="otherValue">The value to compare.</param>
        /// <param name="comparer">
        /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values.
        /// </param>
        public bool Equals(T otherValue, IEqualityComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));

            return IsSuccess && comparer.Equals(Value, otherValue);
        }

        /// <summary>
        /// Determines whether the <see cref="Value"/> of the result is equal to another value as
        /// defined by the <paramref name="isSuccessValue"/> function.
        /// </summary>
        /// <param name="isSuccessValue">
        /// A function that defines the equality of the result value.
        /// </param>
        public bool Equals(Func<T, bool> isSuccessValue)
        {
            ArgumentNullException.ThrowIfNull(isSuccessValue, nameof(isSuccessValue));

            return IsSuccess && isSuccessValue(Value);
        }

        /// <summary>
        /// Gets the value of the success result, or the specified default value if
        /// <see cref="ResultBase.IsSuccess"/> is <see langword="false"/>.
        /// </summary>
        /// <param name="defaultValue">
        /// The fallback value to return if <see cref="ResultBase.IsSuccess"/> is
        /// <see langword="false"/>.
        /// </param>
        [return: NotNull]
        public T GetValueOr([DisallowNull] T defaultValue)
        {
            ArgumentNullException.ThrowIfNull(defaultValue, nameof(defaultValue));

            if (IsSuccess)
                return Value;
            return defaultValue;
        }

        /// <summary>
        /// Gets the value of the success result, or the specified default value if
        /// <see cref="ResultBase.IsSuccess"/> is <see langword="false"/>.
        /// </summary>
        /// <param name="getDefaultValue">
        /// A function that creates the fallback value to return if
        /// <see cref="ResultBase.IsSuccess"/> is <see langword="false"/>.
        /// </param>
        [return: NotNull]
        public T GetValueOr(Func<T> getDefaultValue)
        {
            ArgumentNullException.ThrowIfNull(getDefaultValue, nameof(getDefaultValue));

            if (IsSuccess)
                return Value;
            return getDefaultValue() ?? throw new ArgumentException("Function must not return null value.", nameof(getDefaultValue));
        }
    }
}

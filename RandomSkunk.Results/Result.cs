using System.Diagnostics;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result without a value.
/// </summary>
/// <remarks>
/// Use <see cref="Create"/> to create instances of this type.
/// </remarks>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial struct Result : IEquatable<Result>
{
    /// <summary>
    /// The factory object used to create instances of <see cref="Result"/>. This field is
    /// read-only.
    /// </summary>
    public static readonly IResultFactory Create = new Factory();

    internal readonly ResultType _type;
    private readonly Error? _error;

    private Result(bool success, Error? error = null)
    {
        if (success)
        {
            _type = Success;
            _error = null;
        }
        else
        {
            _type = Fail;
            _error = error ?? new Error();
        }
    }

    /// <summary>
    /// Gets the type of the result: <see cref="Success"/> or <see cref="Fail"/>.
    /// </summary>
    public ResultType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Success</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSuccess => _type == Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == Fail;

    /// <summary>
    /// Gets a value indicating whether this is a default instance of the <see cref="Result"/>
    /// struct.
    /// </summary>
    public bool IsDefault => _type == Fail && _error is null;

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is equal to the
    /// <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="left"/> parameter is equal to the
    /// <paramref name="right"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Result left, Result right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is not equal to the
    /// <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="left"/> parameter is not equal to the
    /// <paramref name="right"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Result left, Result right) => !(left == right);

    /// <inheritdoc/>
    public bool Equals(Result other) =>
        _type == other._type
        && (IsSuccess
            || (IsFail && EqualityComparer<Error?>.Default.Equals(_error, other._error)));

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Result result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1710757158;
        hashCode = (hashCode * -1521134295) + _type.GetHashCode();
        hashCode = (hashCode * -1521134295) + (IsFail ? Error().GetHashCode() : 0);
        return hashCode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;

    private string GetDebuggerDisplay() =>
        Match(
            () => "Success",
            error => $"Fail({error.Type}: \"{error.Message}\")");

    private sealed class Factory : IResultFactory
    {
        public Result Success() => new(success: true);

        public Result Fail(Error? error = null) => new(success: false, error);
    }
}

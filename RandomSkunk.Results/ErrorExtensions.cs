namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for the <see cref="Error"/> class.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Returns a new <see cref="Error"/> identical to <paramref name="source"/>, except with the specified identifier.
    /// </summary>
    /// <param name="source">The source error.</param>
    /// <param name="identifier">The identifier of the returned error.</param>
    /// <returns>A new <see cref="Error"/> with its identifier set to <paramref name="identifier"/>.</returns>
    public static Error WithIdentifier(this Error source, string? identifier)
    {
        return new Error(source.Message, source.StackTrace, source.ErrorCode, identifier, source.Type, source.InnerError);
    }

    /// <summary>
    /// Returns a new <see cref="ExtendedError"/> identical to <paramref name="source"/>, except with the specified identifier.
    /// </summary>
    /// <param name="source">The source error.</param>
    /// <param name="identifier">The identifier of the returned error.</param>
    /// <returns>A new <see cref="ExtendedError"/> with its identifier set to <paramref name="identifier"/>.</returns>
    public static ExtendedError WithIdentifier(this ExtendedError source, string? identifier)
    {
        return new ExtendedError(source.Message, source.StackTrace, source.ErrorCode, identifier, source.Type, source.InnerError, source.Extensions);
    }
}

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
    /// Returns a new <see cref="ExpandableError"/> identical to <paramref name="source"/>, except with the specified identifier.
    /// </summary>
    /// <param name="source">The source error.</param>
    /// <param name="identifier">The identifier of the returned error.</param>
    /// <returns>A new <see cref="ExpandableError"/> with its identifier set to <paramref name="identifier"/>.</returns>
    public static ExpandableError WithIdentifier(this ExpandableError source, string? identifier)
    {
        return new ExpandableError(source.Message, source.StackTrace, source.ErrorCode, identifier, source.Type, source.InnerError, source.Extensions);
    }
}

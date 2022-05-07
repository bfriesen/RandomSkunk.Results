namespace RandomSkunk.Results;

/// <summary>
/// Defines the types of maybe results.
/// </summary>
public enum MaybeResultType
{
    /// <summary>
    /// An unsuccessful result. The default value.
    /// </summary>
    Fail = 0,

    /// <summary>
    /// A successful result with a value.
    /// </summary>
    Some,

    /// <summary>
    /// A successful result without a value.
    /// </summary>
    None,
}

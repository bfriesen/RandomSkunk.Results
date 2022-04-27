namespace RandomSkunk.Results;

/// <summary>
/// Defines the types of maybe results.
/// </summary>
public enum MaybeResultType
{
    /// <summary>
    /// The operation was successful and has a value.
    /// </summary>
    Some,

    /// <summary>
    /// The operation was successful but has no value.
    /// </summary>
    None,

    /// <summary>
    /// The operation was not successful.
    /// </summary>
    Fail,
}

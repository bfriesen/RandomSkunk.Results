namespace RandomSkunk.Results;

/// <summary>
/// The <see cref="Unit"/> struct represents the lack of a value. It has no fields or properties, and the only possible value is
/// <c>default(Unit)</c>. It is used when coercing a value from a <c>Success</c> <see cref="Result"/> in LINQ-to-Results queries
/// and value-tuple-of-results extension methods.
/// </summary>
public readonly record struct Unit
{
    /// <summary>
    /// Represents the sole value of the <see cref="Unit"/> struct.
    /// </summary>
    public static readonly Unit Value;

    /// <summary>
    /// Returns a description of the <see cref="Unit"/> type.
    /// </summary>
    /// <returns>A description of the <see cref="Unit"/> type.</returns>
    public override string ToString() => "(no value)";
}

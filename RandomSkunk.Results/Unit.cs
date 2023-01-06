namespace RandomSkunk.Results;

/// <summary>
/// A value of type <see cref="Unit"/> indicates the lack of a value.
/// </summary>
/// <remarks>
/// The <see cref="Result"/> type implements <see cref="IResult{T}"/>, where <c>T</c> is type <see cref="Unit"/>. This allows
/// <see cref="Result"/> to seamlessly participate in LINQ-to-Result queries.
/// </remarks>
public readonly struct Unit
{
    /// <summary>
    /// Returns a description of the <see cref="Unit"/> type.
    /// </summary>
    /// <returns>A description of the <see cref="Unit"/> type.</returns>
    public override string ToString() => "(no value)";
}

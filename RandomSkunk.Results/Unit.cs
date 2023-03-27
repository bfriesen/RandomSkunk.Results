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
    /// Invokes the specified <see cref="Action"/> and returns the default <see cref="Unit"/> value.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <returns>The default <see cref="Unit"/> value.</returns>
    /// <remarks>
    /// This method exists in order to make it possible to perform side-effects in the middle of a LINQ query. In this example,
    /// we need to retrieve a <c>User</c> object, set its <c>IsDisabled</c> to true, and then update the user.
    /// <code>
    /// Result result =
    ///     // Retrieve the user (GetById returns Result&lt;User&gt;).
    ///     from user in _userRepository.GetById(userId)
    ///
    ///     // Disable the user and assign the resulting Unit to a throw-away variable.
    ///     let userDisabled = Unit.Invoke(() => user.IsDisabled = true)
    ///
    ///     // Update the user (UpdateUser returns Result).
    ///     from userUpdated in _userRepository.UpdateUser(user)
    ///
    ///     // This operation doesn't have a return value, so select the default Unit value.
    ///     select Unit.Value;
    /// </code>
    /// </remarks>
    public static Unit Invoke(Action action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        action();
        return Value;
    }

    /// <summary>
    /// Returns a description of the <see cref="Unit"/> type.
    /// </summary>
    /// <returns>A description of the <see cref="Unit"/> type.</returns>
    public override string ToString() => "(no value)";
}

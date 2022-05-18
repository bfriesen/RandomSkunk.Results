namespace RandomSkunk.Results;

/// <summary>
/// A helper class for creating delegates, specifically for converting to results with extension methods from the
/// <see cref="DelegateExtensions"/> class.
/// </summary>
public static class Delegates
{
    /// <summary>
    /// Returns the specified <c>Action</c> delegate.
    /// </summary>
    /// <param name="action">The <c>Action</c> delegate to return.</param>
    /// <returns>The <c>Action</c> delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="action"/> is <see langword="null"/>.</exception>
    public static Action Action(Action action) => action ?? throw new ArgumentNullException(nameof(action));

    /// <summary>
    /// Returns the specified <c>Func&lt;T&gt;</c> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the method that the delegate encapsulates.</typeparam>
    /// <param name="func">The <c>Func&lt;T&gt;</c> delegate to return.</param>
    /// <returns>The <c>Func&lt;T&gt;</c> delegate.</returns>
    /// <exception cref="ArgumentException">
    /// If <typeparamref name="T"/> is <see cref="Task"/> or <see cref="Task{TResult}"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">If <paramref name="func"/> is <see langword="null"/>.</exception>
    public static Func<T> Func<T>(Func<T> func)
    {
        if (typeof(Task).IsAssignableFrom(typeof(T)))
            throw new ArgumentOutOfRangeException(nameof(T), "Generic argument T cannot be a Task. Call the AsyncAction or AsyncFunc<T> method instead.");

        return func ?? throw new ArgumentNullException(nameof(func));
    }

    /// <summary>
    /// Returns the specified <c>AsyncAction</c> delegate.
    /// </summary>
    /// <param name="asyncAction">The <c>AsyncAction</c> delegate to return.</param>
    /// <returns>The <c>AsyncAction</c> delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="asyncAction"/> is <see langword="null"/>.</exception>
    public static AsyncAction AsyncAction(AsyncAction asyncAction) => asyncAction ?? throw new ArgumentNullException(nameof(asyncAction));

    /// <summary>
    /// Returns the specified <c>AsyncFunc&lt;T&gt;</c> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the method that the delegate encapsulates.</typeparam>
    /// <param name="asyncFunc">The <c>AsyncFunc&lt;T&gt;</c> delegate to return.</param>
    /// <returns>The <c>AsyncFunc&lt;T&gt;</c> delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="asyncFunc"/> is <see langword="null"/>.</exception>
    public static AsyncFunc<T> AsyncFunc<T>(AsyncFunc<T> asyncFunc) => asyncFunc ?? throw new ArgumentNullException(nameof(asyncFunc));
}

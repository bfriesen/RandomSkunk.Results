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
    public static Action Action(Action action) => action;

    /// <summary>
    /// Returns the specified <c>Func&lt;T&gt;</c> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the method that the delegate encapsulates.</typeparam>
    /// <param name="func">The <c>Func&lt;T&gt;</c> delegate to return.</param>
    /// <returns>The <c>Func&lt;T&gt;</c> delegate.</returns>
    public static Func<T> Func<T>(Func<T> func) => func;

    /// <summary>
    /// Returns the specified <c>AsyncAction</c> delegate.
    /// </summary>
    /// <param name="asyncAction">The <c>AsyncAction</c> delegate to return.</param>
    /// <returns>The <c>AsyncAction</c> delegate.</returns>
    public static AsyncAction AsyncAction(AsyncAction asyncAction) => asyncAction;

    /// <summary>
    /// Returns the specified <c>AsyncFunc&lt;T&gt;</c> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the method that the delegate encapsulates.</typeparam>
    /// <param name="asyncFunc">The <c>AsyncFunc&lt;T&gt;</c> delegate to return.</param>
    /// <returns>The <c>AsyncFunc&lt;T&gt;</c> delegate.</returns>
    public static AsyncFunc<T> AsyncFunc<T>(AsyncFunc<T> asyncFunc) => asyncFunc;
}

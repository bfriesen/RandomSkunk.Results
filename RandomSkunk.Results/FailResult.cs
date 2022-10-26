namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that allow an application to be notified whenever a fail result has been created.
/// </summary>
public static class FailResult
{
    private static Action<Error>? _callback;
    private static Func<Error, Error>? _replaceError;

    /// <summary>
    /// Sets the callback function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailCallback">The callback function.</param>
    public static void SetCallbackFunction(Action<Error> onFailCallback) =>
        _callback = onFailCallback;

    /// <summary>
    /// Sets the error replacement function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailReplaceError">The error replacement function.</param>
    public static void SetReplaceErrorFunction(Func<Error, Error> onFailReplaceError) =>
        _replaceError = onFailReplaceError;

    internal static Error InvokeReplaceErrorIfSet(Error error)
    {
        var replaceError = _replaceError;

        if (replaceError is null)
            return error;

        try
        {
            return replaceError(error);
        }
        catch
        {
        }

        return error;
    }

    internal static void InvokeCallbackIfSet(Error error)
    {
        var callback = _callback;

        if (callback is null)
            return;

        try
        {
            callback(error);
        }
        catch
        {
        }
    }
}

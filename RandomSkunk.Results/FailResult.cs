namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that allow an application to be notified whenever a fail result has been created.
/// </summary>
public static class FailResult
{
    private static readonly ConditionalWeakTable<Error, ErrorInfo> _errorInfoTable = new();

    private static Action<Error>? _callback;

    /// <summary>
    /// Gets or sets a value indicating whether exceptions that are thrown by callback function parameters are caught and
    /// returned as a <c>Fail</c> result. Default is <see langword="true"/>.
    /// </summary>
    public static bool CatchCallbackExceptions { get; set; } = true;

    /// <summary>
    /// Sets the callback function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailCallback">The callback function.</param>
    public static void SetCallbackFunction(Action<Error>? onFailCallback) =>
        _callback = onFailCallback;

    /// <summary>
    /// Adds a callback function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailCallback">The callback function.</param>
    public static void AddCallbackFunction(Action<Error> onFailCallback) =>
        _callback += onFailCallback;

    internal static void InvokeCallback(Error error)
    {
        var callback = _callback;

        if (callback is null)
            return;

        var errorInfo = _errorInfoTable.GetValue(error, _ => new());
        if (errorInfo.Handled)
            return;

        try
        {
            callback(error);
        }
        catch
        {
        }

        errorInfo.Handled = true;
    }

    private class ErrorInfo
    {
        public bool Handled { get; set; }
    }
}

namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that allow an application to be notified whenever a fail result has been created.
/// </summary>
public static class FailResult
{
    private static readonly ConditionalWeakTable<Error, ErrorInfo> _errorInfoTable = new();

    private static Action<Error>? _callback;
    private static Func<Error, Error>? _replaceError;

    /// <summary>
    /// Sets the callback function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailCallback">The callback function.</param>
    public static void SetCallbackFunction(Action<Error>? onFailCallback) =>
        _callback = onFailCallback;

    /// <summary>
    /// Sets the error replacement function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailReplaceError">The error replacement function.</param>
    public static void SetReplaceErrorFunction(Func<Error, Error>? onFailReplaceError) =>
        _replaceError = onFailReplaceError;

    /// <summary>
    /// Adds a callback function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailCallback">The callback function.</param>
    public static void AddCallbackFunction(Action<Error> onFailCallback) =>
        _callback += onFailCallback;

    /// <summary>
    /// Adds an error replacement function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <param name="onFailReplaceError">The error replacement function.</param>
    public static void AddReplaceErrorFunction(Func<Error, Error> onFailReplaceError)
    {
        if (onFailReplaceError is null)
            return;

        var replaceError = _replaceError;
        if (replaceError is null)
        {
            _replaceError = onFailReplaceError;
        }
        else
        {
            _replaceError = error => onFailReplaceError(replaceError(error));
        }
    }

    internal static void Handle(ref Error error)
    {
        var errorInfo = _errorInfoTable.GetValue(error, _ => new());
        if (errorInfo.Handled)
            return;

        var originalError = error;

        var replaceError = _replaceError;
        var callback = _callback;

        if (replaceError is not null)
        {
            try
            {
                error = replaceError(error);
            }
            catch
            {
            }
        }

        if (callback is not null)
        {
            try
            {
                callback(error);
            }
            catch
            {
            }
        }

        errorInfo.Handled = true;

        if (!ReferenceEquals(error, originalError))
        {
            _errorInfoTable.Add(error, errorInfo);
            _errorInfoTable.Remove(originalError);
        }
    }

    private class ErrorInfo
    {
        public bool Handled { get; set; }
    }
}

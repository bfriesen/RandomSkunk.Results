namespace RandomSkunk.Results;

/// <summary>
/// Defines global settings for results.
/// </summary>
public static class ResultSettings
{
    private static readonly ConditionalWeakTable<Error, FailResultInfo> _failResultInfoTable = new();

    /// <summary>
    /// Gets or sets a callback function that is invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    /// <remarks>
    /// <em>The callback function that this property is set to should not throw an exception. If it does throw, when it is
    /// invoked upon the creation of a <c>Fail</c> result, the exception will be silently caught and ignored.</em>
    /// </remarks>
    public static Action<Error>? FailResultCreated { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether exceptions that are thrown by callback function parameters are caught and
    /// returned as a <c>Fail</c> result. Default is <see langword="true"/>.
    /// </summary>
    public static bool CatchCallbackExceptions { get; set; } = true;

    internal static void InvokeFailResultCreatedCallback(Error error)
    {
        var callback = FailResultCreated;

        if (callback is null)
            return;

        var failResultInfo = _failResultInfoTable.GetValue(error, _ => new());
        if (failResultInfo.CallbackInvoked)
            return;

        try
        {
            callback(error);
        }
        catch
        {
        }

        failResultInfo.CallbackInvoked = true;
    }

    private class FailResultInfo
    {
        public bool CallbackInvoked { get; set; }
    }
}

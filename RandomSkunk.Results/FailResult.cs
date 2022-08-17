namespace RandomSkunk.Results;

/// <summary>
/// Defines the <see cref="OnCreated"/> property, allowing an application to be notified whenever a fail result has been created.
/// </summary>
public static class FailResult
{
    /// <summary>
    /// Gets or sets the optional callback function that will be invoked whenever a <c>Fail</c> result is created.
    /// </summary>
    public static Action<Error>? OnCreated { get; set; }

    internal static void InvokeOnCreated(Error error)
    {
        var onCreated = OnCreated;
        if (onCreated is null)
            return;

        try
        {
            onCreated.Invoke(error);
        }
        catch
        {
        }
    }
}

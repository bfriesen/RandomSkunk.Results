namespace RandomSkunk.Results;

/// <summary>
/// Defines settings for <c>await</c> calls made by the RandomSkunk.Results libraries.
/// </summary>
public static class AwaitSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether <c>await</c> calls attempt to marshal the continuation back to the original
    /// context captured. Default is <see langword="true"/>.
    /// </summary>
    public static bool ContinueOnCapturedContext { get; set; } = true;
}

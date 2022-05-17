namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    private static readonly Func<Error, Error> _identityErrorFunction = error => error;
}

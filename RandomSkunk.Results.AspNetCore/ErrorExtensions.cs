namespace RandomSkunk.Results.AspNetCore;

/// <summary>
/// Defines extension methods for the <see cref="Error"/> class.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Gets a <see cref="ProblemDetails"/> object that is equavalent to the error object.
    /// </summary>
    /// <param name="source">
    /// The <see cref="Error"/> to create a <see cref="ProblemDetails"/> from.
    /// </param>
    /// <param name="title">
    /// A short, human-readable summary of the problem type.It SHOULD NOT change from
    /// occurrence to occurrence of the problem, except for purposes of localization (e.g.,
    /// using proactive content negotiation; see[RFC7231], Section 3.4).
    /// </param>
    /// <param name="type">
    /// A URI reference [RFC3986] that identifies the problem type. This specification
    /// encourages that, when dereferenced, it provide human-readable documentation for
    /// the problem type (e.g., using HTML [W3C.REC-html5-20141028]). When this member
    /// is not present, its value is assumed to be "about:blank".
    /// </param>
    /// <param name="instance">
    /// A URI reference that identifies the specific occurrence of the problem. It may
    /// or may not yield further information if dereferenced.
    /// </param>
    /// <returns>The equivalent problem details object.</returns>
    public static ProblemDetails GetProblemDetails(this Error source, string? title = null, string? type = null, string? instance = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = source.ErrorCode,
            Detail = source.Message,
            Instance = instance,
            Extensions = { ["errorType"] = source.Type },
        };

        if (source.StackTrace is not null)
            problemDetails.Extensions["errorStackTrace"] = source.StackTrace;

        if (source.Identifier is not null)
            problemDetails.Extensions["errorIdentifier"] = source.Identifier;

        return problemDetails;
    }
}

using System.Globalization;

namespace RandomSkunk.Results;

/// <summary>
/// An <see cref="AggregateException"/> implementation for an <see cref="AggregateError"/>.
/// </summary>
public class AggregateErrorException : AggregateException
{
    private readonly string? _stackTrace;
    private string? _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateErrorException"/> class.
    /// </summary>
    /// <param name="aggregateError">The original <see cref="AggregateError"/>.</param>
    public AggregateErrorException(AggregateError aggregateError)
        : base(aggregateError.Message, aggregateError.InnerErrors.Select(error => (Exception)error))
    {
        Dictionary<string, object> extensions;
#if NET7_0_OR_GREATER
        extensions = new(aggregateError.Extensions);
#else
        extensions = [];
        foreach (var item in aggregateError.Extensions)
            ((IDictionary<string, object>)extensions).Add(item);
#endif

        Extensions = new ReadOnlyDictionary<string, object>(extensions);

        extensions.Remove(AggregateError._innerErrorsFieldFullName);

        if (aggregateError.TryGet(Error._originalExceptionTypeExtensionName, out string? originalExceptionType))
        {
            OriginalExceptionType = originalExceptionType;
            extensions.Remove(Error._originalExceptionTypeExtensionName);
        }

        if (aggregateError.TryGet("System.Exception.StackTrace", out string? stackTrace))
        {
            _stackTrace = stackTrace;
            extensions.Remove("System.Exception.StackTrace");
        }
        else
        {
            _stackTrace = null;
        }

        if (aggregateError.TryGet("System.Exception.Source", out string? source))
        {
            _source = source;
            extensions.Remove("System.Exception.Source");
        }
        else
        {
            _source = null;
        }

        if (aggregateError.TryGet("System.Exception.HResult", out string? hresultString)
            && (int.TryParse(ErrorException.HexSpecifierRegex().Replace(hresultString, string.Empty), NumberStyles.HexNumber, null, out var hresult)
                || int.TryParse(hresultString, out hresult)))
        {
            HResult = hresult;
            extensions.Remove("System.Exception.HResult");
        }

        if (aggregateError.TryGet("System.Exception.HelpLink", out string? helpLink))
        {
            HelpLink = helpLink;
            extensions.Remove("System.Exception.HelpLink");
        }

        foreach (var dataItem in extensions.Where(item => item.Key.StartsWith("System.Exception.Data.")))
        {
            var key = ErrorException.DataPropertyRegex().Replace(dataItem.Key, string.Empty);
            Data[key] = dataItem.Value;
            extensions.Remove(dataItem.Key);
        }

        OriginalError = aggregateError;
    }

    /// <inheritdoc/>
    public override string? StackTrace => base.StackTrace ?? _stackTrace;

    /// <inheritdoc/>
    public override string? Source { get => base.Source ?? _source; set => _source = value; }

    /// <summary>
    /// Gets the <see cref="OriginalError"/> for this exception.
    /// </summary>
    public AggregateError OriginalError { get; }

    /// <summary>
    /// Gets the title for the error.
    /// </summary>
    public string Title => OriginalError.Title;

    /// <summary>
    /// Gets the optional error code.
    /// </summary>
    public int? ErrorCode => OriginalError.ErrorCode;

    /// <summary>
    /// Gets the optional identifier of the error.
    /// </summary>
    public string? Identifier => OriginalError.Identifier;

    /// <summary>
    /// Gets additional properties for the error.
    /// </summary>
    public IReadOnlyDictionary<string, object> Extensions { get; }

    /// <summary>
    /// Gets the <see cref="Type"/> of the <see cref="Exception"/> that was used to create <see cref="OriginalError"/>, or
    /// <see langword="null"/> if <see cref="OriginalError"/> was not created from an <see cref="Exception"/>.
    /// </summary>
    public string? OriginalExceptionType { get; }

    /// <inheritdoc/>
    public override string ToString() => OriginalError.ToString();
}

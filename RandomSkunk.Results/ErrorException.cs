using System.Globalization;
using System.Text.RegularExpressions;

namespace RandomSkunk.Results;

/// <summary>
/// An <see cref="Exception"/> implementation for an <see cref="Error"/>.
/// </summary>
public partial class ErrorException : Exception
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    private const string _dataPropertyPattern = @"^System\.Exception\.Data\.";

    [StringSyntax(StringSyntaxAttribute.Regex)]
    private const string _hexSpecifierPattern = "^0x";

    private readonly string? _stackTrace;
    private string? _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorException"/> class.
    /// </summary>
    /// <param name="originalError">The original <see cref="Error"/>.</param>
    public ErrorException(Error originalError)
        : base(originalError.Message, originalError.InnerError)
    {
        Dictionary<string, object> extensions;
#if NET7_0_OR_GREATER
        extensions = new(originalError.Extensions);
#else
        extensions = [];
        foreach (var item in originalError.Extensions)
            ((IDictionary<string, object>)extensions).Add(item);
#endif

        Extensions = new ReadOnlyDictionary<string, object>(extensions);

        if (originalError.TryGet(Error._originalExceptionTypeExtensionName, out string? originalExceptionType))
        {
            OriginalExceptionType = originalExceptionType;
            extensions.Remove(Error._originalExceptionTypeExtensionName);
        }

        if (originalError.TryGet("System.Exception.StackTrace", out string? stackTrace))
        {
            _stackTrace = stackTrace;
            extensions.Remove("System.Exception.StackTrace");
        }
        else
        {
            _stackTrace = null;
        }

        if (originalError.TryGet("System.Exception.Source", out string? source))
        {
            _source = source;
            extensions.Remove("System.Exception.Source");
        }
        else
        {
            _source = null;
        }

        if (originalError.TryGet("System.Exception.HResult", out string? hresultString)
            && (int.TryParse(HexSpecifierRegex().Replace(hresultString, string.Empty), NumberStyles.HexNumber, null, out var hresult)
                || int.TryParse(hresultString, out hresult)))
        {
            HResult = hresult;
            extensions.Remove("System.Exception.HResult");
        }

        if (originalError.TryGet("System.Exception.HelpLink", out string? helpLink))
        {
            HelpLink = helpLink;
            extensions.Remove("System.Exception.HelpLink");
        }

        foreach (var dataItem in extensions.Where(item => item.Key.StartsWith("System.Exception.Data.")))
        {
            var key = DataPropertyRegex().Replace(dataItem.Key, string.Empty);
            Data[key] = dataItem.Value;
            extensions.Remove(dataItem.Key);
        }

        OriginalError = originalError;
    }

    /// <inheritdoc/>
    public override string? StackTrace => base.StackTrace ?? _stackTrace;

    /// <inheritdoc/>
    public override string? Source { get => base.Source ?? _source; set => _source = value; }

    /// <summary>
    /// Gets the <see cref="OriginalError"/> for this exception.
    /// </summary>
    public Error OriginalError { get; }

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

#if NET7_0_OR_GREATER
    [GeneratedRegex(_dataPropertyPattern)]
    private static partial Regex DataPropertyRegex();

    [GeneratedRegex(_hexSpecifierPattern)]
    private static partial Regex HexSpecifierRegex();
#else
    private static Regex DataPropertyRegex() => DataPropertyRegex_0.Instance;

    private static Regex HexSpecifierRegex() => HexSpecifierRegex_1.Instance;

    private static class DataPropertyRegex_0
    {
        public static readonly Regex Instance = new(_dataPropertyPattern, RegexOptions.Compiled);
    }

    private static class HexSpecifierRegex_1
    {
        public static readonly Regex Instance = new(_hexSpecifierPattern, RegexOptions.Compiled);
    }
#endif
}

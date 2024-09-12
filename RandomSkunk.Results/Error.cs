using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading;

namespace RandomSkunk.Results;

/// <summary>
/// Defines an error.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
[JsonConverter(typeof(ErrorJsonConverter))]
public record class Error
{
    internal static readonly string _originalExceptionTypeExtensionName = $"{GetTypeFullName(typeof(Error))}.ExceptionType";

    private const string _defaultMessage = "An error occurred.";
    private const string _messageFormatForExceptionThrownInCallback = "An exception was thrown in the '{0}' callback parameter. See InnerError for details.";

    private static readonly ConcurrentDictionary<Type, string> _defaultTitleCache = new();
    private static readonly ConcurrentDictionary<Type, IEnumerable<Property>> _propertiesByExceptionType = new();
    private static readonly Lazy<Error> _defaultError = new(() => new Error());
    private static readonly IReadOnlyDictionary<string, object> _emptyExtensions = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

    private readonly string _title;
    private readonly string _message;
    private readonly string? _identifier;
    private readonly IReadOnlyDictionary<string, object> _extensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    public Error()
    {
        _title = GetTypeNameAsSentenceCase(GetType());
        _message = _defaultMessage;
        _extensions = _emptyExtensions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="extensions">Additional properties for the error.</param>
    public Error(params (string Key, object Value)[]? extensions)
    {
        _title = GetTypeNameAsSentenceCase(GetType());
        _message = _defaultMessage;
        _extensions = extensions is null
            ? _emptyExtensions
            : new ReadOnlyDictionary<string, object>(extensions.Where(x => x.Value is not null).ToDictionary(x => x.Key, x => x.Value));
    }

    /// <summary>
    /// Gets the title for the error.
    /// </summary>
    /// <remarks>
    /// The default value for this property is derived from the name of the type of this <see cref="Error"/> (most likely
    /// "Error"). If this property is initialized to <see langword="null"/>, nothing happens - the value remains the default
    /// title.
    /// </remarks>
    public string Title
    {
        get => _title;
        init => _title = string.IsNullOrWhiteSpace(value) ? _title : value;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <remarks>
    /// The default value for this property is "An error occurred.". If this property is initialized to <see langword="null"/>,
    /// the value is set to this default message.
    /// </remarks>
    public string Message
    {
        get => _message;
        init => _message = string.IsNullOrWhiteSpace(value) ? _defaultMessage : value;
    }

    /// <summary>
    /// Gets the optional error code.
    /// </summary>
    public int? ErrorCode { get; init; }

    /// <summary>
    /// Gets the optional identifier of the error.
    /// </summary>
    public string? Identifier
    {
        get => _identifier;
        init => _identifier = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    /// <summary>
    /// Gets additional properties for the error.
    /// </summary>
    public IReadOnlyDictionary<string, object> Extensions
    {
        get => _extensions;
        init
        {
            value ??= _emptyExtensions;
            if (value.Count > 0)
            {
                if (_extensions.Count > 0)
                {
                    var extensions = _extensions.ToDictionary(item => item.Key, item => item.Value);
                    foreach (var item in value)
                        extensions.Add(item.Key, item.Value);
                    _extensions = new ReadOnlyDictionary<string, object>(extensions);
                }
                else
                {
                    _extensions = value;
                }
            }
        }
    }

    /// <summary>
    /// Gets the optional <see cref="Error"/> instance that caused the current error.
    /// </summary>
    public Error? InnerError { get; init; }

    internal static Error DefaultError => _defaultError.Value;

    /// <summary>
    /// Converts the specified <see cref="Error"/> into an <see cref="ErrorException"/>.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> to convert.</param>
    [return: NotNullIfNotNull(nameof(error))]
    public static implicit operator ErrorException?(Error? error)
    {
        if (error is null)
            return null;

        return new ErrorException(error);
    }

    /// <summary>
    /// Converts the specified <see cref="Exception"/> into an <see cref="Error"/>.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to convert.</param>
    [return: NotNullIfNotNull(nameof(exception))]
    public static implicit operator Error?(Exception? exception)
    {
        if (exception is null)
            return null;

        return FromException(exception, null, null);
    }

    /// <summary>
    /// Creates an <see cref="Error"/> object from the specified <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The exception to create the error from.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.CaughtException"/>.</param>
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static Error FromException(
        Exception exception,
        string? identifier = null,
        int? errorCode = ErrorCodes.CaughtException)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        if (exception is ErrorException errorException)
            return errorException.OriginalError;

        return CreateError(exception, errorCode, identifier);
    }

    /// <summary>
    /// Gets the extension property with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the extension property.</typeparam>
    /// <param name="key">The name of the extension property.</param>
    /// <param name="options">JSON serialization options used to deserialize to the desired type when the actual value is a
    ///     <see cref="JsonElement"/>.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found
    ///     and is of a valid type; otherwise, the default value for the type of the value parameter. This parameter is passed
    ///     uninitialized.</param>
    /// <returns><see langword="true"/> if the <see cref="Error"/> contains an extension property with the specified key that is
    ///     of type <typeparamref name="T"/> or convertible to type <typeparamref name="T"/>; otherwise, <see langword="false"/>.
    ///     </returns>
    public bool TryGet<T>(string key, JsonSerializerOptions? options, [NotNullWhen(true)] out T? value)
    {
        if (_extensions.TryGetValue(key, out var obj) && obj != null)
        {
            if (obj is T t)
            {
                value = t;
                return true;
            }

            if (obj is JsonElement jsonElement)
            {
                value = jsonElement.Deserialize<T>(options);
                return value != null;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(obj.GetType()))
            {
                value = (T?)converter.ConvertFrom(obj);
                return value != null;
            }

            converter = TypeDescriptor.GetConverter(obj);
            if (converter.CanConvertTo(typeof(T)))
            {
                value = (T?)converter.ConvertTo(obj, typeof(T));
                return value != null;
            }

            try
            {
                value = (T?)Convert.ChangeType(obj, typeof(T));
                return value != null;
            }
            catch
            {
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Gets the extension property with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the extension property.</typeparam>
    /// <param name="key">The name of the extension property.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found
    ///     and is of a valid type; otherwise, the default value for the type of the value parameter. This parameter is passed
    ///     uninitialized.</param>
    /// <returns><see langword="true"/> if the <see cref="Error"/> contains an extension property with the specified key that is
    ///     of type <typeparamref name="T"/> or convertible to type <typeparamref name="T"/>; otherwise, <see langword="false"/>.
    ///     </returns>
    public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value) =>
        TryGet(key, null, out value);

    /// <summary>
    /// Returns a string that represents the current error.
    /// </summary>
    /// <returns>A string that represents the current error.</returns>
    public sealed override string ToString()
    {
        var sb = new StringBuilder();
        AppendError(sb, this, null);
        return sb.ToString();
    }

    internal static Error FromExceptionThrownInCallback(Exception ex, string callbackName) =>
        new()
        {
            Message = string.Format(_messageFormatForExceptionThrownInCallback, callbackName),
            ErrorCode = ErrorCodes.CaughtException,
            InnerError = ex,
        };

    /// <summary>
    /// Gets the full name of the type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The full name of the type.</returns>
    protected static string GetTypeFullName(Type type)
    {
        if (!string.IsNullOrEmpty(type.FullName))
            return type.FullName;
        else if (!string.IsNullOrEmpty(type.Namespace))
            return $"{type.Namespace}.{type.Name}";
        else
            return type.Name;
    }

    private static Error CreateError(Exception exception, int? errorCode = null, string? identifier = null)
    {
        var exceptionType = exception.GetType();
        var properties = _propertiesByExceptionType.GetOrAdd(exceptionType, GetPropertiesForExceptionType);
        var extensions =
            properties
                .Select(p => new { p.Name, Value = FormatValue(p.GetValue(exception)), FullName = GetPropertyFullName(p) })
                .Where(p => p.Value is not null)
                .OrderBy(p => p.Name)
                .ToDictionary(p => p.FullName, p => (object)p.Value!);

        extensions[_originalExceptionTypeExtensionName] = GetTypeFullName(exceptionType);

        var dataEntries = exception.Data.OfType<DictionaryEntry>()
            .Select(x => new { x.Key, Value = FormatValue(x.Value) })
            .Where(x => x.Value is not null);
        foreach (var dataEntry in dataEntries)
            extensions[$"System.Exception.Data.{dataEntry.Key}"] = dataEntry.Value!;

        Error? innerError = null;
        if (exception.InnerException != null)
            innerError = CreateError(exception.InnerException);

        return new Error
        {
            Message = exception.Message,
            Title = exceptionType.Name,
            ErrorCode = errorCode,
            Identifier = identifier,
            Extensions = new ReadOnlyDictionary<string, object>(extensions),
            InnerError = innerError,
        };

        static string GetPropertyFullName(Property property)
        {
            var prefix = property.DeclaringType is null ? null : GetTypeFullName(property.DeclaringType) + ".";
            return prefix + property.Name;
        }
    }

    [return: NotNullIfNotNull(nameof(value))]
    private static string? FormatValue(object? value)
    {
        if (value is null)
            return null;
        if (value is DateTime dateTime)
            return dateTime.ToString("O");
        if (value is DateTimeOffset dateTimeOffset)
            return dateTimeOffset.ToString("O");
        if (value is bool b)
            return b ? "true" : "false";
        return value.ToString()!;
    }

    private static IEnumerable<Property> GetPropertiesForExceptionType(Type exceptionType)
    {
        var properties = exceptionType.GetProperties()
            .Where(p => p.Name switch
            {
                nameof(Exception.TargetSite) => false,
                nameof(Exception.Message) => false,
                nameof(Exception.Data) => false,
                nameof(Exception.InnerException) => false,
                _ => !typeof(Exception).IsAssignableFrom(p.PropertyType) && !typeof(IEnumerable<Exception>).IsAssignableFrom(p.PropertyType),
            })
            .Select(Property.Create)
            .ToList();
        return properties;
    }

    private static string GetTypeNameAsSentenceCase(Type type) =>
        _defaultTitleCache.GetOrAdd(type, t => Format.AsSentenceCase(t.Name));

    private static void AppendError(StringBuilder sb, Error error, string? indention)
    {
        AppendSummary(sb, error, indention);

        for (var innerError = error.InnerError; innerError is not null; innerError = innerError.InnerError)
        {
            sb.Append(" ---> ");
            AppendSummary(sb, innerError, indention is null ? "      " : indention);
        }

        foreach (var extensionProperty in error.Extensions)
        {
            if (extensionProperty.Value is Error propertyError)
            {
                sb.Append(" ---> ").Append(extensionProperty.Key).Append(": ");
                AppendError(sb, propertyError, indention is null ? "      " : indention);
            }
            else if (extensionProperty.Value is IEnumerable<Error> propertyErrors)
            {
                foreach (var x in propertyErrors.Select((e, i) => new { e, i }))
                {
                    sb.Append(" ---> ").Append(extensionProperty.Key).Append('[').Append(x.i).Append("]: ");
                    AppendError(sb, x.e, indention is null ? "      " : indention);
                }
            }
        }
    }

    private static void AppendSummary(StringBuilder sb, Error error, string? indention)
    {
        sb.Append(error.Title).Append(": ").AppendLine(Indent(error.Message, indention));

        if (error.ErrorCode.HasValue)
            sb.Append(indention + "   ").Append("Error Code: ").AppendLine(ErrorCodes.GetDescription(error.ErrorCode.Value));

        if (error.Identifier is not null)
            sb.Append(indention + "   ").Append("Identifier: ").AppendLine(error.Identifier);

        foreach (var extensionProperty in error.Extensions)
        {
            if (extensionProperty.Value is null
                || extensionProperty.Value is Error
                || extensionProperty.Value is IEnumerable<Error>)
            {
                continue;
            }

            var extensionPropertyValue = extensionProperty.Value.ToString() ?? string.Empty;
            if (extensionPropertyValue.Contains('\n'))
                sb.Append(indention + "   ").Append(extensionProperty.Key).AppendLine(":").AppendLine(Indent(extensionPropertyValue, indention + "   ", indention + "   "));
            else
                sb.Append(indention + "   ").Append(extensionProperty.Key).Append(": ").AppendLine(extensionProperty.Value.ToString());
        }
    }

    private static string Indent(string value, string? indention, string? firstLineIndentation = null)
    {
        if (indention is null)
            return value;

        return firstLineIndentation + value.Replace("\n", "\n" + indention);
    }

    private string GetDebuggerDisplay() => $"{Title}: \"{Message}\"";

    private sealed class Property
    {
        private readonly PropertyInfo _propertyInfo;
        private Func<object, object?> _propertyAccessor;

        private Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;

            if (IsHResultProperty)
                _propertyAccessor = instance => $"0x{(int)_propertyInfo.GetValue(instance)!:x}";
            else
                _propertyAccessor = propertyInfo.GetValue;

            ThreadPool.QueueUserWorkItem(_ => SetOptimizedPropertyAccessor());
        }

        public string Name => _propertyInfo.Name;

        public Type? DeclaringType => _propertyInfo.DeclaringType;

        private bool IsHResultProperty =>
            _propertyInfo.Name == nameof(Exception.HResult)
            && _propertyInfo.DeclaringType == typeof(Exception);

        public static Property Create(PropertyInfo propertyInfo) =>
            new(propertyInfo);

        public object? GetValue(object instance) => _propertyAccessor(instance);

        private void SetOptimizedPropertyAccessor()
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");

            Expression body = Expression.Property(
                Expression.Convert(instanceParameter, _propertyInfo.ReflectedType ?? typeof(Exception)),
                _propertyInfo);

            if (IsHResultProperty)
            {
                var toStringMethod = typeof(int).GetMethod(nameof(int.ToString), [typeof(string)])!;
                var concatMethod = typeof(string).GetMethod(nameof(string.Concat), [typeof(string), typeof(string)])!;

                body =
                    Expression.Call(
                        concatMethod,
                        Expression.Constant("0x", typeof(string)),
                        Expression.Call(body, toStringMethod, Expression.Constant("x", typeof(string))));
            }
            else if (_propertyInfo.PropertyType.IsValueType)
            {
                body = Expression.Convert(body, typeof(object));
            }

            var lambda = Expression.Lambda<Func<object, object?>>(body, instanceParameter);

            _propertyAccessor = lambda.Compile();
        }
    }
}

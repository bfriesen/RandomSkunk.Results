using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;

namespace RandomSkunk.Results;

/// <summary>
/// Defines an error.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
[JsonConverter(typeof(ErrorJsonConverter))]
public record class Error
{
    internal const string DefaultMessage = "An error occurred.";
    internal const string DefaultFromExceptionMessage = "An exception was thrown.";

    private static readonly ConcurrentDictionary<Type, IEnumerable<Property>> _propertiesByExceptionType = new();
    private static readonly Lazy<Error> _defaultError = new(() => new Error());
    private static readonly IReadOnlyDictionary<string, object> _emptyExtensions = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

    private readonly string _title;
    private readonly string _message;
    private readonly string? _identifier;
    private readonly string? _stackTrace;
    private readonly IReadOnlyDictionary<string, object> _extensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message">The error message. If <see langword="null"/>, then a default message is used instead.</param>
    /// <param name="title">The title for the error. If <see langword="null"/>, then the name of the error type is used instead.
    ///     </param>
    /// <param name="extensions">Any additional properties for the error.</param>
    public Error(
        string? message = null,
        string? title = null,
        IReadOnlyDictionary<string, object>? extensions = null)
    {
        _title = string.IsNullOrWhiteSpace(title) ? Format.AsSentenceCase(GetType().Name) : title!;
        _message = string.IsNullOrWhiteSpace(message) ? DefaultMessage : message!;
        _extensions = extensions ?? _emptyExtensions;
    }

    /// <summary>
    /// Gets the title for the error.
    /// </summary>
    public string Title
    {
        get => _title;
        init => _title = string.IsNullOrWhiteSpace(value) ? Format.AsSentenceCase(GetType().Name) : value;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message
    {
        get => _message;
        init => _message = string.IsNullOrWhiteSpace(value) ? _message : value;
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
        init => _identifier = string.IsNullOrWhiteSpace(value) ? _identifier : value;
    }

    /// <summary>
    /// Gets a value indicating whether the current error contains sensitive information.
    /// </summary>
    /// <remarks>
    /// This value is used to determines whether the <see cref="ToString()"/> method outputs the full or abbreviated
    /// representation of the error.
    /// </remarks>
    public bool IsSensitive { get; init; }

    /// <summary>
    /// Gets the optional stack trace.
    /// </summary>
    public string? StackTrace
    {
        get => _stackTrace;
        init => _stackTrace = string.IsNullOrWhiteSpace(value) ? _stackTrace : value;
    }

    /// <summary>
    /// Gets additional properties for the error.
    /// </summary>
    public IReadOnlyDictionary<string, object> Extensions
    {
        get => _extensions;
        init => _extensions = value ?? _emptyExtensions;
    }

    /// <summary>
    /// Gets the optional <see cref="Error"/> instance that caused the current error.
    /// </summary>
    public Error? InnerError { get; init; }

    internal static Error DefaultError => _defaultError.Value;

    /// <summary>
    /// Creates an <see cref="Error"/> object from the specified <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The exception to create the error from.</param>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.CaughtException"/>.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="title">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.</param>
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static Error FromException(
        Exception exception,
        string message = DefaultFromExceptionMessage,
        int? errorCode = ErrorCodes.CaughtException,
        string? identifier = null,
        string? title = null)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var innerError = CreateInnerError(exception);

        return new Error(message ?? DefaultFromExceptionMessage, title)
        {
            ErrorCode = errorCode,
            Identifier = identifier,
            InnerError = innerError,
        };
    }

    private static Error CreateInnerError(Exception exception)
    {
        Error? innerError = null;
        if (exception.InnerException != null)
            innerError = CreateInnerError(exception.InnerException);

        int? errorCode = null;
        if (exception is ExternalException externalException)
            errorCode = externalException.ErrorCode;

        var properties = _propertiesByExceptionType.GetOrAdd(exception.GetType(), GetPropertiesForExceptionType);
        var extensions = new ReadOnlyDictionary<string, object>(
            properties
                .Select(p => new { p.Name, Value = p.GetValue(exception) })
                .Where(p => p.Value is not null && (p.Name != nameof(ExternalException.ErrorCode) || !errorCode.HasValue))
                .ToDictionary(p => p.Name, p => p.Value!));

        return new Error(exception.Message, exception.GetType().Name, extensions)
        {
            StackTrace = exception.StackTrace,
            ErrorCode = errorCode,
            InnerError = innerError,
        };
    }

    private static IEnumerable<Property> GetPropertiesForExceptionType(Type type)
    {
        var properties = type.GetProperties()
            .Where(p => p.Name switch
            {
                nameof(Exception.TargetSite) => false,
                nameof(Exception.Message) => false,
                nameof(Exception.Data) => false,
                nameof(Exception.Source) => false,
                nameof(Exception.StackTrace) => false,
                nameof(Exception.InnerException) => false,
                _ => !typeof(Exception).IsAssignableFrom(p.PropertyType) && !typeof(IEnumerable<Exception>).IsAssignableFrom(p.PropertyType),
            })
            .Select(Property.Create)
            .ToList();
        return properties;
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
            else
            {
                converter = TypeDescriptor.GetConverter(obj);
                if (converter.CanConvertTo(typeof(T)))
                {
                    value = (T?)converter.ConvertTo(obj, typeof(T));
                    return value != null;
                }
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
    /// <remarks>
    /// If the <see cref="IsSensitive"/> is <see langword="true"/>, then a simplified value containing only the
    /// <see cref="Title"/> and <see cref="ErrorCode"/> (if present) is returned. Otherwise, the full representation is returned.
    /// </remarks>
    /// <returns>A string that represents the current error.</returns>
    public sealed override string ToString()
    {
        if (IsSensitive)
            return ToStringAbbreviated();

        return ToStringFull();
    }

    private string ToStringAbbreviated()
    {
        var sb = new StringBuilder();

        if (ErrorCode.HasValue)
        {
            if (ErrorCodes.TryGetDescription(ErrorCode.Value, out var description))
            {
                if (description == $"{ErrorCode} ({Title})")
                    sb.Append(description);
                else
                    sb.Append($"{Title}: {description}");
            }
            else
            {
                sb.Append($"{Title}: {ErrorCode}");
            }
        }

        sb.Append(Title);

        if (!string.IsNullOrWhiteSpace(Identifier))
        {
            sb.Append(" - ").Append(Identifier);
        }

        return sb.ToString();
    }

    private string ToStringFull()
    {
        var sb = new StringBuilder();

        AppendError(sb, this, null);

        while (sb.Length > 0 && char.IsWhiteSpace(sb[^1]))
            sb.Length--;

        return sb.ToString();
    }

    private static void AppendError(StringBuilder sb, Error error, string? indention)
    {
        AppendSummary(sb, error, indention);

        for (var innerError = error.InnerError; innerError is not null; innerError = innerError.InnerError)
        {
            sb.Append(" ---> ");
            AppendSummary(sb, innerError, indention is null ? "      " : indention);
        }

        var first = true;
        for (var e = error; e is not null; e = e.InnerError)
        {
            if (first) first = false;
            else sb.AppendLine("   --- End of inner error stack trace ---");

            if (!string.IsNullOrWhiteSpace(e.StackTrace))
                sb.AppendLine(e.StackTrace);
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

            sb.Append(indention + "   ").Append(extensionProperty.Key).Append(": ").AppendLine(extensionProperty.Value.ToString());
        }
    }

    /// <summary>
    /// Gets an extensions dictionary containing the specified items. If an item value is <see langword="null"/>, the item is not
    /// added to the dictionary.
    /// </summary>
    /// <param name="extensions">A collection of extension items. If an item value is <see langword="null"/>, the item is not
    ///     added to the dictionary.</param>
    /// <returns>An extensions dictionary.</returns>
    protected static IReadOnlyDictionary<string, object> GetExtensions(params (string Key, object Value)[] extensions) =>
        new ReadOnlyDictionary<string, object>(extensions.Where(x => x.Value is not null).ToDictionary(x => x.Key, x => x.Value));

    /// <summary>
    /// Gets an extensions dictionary containing a single item with the specified key and value. If <paramref name="value"/> is
    /// <see langword="null"/>, an item is not added to the dictionary.
    /// </summary>
    /// <param name="key">The item key.</param>
    /// <param name="value">The item value. If <see langword="null"/>, an item is not added to the dictionary.</param>
    /// <returns>An extensions dictionary.</returns>
    protected static IReadOnlyDictionary<string, object> GetExtensions(string key, object value) =>
        GetExtensions((key, value));

    private static string Indent(string value, string? indention, string? firstLineIndentation = null)
    {
        if (indention is null)
            return value;

        return firstLineIndentation + value.Replace("\n", "\n" + indention);
    }

    private string GetDebuggerDisplay() => $"{Title}: \"{Message}\"";

    private class Property
    {
        private readonly PropertyInfo _propertyInfo;
        private Func<object, object?> _propertyAccessor;

        protected Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;

            if (IsHResultProperty)
                _propertyAccessor = instance => $"0x{(int)_propertyInfo.GetValue(instance)!:x}";
            else
                _propertyAccessor = propertyInfo.GetValue;

            ThreadPool.QueueUserWorkItem(_ => SetOptimizedPropertyAccessor());
        }

        public string Name => _propertyInfo.Name;

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
                var toStringMethod = typeof(int).GetMethod(nameof(int.ToString), new[] { typeof(string) })!;
                var concatMethod = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) })!;

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

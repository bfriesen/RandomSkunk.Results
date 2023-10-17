namespace RandomSkunk.Results.Dapper;

/// <summary>
/// Defines Dapper extensions that return result values.
/// </summary>
public static partial class ResultSqlMapper
{
    /// <summary>
    /// Execute parameterized SQL.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The number of rows affected.</returns>
    public static Result<int> TryExecute(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Execute(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Result<T> TryExecuteScalar<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute parameterized SQL and return an <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    /// <remarks>
    /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a
    /// <see cref="DataTable"/> or <see cref="T:DataSet"/>.
    /// </remarks>
    public static Result<IDataReader> TryExecuteReader(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Executes a query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="buffered">Whether to buffer results in memory.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the
    ///     first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name mapping is
    ///     assumed (case insensitive).</returns>
    public static Result<IEnumerable<T>> TryQuery<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
    ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
    ///     column-name===member-name mapping is assumed (case insensitive).</returns>
    public static Result<T> TryQueryFirst<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
    ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
    ///     column-name===member-name mapping is assumed (case insensitive).</returns>
    public static Result<T> TryQueryFirstOrNone<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null)
        where T : class =>
        TryCatch.AsResult(
            () => cnn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
    ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
    ///     column-name===member-name mapping is assumed (case insensitive).</returns>
    public static Result<T> TryQuerySingle<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QuerySingle<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
    ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
    ///     column-name===member-name mapping is assumed (case insensitive).</returns>
    public static Result<T> TryQuerySingleOrNone<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null)
        where T : class =>
        TryCatch.AsResult(
            () => cnn.QuerySingleOrDefault<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A <see cref="GridReader"/> for reading the multiple queries.</returns>
    public static Result<GridReader> TryQueryMultiple(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => new GridReader(cnn.QueryMultiple(sql, param, transaction, commandTimeout, commandType)),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with 2 input types. This returns a single type, combined from the raw types via
    /// <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TFirst, TSecond, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with 3 input types. This returns a single type, combined from the raw types via
    /// <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TFirst, TSecond, TThird, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with 4 input types. This returns a single type, combined from the raw types via
    /// <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TFirst, TSecond, TThird, TFourth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with 5 input types. This returns a single type, combined from the raw types via
    /// <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with 6 input types. This returns a single type, combined from the raw types via
    /// <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with 7 input types. If you need more types -> use Query with Type[] parameter. This returns
    /// a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform a multi-mapping query with an arbitrary number of input types. This returns a single type, combined from the raw
    /// types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="types">Array of types in the recordset.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Result<IEnumerable<TReturn>> TryQuery<TReturn>(
        this IDbConnection cnn,
        string sql,
        Type[] types,
        Func<object[], TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.Query(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A sequence of data of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from
    ///     the first column in assumed, otherwise an instance is created per row, and a direct column-name===member-name mapping
    ///     is assumed (case insensitive).</returns>
    public static Task<Result<IEnumerable<T>>> TryQueryAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The first result.</returns>
    public static Task<Result<T>> TryQueryFirstAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The first result or <c>None</c>.</returns>
    public static Task<Result<T>> TryQueryFirstOrNoneAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null)
        where T : class =>
        TryCatch.AsResult(
            () => cnn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The single result.</returns>
    public static Task<Result<T>> TryQuerySingleAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The single result or <c>None</c>.</returns>
    public static Task<Result<T>> TryQuerySingleOrNoneAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null)
        where T : class =>
        TryCatch.AsResult(
            () => cnn.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a command asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The number of rows affected.</returns>
    public static Task<Result<int>> TryExecuteAsync(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 2 input types. This returns a single type, combined from the raw types
    /// via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TFirst, TSecond, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 3 input types. This returns a single type, combined from the raw types
    /// via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TFirst, TSecond, TThird, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 4 input types. This returns a single type, combined from the raw types
    /// via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 5 input types. This returns a single type, combined from the raw types
    /// via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 6 input types. This returns a single type, combined from the raw types
    /// via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 7 input types. This returns a single type, combined from the raw types
    /// via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with an arbitrary number of input types. This returns a single type, combined
    /// from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="types">Array of types in the recordset.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static Task<Result<IEnumerable<TReturn>>> TryQueryAsync<TReturn>(
        this IDbConnection cnn,
        string sql,
        Type[] types,
        Func<object[], TReturn> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.QueryAsync(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A <see cref="GridReader"/> for reading the multiple queries.</returns>
    public static Task<Result<GridReader>> TryQueryMultipleAsync(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            async () => new GridReader(await cnn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType).ConfigureAwait(ContinueOnCapturedContext)),
            exceptionHandler);

    /// <summary>
    /// Execute parameterized SQL and return an <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    /// <remarks>
    /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a
    /// <see cref="DataTable"/> or <see cref="T:DataSet"/>.
    /// </remarks>
    public static Task<Result<IDataReader>> TryExecuteReaderAsync(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute parameterized SQL and return a <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>An <see cref="DbDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<Result<DbDataReader>> TryExecuteReaderAsync(
        this DbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Whether it is a stored proc or a batch.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<Result<T>> TryExecuteScalarAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        Func<Exception, Error>? exceptionHandler = null) =>
        TryCatch.AsResult(
            () => cnn.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType),
            exceptionHandler);
}

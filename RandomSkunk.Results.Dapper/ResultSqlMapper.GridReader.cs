namespace RandomSkunk.Results.Dapper;

/// <content> Defines the <c>ResultSqlMapper.GridReader</c> type. </content>
public static partial class ResultSqlMapper
{
    /// <summary>
    /// The grid reader provides interfaces for reading multiple result sets from a Dapper query.
    /// </summary>
    public class GridReader : IDisposable
    {
        private readonly SqlMapper.GridReader _gridReader;

        internal GridReader(SqlMapper.GridReader gridReader) =>
            _gridReader = gridReader;

        /// <summary>
        /// Gets a value indicating whether the underlying reader has been consumed.
        /// </summary>
        public bool IsConsumed =>
            _gridReader.IsConsumed;

        /// <summary>
        /// Gets the command associated with the reader.
        /// </summary>
        public IDbCommand Command =>
            _gridReader.Command;

        /// <summary>
        /// Dispose the grid, closing and disposing both the underlying reader and command.
        /// </summary>
        public void Dispose() =>
            _gridReader.Dispose();

        /// <summary>
        /// Read the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <param name="buffered">Whether the results should be buffered in memory.</param>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from
        ///     the first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name
        ///     mapping is assumed (case insensitive).</returns>
        public Result<IEnumerable<T>> TryRead<T>(Func<Exception, Error> exceptionHandler, bool buffered = true) =>
            TryCatch.AsResult(() => _gridReader.Read<T>(buffered), exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buffered">Whether the results should be buffered in memory.</param>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from
        ///     the first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name
        ///     mapping is assumed (case insensitive).</returns>
        public Result<IEnumerable<T>> TryRead<T>(bool buffered = true, string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway) =>
            TryRead<T>(ex => GetDapperError(ex, errorCode, errorIdentifier), buffered);

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadFirst<T>(Func<Exception, Error> exceptionHandler) =>
            TryCatch.AsResult(_gridReader.ReadFirst<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadFirst<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway) =>
            TryReadFirst<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadFirstOrNone<T>(Func<Exception, Error> exceptionHandler)
            where T : class =>
            TryCatch.AsResult(_gridReader.ReadFirstOrDefault<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadFirstOrNone<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway)
            where T : class =>
            TryReadFirstOrNone<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadSingle<T>(Func<Exception, Error> exceptionHandler) =>
            TryCatch.AsResult(_gridReader.ReadSingle<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadSingle<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway) =>
            TryReadSingle<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadSingleOrNone<T>(Func<Exception, Error> exceptionHandler)
            where T : class =>
            TryCatch.AsResult(_gridReader.ReadSingleOrDefault<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadSingleOrNone<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway)
            where T : class =>
            TryReadSingleOrNone<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <param name="buffered">Whether the results should be buffered in memory.</param>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from
        ///     the first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name
        ///     mapping is assumed (case insensitive).</returns>
        public Task<Result<IEnumerable<T>>> TryReadAsync<T>(Func<Exception, Error> exceptionHandler, bool buffered = true) =>
            TryCatch.AsResult(() => _gridReader.ReadAsync<T>(buffered), exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buffered">Whether the results should be buffered in memory.</param>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from
        ///     the first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name
        ///     mapping is assumed (case insensitive).</returns>
        public Task<Result<IEnumerable<T>>> TryReadAsync<T>(bool buffered = true, string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway) =>
            TryReadAsync<T>(ex => GetDapperError(ex, errorCode, errorIdentifier), buffered);

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadFirstAsync<T>(Func<Exception, Error> exceptionHandler) =>
            TryCatch.AsResult(_gridReader.ReadFirstAsync<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadFirstAsync<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway) =>
            TryReadFirstAsync<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadFirstOrNoneAsync<T>(Func<Exception, Error> exceptionHandler)
            where T : class =>
            TryCatch.AsResult(_gridReader.ReadFirstOrDefaultAsync<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadFirstOrNoneAsync<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway)
            where T : class =>
            TryReadFirstOrNoneAsync<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadSingleAsync<T>(Func<Exception, Error> exceptionHandler) =>
            TryCatch.AsResult(_gridReader.ReadSingleAsync<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadSingleAsync<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway) =>
            TryReadSingleAsync<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
        ///     </param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadSingleOrNoneAsync<T>(Func<Exception, Error> exceptionHandler)
            where T : class =>
            TryCatch.AsResult(_gridReader.ReadSingleOrDefaultAsync<T>, exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
        /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadSingleOrNoneAsync<T>(string? errorIdentifier = null, int errorCode = ErrorCodes.BadGateway)
            where T : class =>
            TryReadSingleOrNoneAsync<T>(ex => GetDapperError(ex, errorCode, errorIdentifier));
    }
}

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
        /// <param name="buffered">Whether the results should be buffered in memory.</param>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from
        ///     the first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name
        ///     mapping is assumed (case insensitive).</returns>
        public Result<IEnumerable<T>> TryRead<T>(bool buffered = true) =>
            TryCatch.AsResult(() => _gridReader.Read<T>(buffered));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadFirst<T>() =>
            TryCatch.AsResult(() => _gridReader.ReadFirst<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadFirstOrDefault<T>()
            where T : struct =>
            TryCatch.AsResult(() => _gridReader.ReadFirstOrDefault<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Maybe<T> TryReadFirstOrNone<T>()
            where T : class =>
            TryCatch.AsMaybe(() => _gridReader.ReadFirstOrDefault<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadSingle<T>() =>
            TryCatch.AsResult(() => _gridReader.ReadSingle<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Result<T> TryReadSingleOrDefault<T>()
            where T : struct =>
            TryCatch.AsResult(() => _gridReader.ReadSingleOrDefault<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Maybe<T> TryReadSingleOrNone<T>()
            where T : class =>
            TryCatch.AsMaybe(() => _gridReader.ReadSingleOrDefault<T>());

        /// <summary>
        /// Read the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buffered">Whether the results should be buffered in memory.</param>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from
        ///     the first column is assumed, otherwise an instance is created per row, and a direct column-name===member-name
        ///     mapping is assumed (case insensitive).</returns>
        public Task<Result<IEnumerable<T>>> TryReadAsync<T>(bool buffered = true) =>
            TryCatch.AsResultAsync(() => _gridReader.ReadAsync<T>(buffered));

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadFirstAsync<T>() =>
            TryCatch.AsResultAsync(() => _gridReader.ReadFirstAsync<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadFirstOrDefaultAsync<T>()
            where T : struct =>
            TryCatch.AsResultAsync(() => _gridReader.ReadFirstOrDefaultAsync<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The first of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the
        ///     data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Maybe<T>> TryReadFirstOrNoneAsync<T>()
            where T : class =>
            TryCatch.AsMaybeAsync(() => _gridReader.ReadFirstOrDefaultAsync<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadSingleAsync<T>() =>
            TryCatch.AsResultAsync(() => _gridReader.ReadSingleAsync<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Result<T>> TryReadSingleOrDefaultAsync<T>()
            where T : struct =>
            TryCatch.AsResultAsync(() => _gridReader.ReadSingleOrDefaultAsync<T>());

        /// <summary>
        /// Read an individual row of the next grid of results.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>The single element of a sequence of data of the supplied type; if a basic type (int, string, etc) is queried
        ///     then the data from the first column in assumed, otherwise an instance is created per row, and a direct
        ///     column-name===member-name mapping is assumed (case insensitive).</returns>
        public Task<Maybe<T>> TryReadSingleOrNoneAsync<T>()
            where T : class =>
            TryCatch.AsMaybeAsync(() => _gridReader.ReadSingleOrDefaultAsync<T>());
    }
}

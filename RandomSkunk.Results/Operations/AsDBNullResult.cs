namespace RandomSkunk.Results;

/// <content> Defines the <c>AsDBNullResult</c> method. </content>
public partial struct Result
{
    /// <summary>
    /// Converts this <see cref="Result"/> to an equivalent <see cref="Result{T}"/> of type <see cref="DBNull"/>. If this is a
    /// <c>Success</c> result, then a <c>Success</c> result with a non-null <see cref="DBNull"/> value is returned. Otherwise, if
    /// this is a <c>Fail</c> result, then a <c>Fail</c> result with the same error is returned.
    /// </summary>
    /// <remarks>
    /// This method exists to make it possible for <see cref="Result"/> objects to participate in LINQ-to-Results queries. For
    /// example:
    /// <code>
    /// Result resultIn = Result.Success();
    ///
    /// Maybe&lt;double&gt; resultOut =
    ///     from intValue in Result&lt;int&gt;.Success(2)
    ///     from dbNullValue in resultIn.AsDBNullResult()
    ///     from doubleValue in Maybe&lt;double&gt;.Success(2.5)
    ///     select intValue * doubleValue;
    /// </code>
    /// In this example, <c>resultOut</c> will be <c>Success</c> with a value of <c>5.0</c>. If <c>resultIn</c> had been
    /// <c>Fail</c>, then <c>resultOut</c> would have been <c>Fail</c> with the same error.
    /// <para>
    /// Note that the <see cref="DBNull"/> value in a LINQ-Results query is not very useful in and of itself. What is useful,
    /// however, is being able to call methods that have side effects (i.e. return <see cref="Result"/>) from the middle of a
    /// LINQ-to-Results query. This example shows what that might look like:
    /// </para>
    /// <code>
    /// Maybe&lt;double&gt; resultOut =
    ///     from intValue in MethodReturningResultOfInt()
    ///     from dbNullValue in MethodReturningResult().AsDBNullResult()
    ///     from doubleValue in MethodReturningMaybeOfDouble()
    ///     select intValue * doubleValue;
    /// </code>
    /// </remarks>
    /// <returns>The equivalent <see cref="Result{T}"/> of type <see cref="DBNull"/>.</returns>
    public Result<DBNull> AsDBNullResult() =>
        FlatMap(() => Result<DBNull>.Success(DBNull.Value));
}

/// <content> Defines the <c>AsDBNullResult</c> extension method. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Converts this <see cref="Result"/> to an equivalent <see cref="Result{T}"/> of type <see cref="DBNull"/>. If this is a
    /// <c>Success</c> result, then a <c>Success</c> result with a non-null <see cref="DBNull"/> value is returned. Otherwise, if
    /// this is a <c>Fail</c> result, then a <c>Fail</c> result with the same error is returned.
    /// </summary>
    /// <remarks>
    /// This method exists to make it possible for <see cref="Result"/> objects to participate in LINQ-to-Results queries. For
    /// example:
    /// <code>
    /// Result resultIn = Result.Success();
    ///
    /// Maybe&lt;double&gt; resultOut =
    ///     from intValue in Result&lt;int&gt;.Success(2)
    ///     from dbNullValue in resultIn.AsDBNullResult()
    ///     from doubleValue in Maybe&lt;double&gt;.Success(2.5)
    ///     select intValue * doubleValue;
    /// </code>
    /// In this example, <c>resultOut</c> will be <c>Success</c> with a value of <c>5.0</c>. If <c>resultIn</c> had been
    /// <c>Fail</c>, then <c>resultOut</c> would have been <c>Fail</c> with the same error.
    /// <para>
    /// Note that the <see cref="DBNull"/> value in a LINQ-Results query is not very useful in and of itself. What is useful,
    /// however, is being able to call methods that have side effects (i.e. return <see cref="Result"/>) from the middle of a
    /// LINQ-to-Results query. This example shows what that might look like:
    /// </para>
    /// <code>
    /// Maybe&lt;double&gt; resultOut =
    ///     from intValue in MethodReturningResultOfInt()
    ///     from dbNullValue in MethodReturningResult().AsDBNullResult()
    ///     from doubleValue in MethodReturningMaybeOfDouble()
    ///     select intValue * doubleValue;
    /// </code>
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/> of type <see cref="DBNull"/>.</returns>
    public static async Task<Result<DBNull>> AsDBNullResult(this Task<Result> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).AsDBNullResult();
}

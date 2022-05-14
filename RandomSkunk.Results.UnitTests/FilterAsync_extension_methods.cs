namespace RandomSkunk.Results.UnitTests;

public class FilterAsync_extension_methods
{
    [Fact]
    public async Task Given_cancellation_token_When_IsSome_and_function_returns_true_Returns_source()
    {
        var source = Maybe<int>.Create.Some(1);

        var actual = await source.FilterAsync((value, cancellationToken) => Task.FromResult(value == 1), default);

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_cancellation_token_When_IsSome_and_function_returns_false_Returns_None()
    {
        var source = Maybe<int>.Create.Some(1);

        var actual = await source.FilterAsync((value, cancellationToken) => Task.FromResult(value == 2), default);

        actual.Should().Be(Maybe<int>.Create.None());
    }

    [Fact]
    public async Task Given_cancellation_token_When_IsFail_Returns_source()
    {
        var source = Maybe<int>.Create.Fail();

        var actual = await source.FilterAsync((value, cancellationToken) => Task.FromResult(value == 1), default);

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_cancellation_token_When_IsNone_Returns_source()
    {
        var source = Maybe<int>.Create.None();

        var actual = await source.FilterAsync((value, cancellationToken) => Task.FromResult(value == 1), default);

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_cancellation_token_and_null_filter_function_Throws_ArgumentNullException()
    {
        var source = Maybe<int>.Create.Fail();

        Func<Task> act = () => source.FilterAsync(null!, default);

        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_no_cancellation_token_When_IsSome_and_function_returns_true_Returns_source()
    {
        var source = Maybe<int>.Create.Some(1);

        var actual = await source.FilterAsync(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_no_cancellation_token_When_IsSome_and_function_returns_false_Returns_None()
    {
        var source = Maybe<int>.Create.Some(1);

        var actual = await source.FilterAsync(value => Task.FromResult(value == 2));

        actual.Should().Be(Maybe<int>.Create.None());
    }

    [Fact]
    public async Task Given_no_cancellation_token_When_IsFail_Returns_source()
    {
        var source = Maybe<int>.Create.Fail();

        var actual = await source.FilterAsync(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_no_cancellation_token_When_IsNone_Returns_source()
    {
        var source = Maybe<int>.Create.None();

        var actual = await source.FilterAsync(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_no_cancellation_token_and_null_filter_function_Throws_ArgumentNullException()
    {
        var source = Maybe<int>.Create.Fail();

        Func<Task> act = () => source.FilterAsync(null!);

        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}

namespace RandomSkunk.Results.UnitTests;

public class FilterAsync_methods
{
    [Fact]
    public async Task When_IsSome_and_function_returns_true_Returns_source()
    {
        var source = 1.ToMaybe();

        var actual = await source.FilterAsync(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task When_IsSome_and_function_returns_false_Returns_None()
    {
        var source = 1.ToMaybe();

        var actual = await source.FilterAsync(value => Task.FromResult(value == 2));

        actual.Should().Be(Maybe<int>.Create.None());
    }

    [Fact]
    public async Task When_IsFail_Returns_source()
    {
        var source = Maybe<int>.Create.Fail();

        var actual = await source.FilterAsync(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task When_IsNone_Returns_source()
    {
        var source = Maybe<int>.Create.None();

        var actual = await source.FilterAsync(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_null_filter_function_Throws_ArgumentNullException()
    {
        var source = Maybe<int>.Create.Fail();

        Func<Task> act = () => source.FilterAsync(null!);

        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
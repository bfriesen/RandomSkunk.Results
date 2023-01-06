namespace RandomSkunk.Results.UnitTests;

public class Where_Async_methods
{
    [Fact]
    public async Task When_IsSuccess_and_function_returns_true_Returns_source()
    {
        var source = 1.ToMaybe();

        var actual = await source.Where(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task When_IsSuccess_and_function_returns_false_Returns_None()
    {
        var source = 1.ToMaybe();

        var actual = await source.Where(value => Task.FromResult(value == 2));

        actual.Should().Be(Maybe<int>.None);
    }

    [Fact]
    public async Task When_IsFail_Returns_source()
    {
        var source = Maybe<int>.Fail();

        var actual = await source.Where(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task When_IsNone_Returns_source()
    {
        var source = Maybe<int>.None;

        var actual = await source.Where(value => Task.FromResult(value == 1));

        actual.Should().Be(source);
    }

    [Fact]
    public async Task Given_null_filter_function_Throws_ArgumentNullException()
    {
        var source = Maybe<int>.Fail();

        Func<Task> act = () => source.Where((Func<int, Task<bool>>)null!);

        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}

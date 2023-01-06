namespace RandomSkunk.Results.UnitTests;

public class Where_methods
{
    [Fact]
    public void When_IsSuccess_and_function_returns_true_Returns_source()
    {
        var source = 1.ToMaybe();

        var actual = source.Where(value => value == 1);

        actual.Should().Be(source);
    }

    [Fact]
    public void When_IsSuccess_and_function_returns_false_Returns_None()
    {
        var source = 1.ToMaybe();

        var actual = source.Where(value => value == 2);

        actual.Should().Be(Maybe<int>.None);
    }

    [Fact]
    public void When_IsFail_Returns_source()
    {
        var source = Maybe<int>.Fail();

        var actual = source.Where(value => value == 1);

        actual.Should().Be(source);
    }

    [Fact]
    public void When_IsNone_Returns_source()
    {
        var source = Maybe<int>.None;

        var actual = source.Where(value => value == 1);

        actual.Should().Be(source);
    }

    [Fact]
    public void Given_null_filter_function_Throws_ArgumentNullException()
    {
        var source = Maybe<int>.Fail();

        Action act = () => source.Where((Func<int, bool>)null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }
}

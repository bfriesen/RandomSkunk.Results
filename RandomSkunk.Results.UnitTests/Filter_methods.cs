namespace RandomSkunk.Results.UnitTests;

public class Filter_methods
{
    [Fact]
    public void When_IsSome_and_function_returns_true_Returns_source()
    {
        var source = Maybe<int>.Create.Some(1);

        var actual = source.Filter(value => value == 1);

        actual.Should().Be(source);
    }

    [Fact]
    public void When_IsSome_and_function_returns_false_Returns_None()
    {
        var source = Maybe<int>.Create.Some(1);

        var actual = source.Filter(value => value == 2);

        actual.Should().Be(Maybe<int>.Create.None());
    }

    [Fact]
    public void When_IsFail_Returns_source()
    {
        var source = Maybe<int>.Create.Fail();

        var actual = source.Filter(value => value == 1);

        actual.Should().Be(source);
    }

    [Fact]
    public void When_IsNone_Returns_source()
    {
        var source = Maybe<int>.Create.None();

        var actual = source.Filter(value => value == 1);

        actual.Should().Be(source);
    }

    [Fact]
    public void Given_null_filter_function_Throws_ArgumentNullException()
    {
        var source = Maybe<int>.Create.Fail();

        Action act = () => source.Filter(null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }
}

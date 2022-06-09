namespace RandomSkunk.Results.UnitTests;

public class AndAlso_method
{
    [Fact]
    public void When_IsSuccess_Returns_onSuccess_evaluation()
    {
        var source = Result.Success();
        var otherError = new Error();

        var actual = source.AndAlso(() => Result.Fail(otherError));

        actual.IsFail.Should().BeTrue();
        actual.GetError().Should().BeSameAs(otherError);
    }

    [Fact]
    public void When_IsFail_Returns_Fail()
    {
        var sourceError = new Error();
        var source = Result.Fail(sourceError);

        var actual = source.AndAlso(() => Result.Success());

        actual.IsFail.Should().BeTrue();
        actual.GetError().Should().BeSameAs(sourceError);
    }

    [Fact]
    public void When_IsFail_Given_onFail_parameter_Returns_Fail()
    {
        var sourceError = new Error();
        var source = Result.Fail(sourceError);
        var otherError = new Error();

        var actual = source.AndAlso(() => Result.Success(), error => otherError);

        actual.IsFail.Should().BeTrue();
        actual.GetError().Should().BeSameAs(otherError);
    }

    [Fact]
    public void Given_null_onSuccess_function_Throws_ArgumentNullException()
    {
        var source = Result.Success();

        Action act = () => source.AndAlso(null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }
}

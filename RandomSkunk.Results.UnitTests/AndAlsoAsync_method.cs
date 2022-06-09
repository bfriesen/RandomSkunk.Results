namespace RandomSkunk.Results.UnitTests;

public class AndAlsoAsync_method
{
    [Fact]
    public async Task When_IsSuccess_Returns_onSuccess_evaluation()
    {
        var source = Result.Success();
        var otherError = new Error();

        var actual = await source.AndAlsoAsync(() => Task.FromResult(Result.Fail(otherError)));

        actual.IsFail.Should().BeTrue();
        actual.GetError().Should().BeSameAs(otherError);
    }

    [Fact]
    public async Task When_IsFail_Returns_Fail()
    {
        var sourceError = new Error();
        var source = Result.Fail(sourceError);

        var actual = await source.AndAlsoAsync(() => Task.FromResult(Result.Success()));

        actual.IsFail.Should().BeTrue();
        actual.GetError().Should().BeSameAs(sourceError);
    }

    [Fact]
    public async Task When_IsFail_Given_onFail_parameter_Returns_Fail()
    {
        var sourceError = new Error();
        var source = Result.Fail(sourceError);
        var otherError = new Error();

        var actual = await source.AndAlsoAsync(() => Task.FromResult(Result.Success()), error => otherError);

        actual.IsFail.Should().BeTrue();
        actual.GetError().Should().BeSameAs(otherError);
    }

    [Fact]
    public async Task Given_null_onSuccess_function_Throws_ArgumentNullException()
    {
        var source = Result.Success();

        Func<Task> act = () => source.AndAlsoAsync(null!);

        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}

namespace RandomSkunk.Results.UnitTests;

public class Result_should
{
    private const string _errorMessage = "My error";
    private const string _errorCode = "My error code";
    private const string _stackTrace = "My stack trace";

    [Fact]
    public void Create_success_result()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFail.Should().BeFalse();
        result.Type.Should().Be(ResultType.Success);
        result.CallSite.MemberName.Should().Be(nameof(Create_success_result));
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_fail_result()
    {
        var result = Result.Fail(_errorMessage, _errorCode, _stackTrace);

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeTrue();
        result.Type.Should().Be(ResultType.Fail);
        result.CallSite.MemberName.Should().Be(nameof(Create_fail_result));
        result.Error.Message.Should().Be(_errorMessage);
        result.Error.ErrorCode.Should().Be(_errorCode);
        result.Error.StackTrace.Should().Be(_stackTrace);
    }

    [Fact]
    public void Create_fail_result2()
    {
        var result = Result.Fail();

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeTrue();
        result.Type.Should().Be(ResultType.Fail);
        result.CallSite.MemberName.Should().Be(nameof(Create_fail_result2));
        result.Error.Message.Should().Be(ResultBase.DefaultErrorMessage);
        result.Error.ErrorCode.Should().BeNull();
        result.Error.StackTrace.Should().BeNull();
    }

    [Fact]
    public void Match_func_correctly_on_success()
    {
        var result = Result.Success();

        var actual = result.Match(
            () => 1,
            error => 2);

        actual.Should().Be(1);
    }

    [Fact]
    public void Match_func_correctly_on_fail()
    {
        var result = Result.Fail(_errorMessage, _errorCode, _stackTrace);

        var actual = result.Match(
            () => 1,
            error => 2);

        actual.Should().Be(2);
    }

    [Fact]
    public void Match_action_correctly_on_success()
    {
        var result = Result.Success();

        bool successMatched = false;
        Error? failError = null;

        result.Match(
            () => successMatched = true,
            error => failError = error);

        successMatched.Should().BeTrue();
        failError.Should().BeNull();
    }

    [Fact]
    public void Match_action_correctly_on_fail()
    {
        var result = Result.Fail(_errorMessage, _errorCode, _stackTrace);

        bool successMatched = false;
        Error failError = null!;

        result.Match(
            () => successMatched = true,
            error => failError = error);

        successMatched.Should().BeFalse();
        failError.Should().NotBeNull();
        failError.Message.Should().Be(_errorMessage);
        failError.ErrorCode.Should().Be(_errorCode);
        failError.StackTrace.Should().Be(_stackTrace);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_success()
    {
        var result = Result.Success();

        var actual = await result.MatchAsync(
            () => Task.FromResult(1),
            error => Task.FromResult(2));

        actual.Should().Be(1);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_fail()
    {
        var result = Result.Fail(_errorMessage, _errorCode, _stackTrace);

        var actual = await result.MatchAsync(
            () => Task.FromResult(1),
            error => Task.FromResult(2));

        actual.Should().Be(2);
    }

    [Fact]
    public async Task Match_async_action_correctly_on_success()
    {
        var result = Result.Success();

        bool successMatched = false;
        Error? failError = null;

        await result.MatchAsync(
            () =>
            {
                successMatched = true;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        successMatched.Should().BeTrue();
        failError.Should().BeNull();
    }

    [Fact]
    public async Task Match_async_action_correctly_on_fail()
    {
        var result = Result.Fail(_errorMessage, _errorCode, _stackTrace);

        bool successMatched = false;
        Error failError = null!;

        await result.MatchAsync(
            () =>
            {
                successMatched = true;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        successMatched.Should().BeFalse();
        failError.Should().NotBeNull();
        failError.Message.Should().Be(_errorMessage);
        failError.ErrorCode.Should().Be(_errorCode);
        failError.StackTrace.Should().Be(_stackTrace);
    }
}

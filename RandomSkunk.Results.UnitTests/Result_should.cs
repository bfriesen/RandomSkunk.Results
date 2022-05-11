using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results.UnitTests;

public class Result_should
{
    private const string _errorMessage = "My error";
    private const int _errorCode = 123;
    private const string _stackTrace = "My stack trace";
    private const string _identifier = "My identifier";

    [Fact]
    public void Create_success_result()
    {
        var result = Result.Create.Success();

        result.Type.Should().Be(Success);
        Calling.GetError(result).Should().ThrowExactly<InvalidStateException>()
            .WithMessage(CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_fail_result()
    {
        var error = new Error(_errorMessage, _stackTrace, _errorCode, _identifier);
        var result = Result.Create.Fail(error);

        result.Type.Should().Be(Fail);
        result.Error().Should().Be(error);
    }

    [Fact]
    public void Create_default_result()
    {
        var result = default(Result);

        result.Type.Should().Be(Fail);
        result.Error().Should().BeSameAs(Error.DefaultError);
    }

    [Fact]
    public void Match_func_correctly_on_success()
    {
        var result = Result.Create.Success();

        var actual = result.Match(
            () => 1,
            error => 2);

        actual.Should().Be(1);
    }

    [Fact]
    public void Match_func_correctly_on_fail()
    {
        var result = Result.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = result.Match(
            () => 1,
            error => 2);

        actual.Should().Be(2);
    }

    [Fact]
    public void Match_action_correctly_on_success()
    {
        var result = Result.Create.Success();

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
        var result = Result.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

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
        failError.Identifier.Should().Be(_identifier);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_success()
    {
        var result = Result.Create.Success();

        var actual = await result.MatchAsync(
            () => Task.FromResult(1),
            error => Task.FromResult(2));

        actual.Should().Be(1);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_fail()
    {
        var result = Result.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = await result.MatchAsync(
            () => Task.FromResult(1),
            error => Task.FromResult(2));

        actual.Should().Be(2);
    }

    [Fact]
    public async Task Match_async_action_correctly_on_success()
    {
        var result = Result.Create.Success();

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
        var result = Result.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

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
        failError.Identifier.Should().Be(_identifier);
    }
}

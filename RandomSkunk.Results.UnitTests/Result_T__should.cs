namespace RandomSkunk.Results.UnitTests;

public class Result_T__should
{
    private const string _errorMessage = "My error";
    private const int _errorCode = 123;
    private const string _stackTrace = "My stack trace";
    private const string _identifier = "My identifier";

    [Fact]
    public void Create_success_result()
    {
        var result = Result<int>.Success(321);

        result.Type.Should().Be(ResultType.Success);
        result.Value.Should().Be(321);
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_success_result2()
    {
        var result = Result.Success(321);

        result.Type.Should().Be(ResultType.Success);
        result.Value.Should().Be(321);
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_fail_result()
    {
        var result = Result<int>.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

        result.Type.Should().Be(ResultType.Fail);
        result.Error.Message.Should().Be(_errorMessage);
        result.Error.ErrorCode.Should().Be(_errorCode);
        result.Error.StackTrace.Should().Be(_stackTrace);
        result.Error.Identifier.Should().Be(_identifier);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSuccessMessage);
    }

    [Fact]
    public void Create_fail_result2()
    {
        var result = Result.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        result.Type.Should().Be(ResultType.Fail);
        result.Error.Message.Should().Be(_errorMessage);
        result.Error.ErrorCode.Should().Be(_errorCode);
        result.Error.StackTrace.Should().Be(_stackTrace);
        result.Error.Identifier.Should().Be(_identifier);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSuccessMessage);
    }

    [Fact]
    public void Create_fail_result3()
    {
        var result = Result.Fail<int>();

        result.Type.Should().Be(ResultType.Fail);
        result.Error.Message.Should().Be(Error.DefaultMessage);
        result.Error.ErrorCode.Should().BeNull();
        result.Error.StackTrace.Should().BeNull();
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSuccessMessage);
    }

    [Fact]
    public void Match_func_correctly_on_success()
    {
        var result = Result.Success(3);

        var actual = result.Match(
            value => value + 1,
            error => 2);

        actual.Should().Be(4);
    }

    [Fact]
    public void Match_func_correctly_on_fail()
    {
        var result = Result.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = result.Match(
            value => value + 1,
            error => 2);

        actual.Should().Be(2);
    }

    [Fact]
    public void Match_action_correctly_on_success()
    {
        var result = Result.Success(321);

        int? successValue = null;
        Error? failError = null;

        result.Match(
            value => successValue = value,
            error => failError = error);

        successValue.Should().Be(321);
        failError.Should().BeNull();
    }

    [Fact]
    public void Match_action_correctly_on_fail()
    {
        var result = Result.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        int? successValue = null;
        Error failError = null!;

        result.Match(
            value => successValue = value,
            error => failError = error);

        successValue.Should().BeNull();
        failError.Should().NotBeNull();
        failError.Message.Should().Be(_errorMessage);
        failError.ErrorCode.Should().Be(_errorCode);
        failError.StackTrace.Should().Be(_stackTrace);
        failError.Identifier.Should().Be(_identifier);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_success()
    {
        var result = Result.Success(3);

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            error => Task.FromResult(2));

        actual.Should().Be(4);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_fail()
    {
        var result = Result.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            error => Task.FromResult(2));

        actual.Should().Be(2);
    }

    [Fact]
    public async Task Match_async_action_correctly_on_success()
    {
        var result = Result.Success(321);

        int? successValue = null;
        Error? failError = null;

        await result.MatchAsync(
            value =>
            {
                successValue = value;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        successValue.Should().Be(321);
        failError.Should().BeNull();
    }

    [Fact]
    public async Task Match_async_action_correctly_on_fail()
    {
        var result = Result.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        int? successValue = null;
        Error failError = null!;

        await result.MatchAsync(
            value =>
            {
                successValue = value;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        successValue.Should().BeNull();
        failError.Should().NotBeNull();
        failError.Message.Should().Be(_errorMessage);
        failError.ErrorCode.Should().Be(_errorCode);
        failError.StackTrace.Should().Be(_stackTrace);
        failError.Identifier.Should().Be(_identifier);
    }
}

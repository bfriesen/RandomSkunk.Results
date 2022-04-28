namespace RandomSkunk.Results.UnitTests;

public class MaybeResult_T__should
{
    private const string _errorMessage = "My error";
    private const int _errorCode = 123;
    private const string _stackTrace = "My stack trace";
    private const string _identifier = "My identifier";

    [Fact]
    public void Create_some_result()
    {
        var result = MaybeResult<int>.Some(321);

        result.IsSuccess.Should().BeTrue();
        result.IsFail.Should().BeFalse();
        result.Type.Should().Be(MaybeResultType.Some);
        result.CallSite.MemberName.Should().Be(nameof(Create_some_result));
        result.Value.Should().Be(321);
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_some_result2()
    {
        var result = MaybeResult.Some(321);

        result.IsSuccess.Should().BeTrue();
        result.IsFail.Should().BeFalse();
        result.Type.Should().Be(MaybeResultType.Some);
        result.CallSite.MemberName.Should().Be(nameof(Create_some_result2));
        result.Value.Should().Be(321);
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_none_result()
    {
        var result = MaybeResult<int>.None();

        result.IsSuccess.Should().BeTrue();
        result.IsFail.Should().BeFalse();
        result.Type.Should().Be(MaybeResultType.None);
        result.CallSite.MemberName.Should().Be(nameof(Create_none_result));
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Create_none_result2()
    {
        var result = MaybeResult.None<int>();

        result.IsSuccess.Should().BeTrue();
        result.IsFail.Should().BeFalse();
        result.Type.Should().Be(MaybeResultType.None);
        result.CallSite.MemberName.Should().Be(nameof(Create_none_result2));
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Create_fail_result()
    {
        var result = MaybeResult<int>.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeTrue();
        result.Type.Should().Be(MaybeResultType.Fail);
        result.CallSite.MemberName.Should().Be(nameof(Create_fail_result));
        result.Error.Message.Should().Be(_errorMessage);
        result.Error.ErrorCode.Should().Be(_errorCode);
        result.Error.StackTrace.Should().Be(_stackTrace);
        result.Error.Identifier.Should().Be(_identifier);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Create_fail_result2()
    {
        var result = MaybeResult.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeTrue();
        result.Type.Should().Be(MaybeResultType.Fail);
        result.CallSite.MemberName.Should().Be(nameof(Create_fail_result2));
        result.Error.Message.Should().Be(_errorMessage);
        result.Error.ErrorCode.Should().Be(_errorCode);
        result.Error.StackTrace.Should().Be(_stackTrace);
        result.Error.Identifier.Should().Be(_identifier);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Create_fail_result3()
    {
        var result = MaybeResult.Fail<int>();

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeTrue();
        result.Type.Should().Be(MaybeResultType.Fail);
        result.CallSite.MemberName.Should().Be(nameof(Create_fail_result3));
        result.Error.Message.Should().Be(ResultBase.DefaultErrorMessage);
        result.Error.ErrorCode.Should().BeNull();
        result.Error.StackTrace.Should().BeNull();
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Match_func_correctly_on_some()
    {
        var result = MaybeResult.Some(3);

        var actual = result.Match(
            value => value + 1,
            () => -1,
            error => 2);

        actual.Should().Be(4);
    }

    [Fact]
    public void Match_func_correctly_on_none()
    {
        var result = MaybeResult.None<int>();

        var actual = result.Match(
            value => value + 1,
            () => -1,
            error => 2);

        actual.Should().Be(-1);
    }

    [Fact]
    public void Match_func_correctly_on_fail()
    {
        var result = MaybeResult.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = result.Match(
            value => value + 1,
            () => -1,
            error => 2);

        actual.Should().Be(2);
    }

    [Fact]
    public void Match_action_correctly_on_some()
    {
        var result = MaybeResult.Some(321);

        int? someValue = null;
        bool noneMatched = false;
        Error? failError = null;

        result.Match(
            value => someValue = value,
            () => noneMatched = true,
            error => failError = error);

        someValue.Should().Be(321);
        noneMatched.Should().Be(false);
        failError.Should().BeNull();
    }

    [Fact]
    public void Match_action_correctly_on_none()
    {
        var result = MaybeResult.None<int>();

        int? someValue = null;
        bool noneMatched = false;
        Error? failError = null;

        result.Match(
            value => someValue = value,
            () => noneMatched = true,
            error => failError = error);

        someValue.Should().BeNull();
        noneMatched.Should().Be(true);
        failError.Should().BeNull();
    }

    [Fact]
    public void Match_action_correctly_on_fail()
    {
        var result = MaybeResult.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        int? someValue = null;
        bool noneMatched = false;
        Error failError = null!;

        result.Match(
            value => someValue = value,
            () => noneMatched = true,
            error => failError = error);

        someValue.Should().BeNull();
        noneMatched.Should().Be(false);
        failError.Should().NotBeNull();
        failError.Message.Should().Be(_errorMessage);
        failError.ErrorCode.Should().Be(_errorCode);
        failError.StackTrace.Should().Be(_stackTrace);
        failError.Identifier.Should().Be(_identifier);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_some()
    {
        var result = MaybeResult.Some(3);

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            () => Task.FromResult(-1),
            error => Task.FromResult(2));

        actual.Should().Be(4);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_none()
    {
        var result = MaybeResult.None<int>();

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            () => Task.FromResult(-1),
            error => Task.FromResult(2));

        actual.Should().Be(-1);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_fail()
    {
        var result = MaybeResult.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            () => Task.FromResult(-1),
            error => Task.FromResult(2));

        actual.Should().Be(2);
    }

    [Fact]
    public async Task Match_async_action_correctly_on_some()
    {
        var result = MaybeResult.Some(321);

        int? someValue = null;
        bool noneMatched = false;
        Error? failError = null;

        await result.MatchAsync(
            value =>
            {
                someValue = value;
                return Task.CompletedTask;
            },
            () =>
            {
                noneMatched = true;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        someValue.Should().Be(321);
        noneMatched.Should().Be(false);
        failError.Should().BeNull();
    }

    [Fact]
    public async Task Match_async_action_correctly_on_none()
    {
        var result = MaybeResult.None<int>();

        int? someValue = null;
        bool noneMatched = false;
        Error? failError = null;

        await result.MatchAsync(
            value =>
            {
                someValue = value;
                return Task.CompletedTask;
            },
            () =>
            {
                noneMatched = true;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        someValue.Should().BeNull();
        noneMatched.Should().Be(true);
        failError.Should().BeNull();
    }

    [Fact]
    public async Task Match_async_action_correctly_on_fail()
    {
        var result = MaybeResult.Fail<int>(_errorMessage, _stackTrace, _errorCode, _identifier);

        int? someValue = null;
        bool noneMatched = false;
        Error failError = null!;

        await result.MatchAsync(
            value =>
            {
                someValue = value;
                return Task.CompletedTask;
            },
            () =>
            {
                noneMatched = true;
                return Task.CompletedTask;
            },
            error =>
            {
                failError = error;
                return Task.CompletedTask;
            });

        someValue.Should().BeNull();
        noneMatched.Should().Be(false);
        failError.Should().NotBeNull();
        failError.Message.Should().Be(_errorMessage);
        failError.ErrorCode.Should().Be(_errorCode);
        failError.StackTrace.Should().Be(_stackTrace);
        failError.Identifier.Should().Be(_identifier);
    }
}

namespace RandomSkunk.Results.UnitTests;

public class Maybe_T__should
{
    private const string _errorMessage = "My error";
    private const int _errorCode = 123;
    private const string _stackTrace = "My stack trace";
    private const string _identifier = "My identifier";

    [Fact]
    public void Create_some_result()
    {
        var result = Maybe<int>.Create.Some(321);

        result.Type.Should().Be(MaybeType.Some);
        result.Value().Should().Be(321);
        Calling.GetError(result).Should().ThrowExactly<InvalidStateException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    [Fact]
    public void Create_none_result()
    {
        var result = Maybe<int>.Create.None();

        result.Type.Should().Be(MaybeType.None);
        Calling.GetError(result).Should().ThrowExactly<InvalidStateException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
        Calling.GetValue(result).Should().ThrowExactly<InvalidStateException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Create_fail_result()
    {
        var error = new Error(_errorMessage, _stackTrace, _errorCode, _identifier);
        var result = Maybe<int>.Create.Fail(error);

        result.Type.Should().Be(MaybeType.Fail);
        result.Error().Should().Be(error);
        Calling.GetValue(result).Should().ThrowExactly<InvalidStateException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Create_default_result()
    {
        var result = default(Maybe<int>);

        result.Type.Should().Be(MaybeType.Fail);
        result.Error().Should().BeSameAs(Error.DefaultError);
        Calling.GetValue(result).Should().ThrowExactly<InvalidStateException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSomeMessage);
    }

    [Fact]
    public void Match_func_correctly_on_some()
    {
        var result = Maybe<int>.Create.Some(3);

        var actual = result.Match(
            value => value + 1,
            () => -1,
            error => 2);

        actual.Should().Be(4);
    }

    [Fact]
    public void Match_func_correctly_on_none()
    {
        var result = Maybe<int>.Create.None();

        var actual = result.Match(
            value => value + 1,
            () => -1,
            error => 2);

        actual.Should().Be(-1);
    }

    [Fact]
    public void Match_func_correctly_on_fail()
    {
        var result = Maybe<int>.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = result.Match(
            value => value + 1,
            () => -1,
            error => 2);

        actual.Should().Be(2);
    }

    [Fact]
    public void Match_action_correctly_on_some()
    {
        var result = Maybe<int>.Create.Some(321);

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
        var result = Maybe<int>.Create.None();

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
        var result = Maybe<int>.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

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
        var result = Maybe<int>.Create.Some(3);

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            () => Task.FromResult(-1),
            error => Task.FromResult(2));

        actual.Should().Be(4);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_none()
    {
        var result = Maybe<int>.Create.None();

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            () => Task.FromResult(-1),
            error => Task.FromResult(2));

        actual.Should().Be(-1);
    }

    [Fact]
    public async Task Match_async_func_correctly_on_fail()
    {
        var result = Maybe<int>.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

        var actual = await result.MatchAsync(
            value => Task.FromResult(value + 1),
            () => Task.FromResult(-1),
            error => Task.FromResult(2));

        actual.Should().Be(2);
    }

    [Fact]
    public async Task Match_async_action_correctly_on_some()
    {
        var result = Maybe<int>.Create.Some(321);

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
        var result = Maybe<int>.Create.None();

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
        var result = Maybe<int>.Create.Fail(_errorMessage, _stackTrace, _errorCode, _identifier);

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

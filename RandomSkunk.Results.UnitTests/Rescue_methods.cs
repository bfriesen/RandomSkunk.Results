namespace RandomSkunk.Results.UnitTests;

public class Rescue_methods
{
    public class For_Result_sync
    {
        [Fact]
        public void Given_IsSuccess_Returns_source()
        {
            var sourceResult = Result.Success();
            var rescueResult = Result.Fail();

            var actual = sourceResult.Rescue(error => rescueResult);

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public void Given_IsFail_Returns_result_from_rescue_function()
        {
            var sourceResult = Result.Fail();
            var rescueResult = Result.Success();
            Error? capturedError = null;

            var actual = sourceResult.Rescue(error =>
            {
                capturedError = error;
                return rescueResult;
            });

            actual.Should().Be(rescueResult);
            capturedError.Should().Be(sourceResult.Error);
        }

        [Fact]
        public void Given_IsFail_When_rescue_function_throws_Returns_result_from_caught_exception()
        {
            var sourceResult = Result.Fail();
            var thrownException = new Exception();

            var actual = sourceResult.Rescue((Func<Error, Result>)(error => throw thrownException));

            actual.IsFail.Should().BeTrue();
            actual.Error.ErrorCode.Should().Be(ErrorCodes.CaughtException);
            actual.Error.InnerError!.Title.Should().Be(typeof(Exception).FullName);
        }
    }

    public class For_Result_async
    {
        [Fact]
        public async Task Given_IsSuccess_Returns_source()
        {
            var sourceResult = Result.Success();
            var rescueResult = Result.Fail();

            var actual = await sourceResult.Rescue(error => Task.FromResult(rescueResult));

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public async Task Given_IsFail_Returns_result_from_rescue_function()
        {
            var sourceResult = Result.Fail();
            var rescueResult = Result.Success();
            Error? capturedError = null;

            var actual = await sourceResult.Rescue(error =>
            {
                capturedError = error;
                return Task.FromResult(rescueResult);
            });

            actual.Should().Be(rescueResult);
            capturedError.Should().Be(sourceResult.Error);
        }

        [Fact]
        public async Task Given_IsFail_When_rescue_function_throws_Returns_result_from_caught_exception()
        {
            var sourceResult = Result.Fail();
            var thrownException = new Exception();

            var actual = await sourceResult.Rescue((Func<Error, Task<Result>>)(error => throw thrownException));

            actual.IsFail.Should().BeTrue();
            actual.Error.ErrorCode.Should().Be(ErrorCodes.CaughtException);
            actual.Error.InnerError!.Title.Should().Be(typeof(Exception).FullName);
        }
    }

    public class For_Result_of_T_sync
    {
        [Fact]
        public void Given_IsSuccess_Returns_source()
        {
            var sourceResult = Result<int>.Success(123);
            var rescueResult = Result<int>.Fail();

            var actual = sourceResult.Rescue(error => rescueResult);

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public void Given_IsFail_Returns_result_from_rescue_function()
        {
            var sourceResult = Result<int>.Fail();
            var rescueResult = Result<int>.Success(123);
            Error? capturedError = null;

            var actual = sourceResult.Rescue(error =>
            {
                capturedError = error;
                return rescueResult;
            });

            actual.Should().Be(rescueResult);
            capturedError.Should().Be(sourceResult.Error);
        }

        [Fact]
        public void Given_IsFail_When_rescue_function_throws_Returns_result_from_caught_exception()
        {
            var sourceResult = Result<int>.Fail();
            var thrownException = new Exception();

            var actual = sourceResult.Rescue((Func<Error, Result<int>>)(error => throw thrownException));

            actual.IsFail.Should().BeTrue();
            actual.Error.ErrorCode.Should().Be(ErrorCodes.CaughtException);
            actual.Error.InnerError!.Title.Should().Be(typeof(Exception).FullName);
        }
    }

    public class For_Result_of_T_async
    {
        [Fact]
        public async Task Given_IsSuccess_Returns_source()
        {
            var sourceResult = Result<int>.Success(123);
            var rescueResult = Result<int>.Fail();

            var actual = await sourceResult.Rescue(error => Task.FromResult(rescueResult));

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public async Task Given_IsFail_Returns_result_from_rescue_function()
        {
            var sourceResult = Result<int>.Fail();
            var rescueResult = Result<int>.Success(123);
            Error? capturedError = null;

            var actual = await sourceResult.Rescue(error =>
            {
                capturedError = error;
                return Task.FromResult(rescueResult);
            });

            actual.Should().Be(rescueResult);
            capturedError.Should().Be(sourceResult.Error);
        }

        [Fact]
        public async Task Given_IsFail_When_rescue_function_throws_Returns_result_from_caught_exception()
        {
            var sourceResult = Result<int>.Fail();
            var thrownException = new Exception();

            var actual = await sourceResult.Rescue((Func<Error, Task<Result<int>>>)(error => throw thrownException));

            actual.IsFail.Should().BeTrue();
            actual.Error.ErrorCode.Should().Be(ErrorCodes.CaughtException);
            actual.Error.InnerError!.Title.Should().Be(typeof(Exception).FullName);
        }
    }

    public class For_Maybe_of_T_sync
    {
        [Fact]
        public void Given_IsSuccess_Returns_source()
        {
            var sourceResult = Maybe<int>.Success(123);
            var rescueResult = Maybe<int>.Fail();

            var actual = sourceResult.Rescue(error => rescueResult);

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public void Given_None_Returns_source()
        {
            var sourceResult = Maybe<int>.None;
            var rescueResult = Maybe<int>.Fail();

            var actual = sourceResult.Rescue(error => rescueResult);

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public void Given_None_Returns_result_from_onNone_rescue_function()
        {
            var sourceResult = Maybe<int>.None;
            var rescueResult = Maybe<int>.Fail("b");

            var actual = sourceResult.Rescue(error => Maybe<int>.Fail("a"), () => rescueResult);

            actual.Should().Be(rescueResult);
        }

        [Fact]
        public void Given_IsFail_Returns_result_from_rescue_function()
        {
            var sourceResult = Maybe<int>.Fail();
            var rescueResult = Maybe<int>.Success(123);
            Error? capturedError = null;

            var actual = sourceResult.Rescue(error =>
            {
                capturedError = error;
                return rescueResult;
            });

            actual.Should().Be(rescueResult);
            capturedError.Should().Be(sourceResult.Error);
        }

        [Fact]
        public void Given_IsFail_When_rescue_function_throws_Returns_result_from_caught_exception()
        {
            var sourceResult = Maybe<int>.Fail();
            var thrownException = new Exception();

            var actual = sourceResult.Rescue((Func<Error, Maybe<int>>)(error => throw thrownException));

            actual.IsFail.Should().BeTrue();
            actual.Error.ErrorCode.Should().Be(ErrorCodes.CaughtException);
            actual.Error.InnerError!.Title.Should().Be(typeof(Exception).FullName);
        }
    }

    public class For_Maybe_of_T_async
    {
        [Fact]
        public async Task Given_IsSuccess_Returns_source()
        {
            var sourceResult = Maybe<int>.Success(123);
            var rescueResult = Maybe<int>.Fail();

            var actual = await sourceResult.Rescue(error => Task.FromResult(rescueResult));

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public async Task Given_IsNone_Returns_source()
        {
            var sourceResult = Maybe<int>.None;
            var rescueResult = Maybe<int>.Fail();

            var actual = await sourceResult.Rescue(error => Task.FromResult(rescueResult));

            actual.Should().Be(sourceResult);
        }

        [Fact]
        public async Task Given_IsNone_Returns_result_from_onNone_rescue_function()
        {
            var sourceResult = Maybe<int>.None;
            var rescueResult = Maybe<int>.Fail("b");

            var actual = await sourceResult.Rescue(
                error => Task.FromResult(Maybe<int>.Fail("a")),
                () => Task.FromResult(rescueResult));

            actual.Should().Be(rescueResult);
        }

        [Fact]
        public async Task Given_IsFail_Returns_result_from_rescue_function()
        {
            var sourceResult = Maybe<int>.Fail();
            var rescueResult = Maybe<int>.Success(123);
            Error? capturedError = null;

            var actual = await sourceResult.Rescue(error =>
            {
                capturedError = error;
                return Task.FromResult(rescueResult);
            });

            actual.Should().Be(rescueResult);
            capturedError.Should().Be(sourceResult.Error);
        }

        [Fact]
        public async Task Given_IsFail_When_rescue_function_throws_Returns_result_from_caught_exception()
        {
            var sourceResult = Maybe<int>.Fail();
            var thrownException = new Exception();

            var actual = await sourceResult.Rescue((Func<Error, Task<Maybe<int>>>)(error => throw thrownException));

            actual.IsFail.Should().BeTrue();
            actual.Error.ErrorCode.Should().Be(ErrorCodes.CaughtException);
            actual.Error.InnerError!.Title.Should().Be(typeof(Exception).FullName);
        }
    }
}

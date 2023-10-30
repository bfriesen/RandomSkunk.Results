namespace RandomSkunk.Results.UnitTests;

public class OnFail_methods
{
    public class For_Result_sync
    {
        [Fact]
        public void Given_IsSuccess_DoesNothing()
        {
            var result = Result.Success();

            Error? capturedError = null;

            var actual = result.OnFail(error => capturedError = error);

            actual.Should().Be(result);
            capturedError.Should().BeNull();
        }

        [Fact]
        public void Given_IsFail_InvokesOnFailFunction()
        {
            var result = Result.Fail();

            Error? capturedError = null;

            var actual = result.OnFail(error => capturedError = error);

            actual.Should().Be(result);
            capturedError.Should().BeSameAs(result.Error);
        }
    }

    public class For_Result_async
    {
        [Fact]
        public async Task Given_IsSuccess_DoesNothing()
        {
            var result = Result.Success();

            Error? capturedError = null;

            var actual = await result.OnFail(error =>
            {
                capturedError = error;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedError.Should().BeNull();
        }

        [Fact]
        public async Task Given_IsFail_InvokesOnFailFunction()
        {
            var result = Result.Fail();

            Error? capturedError = null;

            var actual = await result.OnFail(error =>
            {
                capturedError = error;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedError.Should().BeSameAs(result.Error);
        }
    }

    public class For_Result_of_T_sync
    {
        [Fact]
        public void Given_IsSuccess_DoesNothing()
        {
            var result = Result<int>.Success(123);

            Error? capturedError = null;

            var actual = result.OnFail(error => capturedError = error);

            actual.Should().Be(result);
            capturedError.Should().BeNull();
        }

        [Fact]
        public void Given_IsFail_InvokesOnFailFunction()
        {
            var result = Result<int>.Fail();

            Error? capturedError = null;

            var actual = result.OnFail(error => capturedError = error);

            actual.Should().Be(result);
            capturedError.Should().BeSameAs(result.Error);
        }
    }

    public class For_Result_of_T_async
    {
        [Fact]
        public async Task Given_IsSuccess_DoesNothing()
        {
            var result = Result<int>.Success(123);

            Error? capturedError = null;

            var actual = await result.OnFail(error =>
            {
                capturedError = error;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedError.Should().BeNull();
        }

        [Fact]
        public async Task Given_IsFail_InvokesOnFailFunction()
        {
            var result = Result<int>.Fail();

            Error? capturedError = null;

            var actual = await result.OnFail(error =>
            {
                capturedError = error;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedError.Should().BeSameAs(result.Error);
        }
    }
}

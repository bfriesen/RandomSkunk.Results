namespace RandomSkunk.Results.UnitTests;

public class Finally_methods
{
    public class For_Result_sync
    {
        [Fact]
        public void Given_IsFail_InvokesCallbackFunction()
        {
            var result = Result.Fail();

            Result? capturedResult = null;

            var actual = result.Finally(r => capturedResult = r);

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }

        [Fact]
        public void Given_IsSuccess_InvokesCallbackFunction()
        {
            var result = Result.Success();

            Result? capturedResult = null;

            var actual = result.Finally(r => capturedResult = r);

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }
    }

    public class For_Result_async
    {
        [Fact]
        public async Task Given_IsFail_InvokesCallbackFunction()
        {
            var result = Result.Fail();

            Result? capturedResult = null;

            var actual = await result.Finally(r =>
            {
                capturedResult = r;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }

        [Fact]
        public async Task Given_IsSuccess_InvokesCallbackFunction()
        {
            var result = Result.Success();

            Result? capturedResult = null;

            var actual = await result.Finally(r =>
            {
                capturedResult = r;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }
    }

    public class For_Result_of_T_sync
    {
        [Fact]
        public void Given_IsFail_DoesNothing()
        {
            var result = Result<int>.Fail();

            Result<int>? capturedResult = null;

            var actual = result.Finally(r => capturedResult = r);

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }

        [Fact]
        public void Given_IsSuccess_InvokesOnSuccessFunction()
        {
            var result = Result<int>.Success(123);

            Result<int>? capturedResult = null;

            var actual = result.Finally(r => capturedResult = r);

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }
    }

    public class For_Result_of_T_async
    {
        [Fact]
        public async Task Given_IsFail_DoesNothing()
        {
            var result = Result<int>.Fail();

            Result<int>? capturedResult = null;

            var actual = await result.Finally(r =>
            {
                capturedResult = r;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }

        [Fact]
        public async Task Given_IsSuccess_InvokesOnSuccessFunction()
        {
            var result = Result<int>.Success(123);

            Result<int>? capturedResult = null;

            var actual = await result.Finally(r =>
            {
                capturedResult = r;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedResult.Should().Be(result);
        }
    }
}

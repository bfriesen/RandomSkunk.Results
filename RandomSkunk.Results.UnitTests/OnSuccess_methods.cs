namespace RandomSkunk.Results.UnitTests;

public class OnSuccess_methods
{
    public class For_Result_sync
    {
        [Fact]
        public void Given_IsFail_DoesNothing()
        {
            var result = Result.Fail();

            var invoked = false;

            var actual = result.OnSuccess(() => invoked = true);

            actual.Should().Be(result);
            invoked.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_InvokesOnSuccessFunction()
        {
            var result = Result.Success();

            var invoked = false;

            var actual = result.OnSuccess(() => invoked = true);

            actual.Should().Be(result);
            invoked.Should().BeTrue();
        }
    }

    public class For_Result_async
    {
        [Fact]
        public async Task Given_IsFail_DoesNothing()
        {
            var result = Result.Fail();

            var invoked = false;

            var actual = await result.OnSuccess(() =>
            {
                invoked = true;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            invoked.Should().BeFalse();
        }

        [Fact]
        public async Task Given_IsSuccess_InvokesOnSuccessFunction()
        {
            var result = Result.Success();

            var invoked = false;

            var actual = await result.OnSuccess(() =>
            {
                invoked = true;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            invoked.Should().BeTrue();
        }
    }

    public class For_Result_of_T_sync
    {
        [Fact]
        public void Given_IsFail_DoesNothing()
        {
            var result = Result<int>.Fail();

            int? capturedValue = null;

            var actual = result.OnSuccess(value => capturedValue = value);

            actual.Should().Be(result);
            capturedValue.Should().BeNull();
        }

        [Fact]
        public void Given_IsSuccess_InvokesOnSuccessFunction()
        {
            var result = Result<int>.Success(123);
            int? capturedValue = null;

            var actual = result.OnSuccess(value => capturedValue = value);

            actual.Should().Be(result);
            capturedValue.Should().Be(123);
        }
    }

    public class For_Result_of_T_async
    {
        [Fact]
        public async Task Given_IsFail_DoesNothing()
        {
            var result = Result<int>.Fail();

            int? capturedValue = null;

            var actual = await result.OnSuccess(value =>
            {
                capturedValue = value;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedValue.Should().BeNull();
        }

        [Fact]
        public async Task Given_IsSuccess_InvokesOnSuccessFunction()
        {
            var result = Result<int>.Success(123);

            int? capturedValue = null;

            var actual = await result.OnSuccess(value =>
            {
                capturedValue = value;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            capturedValue.Should().Be(123);
        }
    }
}

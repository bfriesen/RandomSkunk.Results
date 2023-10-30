namespace RandomSkunk.Results.UnitTests;

public class OnNone_methods
{
    public class For_Result_of_T_sync
    {
        [Fact]
        public void Given_IsSuccess_DoesNothing()
        {
            var result = Result<int>.Success(123);

            var invoked = false;

            var actual = result.OnNone(() => invoked = true);

            actual.Should().Be(result);
            invoked.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_DoesNothing()
        {
            var result = Result<int>.Fail();

            var invoked = false;

            var actual = result.OnNone(() => invoked = true);

            actual.Should().Be(result);
            invoked.Should().BeFalse();
        }

        [Fact]
        public void Given_IsNone_InvokesOnNoneFunction()
        {
            var result = Result<int>.None();

            var invoked = false;

            var actual = result.OnNone(() => invoked = true);

            actual.Should().Be(result);
            invoked.Should().BeTrue();
        }
    }

    public class For_Result_of_T_async
    {
        [Fact]
        public async Task Given_IsSuccess_DoesNothing()
        {
            var result = Result<int>.Success(123);

            var invoked = false;

            var actual = await result.OnNone(() =>
            {
                invoked = true;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            invoked.Should().BeFalse();
        }

        [Fact]
        public async Task Given_IsFail_DoesNothing()
        {
            var result = Result<int>.Fail();

            var invoked = false;

            var actual = await result.OnNone(() =>
            {
                invoked = true;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            invoked.Should().BeFalse();
        }

        [Fact]
        public async Task Given_IsNone_InvokesOnNoneFunction()
        {
            var result = Result<int>.None();

            var invoked = false;

            var actual = await result.OnNone(() =>
            {
                invoked = true;
                return Task.CompletedTask;
            });

            actual.Should().Be(result);
            invoked.Should().BeTrue();
        }
    }
}

namespace RandomSkunk.Results.UnitTests;

public class Match_methods
{
    public class For_Result_sync
    {
        [Fact]
        public void When_IsSuccess_Returns_success_function_evaluation()
        {
            var result = Result.Success();

            var actual = result.Match(
                () => 1,
                error => -1);

            actual.Should().Be(1);
        }

        [Fact]
        public void When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Result.Fail();

            var actual = result.Match(
                () => 1,
                error => -1);

            actual.Should().Be(-1);
        }
    }

    public class For_Result_async
    {
        [Fact]
        public async Task When_IsSuccess_Returns_success_function_evaluation()
        {
            var result = Result.Success();

            var actual = await result.Match(
                () => Task.FromResult(1),
                error => Task.FromResult(-1));

            actual.Should().Be(1);
        }

        [Fact]
        public async Task When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Result.Fail();

            var actual = await result.Match(
                () => Task.FromResult(1),
                error => Task.FromResult(-1));

            actual.Should().Be(-1);
        }
    }

    public class For_Result_of_T_sync
    {
        [Fact]
        public void When_IsSuccess_Returns_success_function_evaluation()
        {
            var result = 1.ToResult();

            var actual = result.Match(
                value => value + 1,
                error => -1);

            actual.Should().Be(2);
        }

        [Fact]
        public void When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Result<int>.Fail();

            var actual = result.Match(
                value => value + 1,
                error => -1);

            actual.Should().Be(-1);
        }
    }

    public class For_Result_of_T_async
    {
        [Fact]
        public async Task When_IsSuccess_Returns_success_function_evaluation()
        {
            var result = 1.ToResult();

            var actual = await result.Match(
                value => Task.FromResult(value + 1),
                error => Task.FromResult(-1));

            actual.Should().Be(2);
        }

        [Fact]
        public async Task When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Result<int>.Fail();

            var actual = await result.Match(
                value => Task.FromResult(value + 1),
                error => Task.FromResult(-1));

            actual.Should().Be(-1);
        }
    }
}

namespace RandomSkunk.Results.UnitTests;

public class SelectAsync_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public async Task When_IsSuccess_Returns_Success_result_from_function_evaluation()
        {
            var source = 1.ToResult();

            var actual = await source.SelectAsync(value => Task.FromResult(value.ToString()));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = await source.SelectAsync(value => Task.FromResult(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_null_selector_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Func<Task> act = () => source.SelectAsync<string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public async Task When_IsSuccess_Returns_Success_result_from_function_evaluation()
        {
            var source = 1.ToMaybe();

            var actual = await source.SelectAsync(value => Task.FromResult(value.ToString()));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = await source.SelectAsync(value => Task.FromResult(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task When_IsNone_Returns_None()
        {
            var source = Maybe<int>.None();

            var actual = await source.SelectAsync(value => Task.FromResult(value.ToString()));

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task Given_null_selector_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Func<Task> act = () => source.SelectAsync<string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}

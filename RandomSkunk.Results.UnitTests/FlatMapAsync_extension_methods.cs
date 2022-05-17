namespace RandomSkunk.Results.UnitTests;

public class FlatMapAsync_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public async Task When_IsSuccess_Returns_function_evaluation()
        {
            var source = Result<int>.Create.Success(1);

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result<string>.Create.Success(value.ToString())));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result<string>.Create.Success(value.ToString())));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_null_filter_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Func<Task> act = () => source.FlatMapAsync<int, string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public async Task When_IsSome_Returns_function_evaluation()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = await source.FlatMapAsync(value => Task.FromResult(Maybe<string>.Create.Some(value.ToString())));

            actual.IsSome.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(Maybe<string>.Create.Some(value.ToString())));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task When_IsNone_Returns_None()
        {
            var source = Maybe<int>.Create.None();

            var actual = await source.FlatMapAsync(value => Task.FromResult(Maybe<string>.Create.Some(value.ToString())));

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task When_null_filter_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Func<Task> act = () => source.FlatMapAsync<int, string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}

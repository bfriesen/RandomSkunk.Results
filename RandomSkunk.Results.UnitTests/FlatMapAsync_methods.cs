namespace RandomSkunk.Results.UnitTests;

public class FlatMapAsync_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = 1.ToResult();

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Func<Task> act = () => source.FlatMapAsync((Func<int, Task<Result<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Func<Task> act = () => source.FlatMapAsync(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = await source.FlatMapAsync(value => Task.FromResult(Maybe<int>.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToMaybe()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Func<Task> act = () => source.FlatMapAsync((Func<int, Task<Maybe<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = 1.ToMaybe();

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToMaybe()));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToMaybe()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_None_Returns_None()
        {
            var source = Maybe<int>.None();

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToMaybe()));

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Func<Task> act = () => source.FlatMapAsync((Func<int, Task<Maybe<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<string>.None();

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();

            var expectedError = Errors.ResultIsNone();

            actual.GetError().Message.Should().Be(expectedError.Message);
            actual.GetError().ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.GetError().Title.Should().Be(expectedError.Title);
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<string>.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Func<Task> act = () => source.FlatMapAsync(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = await source.FlatMapAsync(value => Task.FromResult(Result<int>.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<int>.None();

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();

            var expectedError = Errors.ResultIsNone();

            actual.GetError().Message.Should().Be(expectedError.Message);
            actual.GetError().ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.GetError().Title.Should().Be(expectedError.Title);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = await source.FlatMapAsync(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Func<Task> act = () => source.FlatMapAsync((Func<int, Task<Result<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }

    public class For_Result
    {
        [Fact]
        public async Task Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = await source.FlatMapAsync(() => Task.FromResult(Result.Fail("a")));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail()
        {
            var source = Result.Fail("a");

            var actual = await source.FlatMapAsync(() => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Func<Task> act = () => source.FlatMapAsync(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = await source.FlatMapAsync(() => Task.FromResult(Result<int>.Success(1)));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be(1);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var source = Result.Fail("a");

            var actual = await source.FlatMapAsync(() => Task.FromResult(Result<int>.Success(1)));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Func<Task> act = () => source.FlatMapAsync((Func<Task<Result<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = await source.FlatMapAsync(() => Task.FromResult(Maybe<int>.Success(1)));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be(1);
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var source = Result.Fail("a");

            var actual = await source.FlatMapAsync(() => Task.FromResult(Maybe<int>.Success(1)));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Func<Task> act = () => source.FlatMapAsync((Func<Task<Maybe<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}

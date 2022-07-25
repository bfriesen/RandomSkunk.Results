namespace RandomSkunk.Results.UnitTests;

public class FlatMap_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = 1.ToResult();

            var actual = source.FlatMap(value => value.ToString().ToResult());

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.FlatMap(value => value.ToString().ToResult());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.FlatMap((Func<int, Result<string>>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = source.FlatMap(value => Result.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.FlatMap(value => Result.Success());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Result_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.FlatMap(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = source.FlatMap(value => Maybe<int>.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.FlatMap((Func<int, Maybe<string>>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = 1.ToMaybe();

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_None_Returns_None()
        {
            var source = Maybe<int>.None();

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.FlatMap((Func<int, Maybe<string>>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = source.FlatMap(value => Result.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<string>.None();

            var actual = source.FlatMap(value => Result.Success());

            actual.IsFail.Should().BeTrue();

            var expectedError = Errors.ResultIsNone();

            actual.GetError().Message.Should().Be(expectedError.Message);
            actual.GetError().ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.GetError().Title.Should().Be(expectedError.Title);
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<string>.Fail(error);

            var actual = source.FlatMap(value => Result.Success());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Result_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.FlatMap(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = source.FlatMap(value => Result<int>.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<int>.None();

            var actual = source.FlatMap(value => value.ToString().ToResult());

            actual.IsFail.Should().BeTrue();

            var expectedError = Errors.ResultIsNone();

            actual.GetError().Message.Should().Be(expectedError.Message);
            actual.GetError().ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.GetError().Title.Should().Be(expectedError.Title);
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = source.FlatMap(value => value.ToString().ToResult());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Result_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.FlatMap((Func<int, Result<string>>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Result
    {
        [Fact]
        public void Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = source.FlatMap(() => Result.Fail("a"));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail()
        {
            var source = Result.Fail("a");

            var actual = source.FlatMap(() => Result.Success());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Action act = () => source.FlatMap(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = source.FlatMap(() => Result<int>.Success(1));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be(1);
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var source = Result.Fail("a");

            var actual = source.FlatMap(() => Result<int>.Success(1));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Action act = () => source.FlatMap((Func<Result<string>>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = source.FlatMap(() => Maybe<int>.Success(1));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be(1);
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var source = Result.Fail("a");

            var actual = source.FlatMap(() => Maybe<int>.Success(1));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Action act = () => source.FlatMap((Func<Maybe<string>>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}

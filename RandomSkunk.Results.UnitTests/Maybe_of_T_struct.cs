using static RandomSkunk.Results.MaybeOutcome;

namespace RandomSkunk.Results.UnitTests;

public class Maybe_of_T_struct
{
    [Fact]
    public void Default_value_is_error_result()
    {
        var result = default(Maybe<int>);

        result._outcome.Should().Be(Fail);
        result.IsFail.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.IsDefault.Should().BeTrue();
        result.Error.Should().BeSameAs(Error.DefaultError);
    }

    public class Create
    {
        [Fact]
        public void Success_Returns_Success_result_with_specified_value()
        {
            var result = 1.ToMaybe();

            result._outcome.Should().Be(Success);
            result.IsSuccess.Should().BeTrue();
            result.IsFail.Should().BeFalse();
            result.IsNone.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
            result._value.Should().Be(1);
        }

        [Fact]
        public void Fail_Returns_Fail_result_with_specified_error()
        {
            var error = new Error();
            var result = Maybe<int>.Fail(error);

            result._outcome.Should().Be(Fail);
            result.IsFail.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.IsNone.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
            result.Error.Should().BeSameAs(error);
        }

        [Fact]
        public void None_Returns_None_result()
        {
            var result = Maybe<int>.None();

            result._outcome.Should().Be(None);
            result.IsNone.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.IsFail.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
        }
    }

    public class Match
    {
        [Fact]
        public void When_IsSuccess_Returns_Success_function_evaluation()
        {
            var result = 1.ToMaybe();

            var actual = result.Match(
                value => value + 1,
                () => 0,
                error => -1);

            actual.Should().Be(2);
        }

        [Fact]
        public void When_IsFail_Returns_Fail_function_evaluation()
        {
            var result = Maybe<int>.Fail();

            var actual = result.Match(
                value => value + 1,
                () => 0,
                error => -1);

            actual.Should().Be(-1);
        }

        [Fact]
        public void When_IsNone_Returns_None_function_evaluation()
        {
            var result = Maybe<int>.None();

            var actual = result.Match(
                value => value + 1,
                () => 0,
                error => -1);

            actual.Should().Be(0);
        }
    }

    public class MatchAsync
    {
        [Fact]
        public async Task When_IsSuccess_Returns_Success_function_evaluation()
        {
            var result = 1.ToMaybe();

            var actual = await result.MatchAsync(
                value => Task.FromResult(value + 1),
                () => Task.FromResult(0),
                error => Task.FromResult(-1));

            actual.Should().Be(2);
        }

        [Fact]
        public async Task When_IsFail_Returns_Fail_function_evaluation()
        {
            var result = Maybe<int>.Fail();

            var actual = await result.MatchAsync(
                value => Task.FromResult(value + 1),
                () => Task.FromResult(0),
                error => Task.FromResult(-1));

            actual.Should().Be(-1);
        }

        [Fact]
        public async Task When_IsNone_Returns_None_function_evaluation()
        {
            var result = Maybe<int>.None();

            var actual = await result.MatchAsync(
                value => Task.FromResult(value + 1),
                () => Task.FromResult(0),
                error => Task.FromResult(-1));

            actual.Should().Be(0);
        }
    }

    public new class Equals
    {
        [Fact]
        public void Given_IsSuccess_When_other_IsSuccess_with_same_value_Returns_true()
        {
            var result = 1.ToMaybe();
            object other = 1.ToMaybe();

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_equal_error_Returns_true()
        {
            var error = new Error("a", "d") { StackTrace = "b", ErrorCode = 1, Identifier = "c" };
            var result = Maybe<int>.Fail(error);
            var otherError = new Error("a", "d") { StackTrace = "b", ErrorCode = 1, Identifier = "c" };
            object other = Maybe<int>.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_IsNone_When_other_IsNone_Returns_true()
        {
            var result = Maybe<int>.None();
            object other = Maybe<int>.None();

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void When_other_is_null_Returns_false()
        {
            var result = 1.ToMaybe();
            object? other = null;

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsSuccess_with_different_value_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = 2.ToMaybe();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsFail_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = Maybe<int>.Fail();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsNone_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = Maybe<int>.None();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsSuccess_Returns_false()
        {
            var result = Maybe<int>.Fail();
            object other = 1.ToMaybe();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsNone_Returns_false()
        {
            var result = Maybe<int>.Fail();
            object other = Maybe<int>.None();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_unequal_error_Returns_false()
        {
            var error = new Error("a", "d") { StackTrace = "b", ErrorCode = 1, Identifier = "c" };
            var result = Maybe<int>.Fail(error);
            var otherError = new Error("w", "z") { StackTrace = "x", ErrorCode = 2, Identifier = "y" };
            object other = Maybe<int>.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_is_different_value_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = 2;

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }
    }

    public new class GetHashCode
    {
        [Fact]
        public void Different_results_return_different_values()
        {
            var success1 = 1.ToMaybe().GetHashCode();
            var successA = "1".ToMaybe().GetHashCode();
            var fail1 = Maybe<int>.Fail("X").GetHashCode();
            var fail2 = Maybe<int>.Fail("Y").GetHashCode();
            var failA = Maybe<string>.Fail("X").GetHashCode();
            var failB = Maybe<string>.Fail("Y").GetHashCode();
            var none1 = Maybe<int>.None().GetHashCode();
            var noneA = Maybe<string>.None().GetHashCode();

            success1.Should().NotBe(successA);
            success1.Should().NotBe(fail1);
            success1.Should().NotBe(fail2);
            success1.Should().NotBe(failA);
            success1.Should().NotBe(failB);
            success1.Should().NotBe(none1);
            success1.Should().NotBe(noneA);

            successA.Should().NotBe(fail1);
            successA.Should().NotBe(fail2);
            successA.Should().NotBe(failA);
            successA.Should().NotBe(failB);
            successA.Should().NotBe(none1);
            successA.Should().NotBe(noneA);

            fail1.Should().NotBe(fail2);
            fail1.Should().NotBe(failA);
            fail1.Should().NotBe(failB);
            fail1.Should().NotBe(none1);
            fail1.Should().NotBe(noneA);

            fail2.Should().NotBe(failA);
            fail2.Should().NotBe(failB);
            fail2.Should().NotBe(none1);
            fail2.Should().NotBe(noneA);

            failA.Should().NotBe(failB);
            failA.Should().NotBe(none1);
            failA.Should().NotBe(noneA);

            failB.Should().NotBe(none1);
            failB.Should().NotBe(noneA);

            none1.Should().NotBe(noneA);
        }
    }
}

namespace RandomSkunk.Results.UnitTests;

public class Result_of_T_struct
{
    [Fact]
    public void Default_value_is_error_result()
    {
        var result = default(Result<int>);

        result.IsFail.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeSameAs(Error.DefaultError);
    }

    public class Create
    {
        [Fact]
        public void Success_Returns_success_result_with_specified_value()
        {
            var result = 1.ToResult();

            result.IsSuccess.Should().BeTrue();
            result.IsFail.Should().BeFalse();
            result.Value.Should().Be(1);
        }

        [Fact]
        public void Fail_Returns_fail_result_with_specified_error()
        {
            var error = new Error();
            var result = Result<int>.Fail(error, true);

            result.IsFail.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeSameAs(error);
        }
    }

    public class Match
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

    public class Async_Match
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

    public new class Equals
    {
        [Fact]
        public void Given_IsSuccess_When_other_IsSuccess_with_same_value_Returns_true()
        {
            var result = 1.ToResult();
            object other = 1.ToResult();

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_equal_error_Returns_true()
        {
            var error = new Error { Message = "a", StackTrace = "b", ErrorCode = 1, Identifier = "c", Title = "d" };
            var result = Result<int>.Fail(error);
            var otherError = new Error { Message = "a", StackTrace = "b", ErrorCode = 1, Identifier = "c", Title = "d" };
            object other = Result<int>.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void When_other_is_null_Returns_false()
        {
            var result = 1.ToResult();
            object? other = null;

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsSuccess_with_different_value_Returns_false()
        {
            var result = 1.ToResult();
            object other = 2.ToResult();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsFail_Returns_false()
        {
            var result = 1.ToResult();
            object other = Result<int>.Fail();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsSuccess_Returns_false()
        {
            var result = Result<int>.Fail();
            object other = 1.ToResult();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_unequal_error_Returns_false()
        {
            var error = new Error { Message = "a", StackTrace = "b", ErrorCode = 1, Identifier = "c", Title = "d" };
            var result = Result<int>.Fail(error);
            var otherError = new Error { Message = "w", StackTrace = "x", ErrorCode = 2, Identifier = "y", Title = "d" };
            object other = Result<int>.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_is_different_value_Returns_false()
        {
            var result = 1.ToResult();
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
            var success1 = 1.ToResult().GetHashCode();
            var successA = "1".ToResult().GetHashCode();
            var fail1 = Result<int>.Fail("X").GetHashCode();
            var fail2 = Result<int>.Fail("Y").GetHashCode();
            var failA = Result<string>.Fail("X").GetHashCode();
            var failB = Result<string>.Fail("Y").GetHashCode();

            success1.Should().NotBe(successA);
            success1.Should().NotBe(fail1);
            success1.Should().NotBe(fail2);
            success1.Should().NotBe(failA);
            success1.Should().NotBe(failB);

            successA.Should().NotBe(fail1);
            successA.Should().NotBe(fail2);
            successA.Should().NotBe(failA);
            successA.Should().NotBe(failB);

            fail1.Should().NotBe(fail2);
            fail1.Should().NotBe(failA);
            fail1.Should().NotBe(failB);

            fail2.Should().NotBe(failA);
            fail2.Should().NotBe(failB);

            failA.Should().NotBe(failB);
        }
    }
}

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
        result.IsUninitialized.Should().BeTrue();
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
            var result = Result<int>.Fail(error);

            result.IsFail.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeSameAs(error);
        }
    }

    public class Error_property
    {
        [Fact]
        public void When_IsFail_Returns_error()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.Error;

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsSuccess_Throws_InvalidStateException()
        {
            var source = 1.ToResult();

            Action act = () => _ = source.Error;

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }
    }

    public class Value_property
    {
        [Fact]
        public void When_IsSuccess_Returns_value()
        {
            var source = 1.ToResult();

            var actual = source.Value;

            actual.Should().Be(1);
        }

        [Fact]
        public void When_IsFail_Throws_InvalidStateException()
        {
            var source = Result<int>.Fail();

            Action act = () => _ = source.Value;

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSuccessMessage);
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
            var error = new Error { Message = "a", ErrorCode = 1, Identifier = "c", Title = "d" };
            var result = Result<int>.Fail(error);
            var otherError = new Error { Message = "a", ErrorCode = 1, Identifier = "c", Title = "d" };
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
            var error = new Error { Message = "a", ErrorCode = 1, Identifier = "c", Title = "d" };
            var result = Result<int>.Fail(error);
            var otherError = new Error { Message = "w", ErrorCode = 2, Identifier = "y", Title = "d" };
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

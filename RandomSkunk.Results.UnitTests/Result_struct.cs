namespace RandomSkunk.Results.UnitTests;

public class Result_struct
{
    [Fact]
    public void Default_value_is_error_result()
    {
        var result = default(Result);

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
            var result = Result.Success();

            result.IsSuccess.Should().BeTrue();
            result.IsFail.Should().BeFalse();
        }

        [Fact]
        public void Fail_Returns_fail_result_with_specified_error()
        {
            var error = new Error();
            var result = Result.Fail(error);

            result.IsFail.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeSameAs(error);
        }
    }

    public class ErrorProperty
    {
        [Fact]
        public void When_IsFail_Returns_error()
        {
            var error = new Error();
            var source = Result.Fail(error);

            var actual = source.Error;

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsSuccess_Throws_InvalidStateException()
        {
            var source = Result.Success();

            Action act = () => _ = source.Error;

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }
    }

    public new class Equals
    {
        [Fact]
        public void Given_IsSuccess_When_other_IsSuccess_Returns_true()
        {
            var result = Result.Success();
            object other = Result.Success();

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_equal_error_Returns_true()
        {
            var error = new Error { Message = "a", ErrorCode = 1, Identifier = "c", Title = "d" };
            var result = Result.Fail(error);
            var otherError = new Error { Message = "a", ErrorCode = 1, Identifier = "c", Title = "d" };
            object other = Result.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void When_other_is_null_Returns_false()
        {
            var result = Result.Success();
            object? other = null;

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsFail_Returns_false()
        {
            var result = Result.Success();
            object other = Result.Fail();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsSuccess_Returns_false()
        {
            var result = Result.Fail();
            object other = Result.Success();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_unequal_error_Returns_false()
        {
            var error = new Error { Message = "a", ErrorCode = 1, Identifier = "c", Title = "d" };
            var result = Result.Fail(error);
            var otherError = new Error { Message = "w", ErrorCode = 2, Identifier = "y", Title = "z" };
            object other = Result.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }
    }

    public new class GetHashCode
    {
        [Fact]
        public void Different_results_return_different_values()
        {
            var success1 = Result.Success().GetHashCode();
            var fail1 = Result.Fail("X").GetHashCode();
            var fail2 = Result.Fail("Y").GetHashCode();

            success1.Should().NotBe(fail1);
            success1.Should().NotBe(fail2);

            fail1.Should().NotBe(fail2);
        }
    }
}

using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results.UnitTests;

public class Result_struct
{
    [Fact]
    public void Default_value_is_error_result()
    {
        var result = default(Result);

        result.Type.Should().Be(Fail);
        result.IsFail.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.IsDefault.Should().BeTrue();
        result.Error().Should().BeSameAs(Error.DefaultError);
    }

    public class Create
    {
        [Fact]
        public void Success_Returns_success_result_with_specified_value()
        {
            var result = Result.Create.Success();

            result.Type.Should().Be(Success);
            result.IsSuccess.Should().BeTrue();
            result.IsFail.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
        }

        [Fact]
        public void Fail_Returns_fail_result_with_specified_error()
        {
            var error = new Error();
            var result = Result.Create.Fail(error);

            result.Type.Should().Be(Fail);
            result.IsFail.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
            result.Error().Should().BeSameAs(error);
        }
    }

    public class Match
    {
        [Fact]
        public void Given_nonvoid_functions_When_IsSuccess_Returns_success_function_evaluation()
        {
            var result = Result.Create.Success();

            var actual = result.Match(
                () => 1,
                error => -1);

            actual.Should().Be(1);
        }

        [Fact]
        public void Given_nonvoid_functions_When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Result.Create.Fail();

            var actual = result.Match(
                () => 1,
                error => -1);

            actual.Should().Be(-1);
        }

        [Fact]
        public void Given_void_functions_When_IsSuccess_Returns_success_function_evaluation()
        {
            var result = Result.Create.Success();

            int? successValue = null;
            Error? failError = null;

            result.Match(
                () => successValue = 1,
                error => failError = error);

            successValue.Should().Be(1);
            failError.Should().BeNull();
        }

        [Fact]
        public void Given_void_functions_When_IsFail_Returns_fail_function_evaluation()
        {
            var error = new Error();
            var result = Result.Create.Fail(error);

            int? successValue = null;
            Error? failError = null;

            result.Match(
                () => successValue = 1,
                error => failError = error);

            successValue.Should().BeNull();
            failError.Should().BeSameAs(error);
        }
    }

    public new class Equals
    {
        [Fact]
        public void Given_IsSuccess_When_other_IsSuccess_Returns_true()
        {
            var result = Result.Create.Success();
            object other = Result.Create.Success();

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_equal_error_Returns_true()
        {
            var error = new Error("a", "b", 1, "c", "d");
            var result = Result.Create.Fail(error);
            var otherError = new Error("a", "b", 1, "c", "d");
            object other = Result.Create.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void When_other_is_null_Returns_false()
        {
            var result = Result.Create.Success();
            object? other = null;

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSuccess_When_other_IsFail_Returns_false()
        {
            var result = Result.Create.Success();
            object other = Result.Create.Fail();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsSuccess_Returns_false()
        {
            var result = Result.Create.Fail();
            object other = Result.Create.Success();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsFail_with_unequal_error_Returns_false()
        {
            var error = new Error("a", "b", 1, "c", "d");
            var result = Result.Create.Fail(error);
            var otherError = new Error("w", "x", 2, "y", "z");
            object other = Result.Create.Fail(otherError);

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }
    }

    public new class GetHashCode
    {
        [Fact]
        public void Different_results_return_different_values()
        {
            var success1 = Result.Create.Success().GetHashCode();
            var fail1 = Result.Create.Fail("X").GetHashCode();
            var fail2 = Result.Create.Fail("Y").GetHashCode();

            success1.Should().NotBe(fail1);
            success1.Should().NotBe(fail2);

            fail1.Should().NotBe(fail2);
        }
    }
}
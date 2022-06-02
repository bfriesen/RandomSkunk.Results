using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results.UnitTests;

public class Maybe_of_T_struct
{
    [Fact]
    public void Default_value_is_error_result()
    {
        var result = default(Maybe<int>);

        result._type.Should().Be(Fail);
        result.IsFail.Should().BeTrue();
        result.IsSome.Should().BeFalse();
        result.IsDefault.Should().BeTrue();
        result.Error().Should().BeSameAs(Error.DefaultError);
    }

    public class Create
    {
        [Fact]
        public void Some_Returns_some_result_with_specified_value()
        {
            var result = 1.ToMaybe();

            result._type.Should().Be(Some);
            result.IsSome.Should().BeTrue();
            result.IsFail.Should().BeFalse();
            result.IsNone.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
            result._value.Should().Be(1);
        }

        [Fact]
        public void Fail_Returns_fail_result_with_specified_error()
        {
            var error = new Error();
            var result = Maybe<int>.Fail(error);

            result._type.Should().Be(Fail);
            result.IsFail.Should().BeTrue();
            result.IsSome.Should().BeFalse();
            result.IsNone.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
            result.Error().Should().BeSameAs(error);
        }

        [Fact]
        public void None_Returns_none_result()
        {
            var result = Maybe<int>.None();

            result._type.Should().Be(None);
            result.IsNone.Should().BeTrue();
            result.IsSome.Should().BeFalse();
            result.IsFail.Should().BeFalse();
            result.IsDefault.Should().BeFalse();
        }
    }

    public class Match
    {
        [Fact]
        public void Given_nonvoid_functions_When_IsSome_Returns_some_function_evaluation()
        {
            var result = 1.ToMaybe();

            var actual = result.Match(
                value => value + 1,
                () => 0,
                error => -1);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_nonvoid_functions_When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Maybe<int>.Fail();

            var actual = result.Match(
                value => value + 1,
                () => 0,
                error => -1);

            actual.Should().Be(-1);
        }

        [Fact]
        public void Given_nonvoid_functions_When_IsNone_Returns_none_function_evaluation()
        {
            var result = Maybe<int>.None();

            var actual = result.Match(
                value => value + 1,
                () => 0,
                error => -1);

            actual.Should().Be(0);
        }

        [Fact]
        public void Given_void_functions_When_IsSome_Returns_some_function_evaluation()
        {
            var result = 1.ToMaybe();

            int? someValue = null;
            Error? failError = null;
            object? none = null;

            result.Match(
                value => { someValue = value; },
                () => { none = new object(); },
                error => { failError = error; });

            someValue.Should().Be(1);
            failError.Should().BeNull();
            none.Should().BeNull();
        }

        [Fact]
        public void Given_void_functions_When_IsFail_Returns_fail_function_evaluation()
        {
            var error = new Error();
            var result = Maybe<int>.Fail(error);

            int? someValue = null;
            Error? failError = null;
            object? none = null;

            result.Match(
                value => { someValue = value; },
                () => { none = new object(); },
                error => { failError = error; });

            someValue.Should().BeNull();
            failError.Should().BeSameAs(error);
            none.Should().BeNull();
        }

        [Fact]
        public void Given_void_functions_When_IsNone_Returns_none_function_evaluation()
        {
            var error = new Error();
            var result = Maybe<int>.None();

            int? someValue = null;
            Error? failError = null;
            object? none = null;

            result.Match(
                value => { someValue = value; },
                () => { none = new object(); },
                error => { failError = error; });

            none.Should().NotBeNull();
            someValue.Should().BeNull();
            failError.Should().BeNull();
        }
    }

    public class MatchAsync
    {
        [Fact]
        public async Task Given_nonvoid_functions_When_IsSome_Returns_some_function_evaluation()
        {
            var result = 1.ToMaybe();

            var actual = await result.MatchAsync(
                value => Task.FromResult(value + 1),
                () => Task.FromResult(0),
                error => Task.FromResult(-1));

            actual.Should().Be(2);
        }

        [Fact]
        public async Task Given_nonvoid_functions_When_IsFail_Returns_fail_function_evaluation()
        {
            var result = Maybe<int>.Fail();

            var actual = await result.MatchAsync(
                value => Task.FromResult(value + 1),
                () => Task.FromResult(0),
                error => Task.FromResult(-1));

            actual.Should().Be(-1);
        }

        [Fact]
        public async Task Given_nonvoid_functions_When_IsNone_Returns_none_function_evaluation()
        {
            var result = Maybe<int>.None();

            var actual = await result.MatchAsync(
                value => Task.FromResult(value + 1),
                () => Task.FromResult(0),
                error => Task.FromResult(-1));

            actual.Should().Be(0);
        }

        [Fact]
        public async Task Given_void_functions_When_IsSome_Returns_some_function_evaluation()
        {
            var result = 1.ToMaybe();

            int? someValue = null;
            Error? failError = null;
            object? none = null;

            await result.MatchAsync(
                value =>
                {
                    someValue = value;
                    return Task.CompletedTask;
                },
                () =>
                {
                    none = new object();
                    return Task.CompletedTask;
                },
                error =>
                {
                    failError = error;
                    return Task.CompletedTask;
                });

            someValue.Should().Be(1);
            failError.Should().BeNull();
            none.Should().BeNull();
        }

        [Fact]
        public async Task Given_void_functions_When_IsFail_Returns_fail_function_evaluation()
        {
            var error = new Error();
            var result = Maybe<int>.Fail(error);

            int? someValue = null;
            Error? failError = null;
            object? none = null;

            await result.MatchAsync(
                value =>
                {
                    someValue = value;
                    return Task.CompletedTask;
                },
                () =>
                {
                    none = new object();
                    return Task.CompletedTask;
                },
                error =>
                {
                    failError = error;
                    return Task.CompletedTask;
                });

            someValue.Should().BeNull();
            failError.Should().BeSameAs(error);
            none.Should().BeNull();
        }

        [Fact]
        public async Task Given_void_functions_When_IsNone_Returns_none_function_evaluation()
        {
            var result = Maybe<int>.None();

            int? someValue = null;
            Error? failError = null;
            object? none = null;

            await result.MatchAsync(
                value =>
                {
                    someValue = value;
                    return Task.CompletedTask;
                },
                () =>
                {
                    none = new object();
                    return Task.CompletedTask;
                },
                error =>
                {
                    failError = error;
                    return Task.CompletedTask;
                });

            none.Should().NotBeNull();
            someValue.Should().BeNull();
            failError.Should().BeNull();
        }
    }

    public new class Equals
    {
        [Fact]
        public void Given_IsSome_When_other_IsSome_with_same_value_Returns_true()
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
        public void Given_IsSome_When_other_IsSome_with_different_value_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = 2.ToMaybe();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSome_When_other_IsFail_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = Maybe<int>.Fail();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsSome_When_other_IsNone_Returns_false()
        {
            var result = 1.ToMaybe();
            object other = Maybe<int>.None();

            var actual = result.Equals(other);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_IsFail_When_other_IsSome_Returns_false()
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
        public void Given_IsSome_When_other_is_different_value_Returns_false()
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
            var some1 = 1.ToMaybe().GetHashCode();
            var someA = "1".ToMaybe().GetHashCode();
            var fail1 = Maybe<int>.Fail("X").GetHashCode();
            var fail2 = Maybe<int>.Fail("Y").GetHashCode();
            var failA = Maybe<string>.Fail("X").GetHashCode();
            var failB = Maybe<string>.Fail("Y").GetHashCode();
            var none1 = Maybe<int>.None().GetHashCode();
            var noneA = Maybe<string>.None().GetHashCode();

            some1.Should().NotBe(someA);
            some1.Should().NotBe(fail1);
            some1.Should().NotBe(fail2);
            some1.Should().NotBe(failA);
            some1.Should().NotBe(failB);
            some1.Should().NotBe(none1);
            some1.Should().NotBe(noneA);

            someA.Should().NotBe(fail1);
            someA.Should().NotBe(fail2);
            someA.Should().NotBe(failA);
            someA.Should().NotBe(failB);
            someA.Should().NotBe(none1);
            someA.Should().NotBe(noneA);

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

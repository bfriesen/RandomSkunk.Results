using System.Collections.Generic;
using System.Linq;

namespace RandomSkunk.Results.UnitTests;

public class Tuple_extension_methods
{
    public static IEnumerable<object[]> NonSuccessResultsWithoutExpectedError
    {
        get
        {
            yield return new object[] { Result.Fail("a", 1), Result<int>.Success(123), Maybe<int>.Success(456) };
            yield return new object[] { Result.Success(), Result<int>.Fail("b", 2), Maybe<int>.Success(456) };
            yield return new object[] { Result.Success(), Result<int>.Success(123), Maybe<int>.Fail("c", 3) };
            yield return new object[] { Result.Success(), Result<int>.Success(123), Maybe<int>.None() };
            yield return new object[] { Result.Fail("a", 1), Result<int>.Fail("b", 2), Maybe<int>.Fail("c", 3) };
        }
    }

    public static IEnumerable<object[]> NonSuccessResultsWithExpectedError =>
        NonSuccessResultsWithoutExpectedError.Select((testData, index) =>
            new object[]
            {
                testData[0],
                testData[1],
                testData[2],
                index switch
                {
                    0 => new Error("a") { ErrorCode = 1 },
                    1 => new Error("b") { ErrorCode = 2 },
                    2 => new Error("c") { ErrorCode = 3 },
                    3 => Errors.ResultIsNone(),
                    _ => CompositeError.CreateOrGetSingle(new[] { new Error("a") { ErrorCode = 1 }, new Error("b") { ErrorCode = 2 }, new Error("c") { ErrorCode = 3 } }),
                },
            });

    public class For_OnAllSuccess
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithoutExpectedError;

        [Fact]
        public void When_all_Success_Invokes_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            (DBNull?, int?, int?) capturedValues = default;

            (resultA, resultB, resultC).OnAllSuccess(
                (a, b, c) => capturedValues = (a, b, c));

            capturedValues.Should().Be((DBNull.Value, 123, 456));
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Does_not_invoke_callback(Result resultA, Result<int> resultB, Maybe<int> resultC)
        {
            (DBNull?, int?, int?) capturedValues = default;

            (resultA, resultB, resultC).OnAllSuccess(
                (a, b, c) => capturedValues = (a, b, c));

            capturedValues.Should().Be(default);
        }
    }

    public class For_OnAllSuccessAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithoutExpectedError;

        [Fact]
        public async Task When_all_Success_Invokes_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            (DBNull?, int?, int?) capturedValues = default;

            await (resultA, resultB, resultC).OnAllSuccessAsync(
                (a, b, c) =>
                {
                    capturedValues = (a, b, c);
                    return Task.CompletedTask;
                });

            capturedValues.Should().Be((DBNull.Value, 123, 456));
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Does_not_invoke_callback(Result resultA, Result<int> resultB, Maybe<int> resultC)
        {
            (DBNull?, int?, int?) capturedValues = default;

            await (resultA, resultB, resultC).OnAllSuccessAsync(
                (a, b, c) =>
                {
                    capturedValues = (a, b, c);
                    return Task.CompletedTask;
                });

            capturedValues.Should().Be(default);
        }
    }

    public class For_OnAnyNonSuccess
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public void When_all_Success_Does_not_invoke_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            Error? capturedError = null;

            (resultA, resultB, resultC).OnAnyNonSuccess(error => capturedError = error);

            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Invokes_callback(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            Error? capturedError = null;

            (resultA, resultB, resultC).OnAnyNonSuccess(error => capturedError = error);

            capturedError.Should().NotBeNull();
            capturedError!.Message.Should().Be(expectedError.Message);
            capturedError.ErrorCode.Should().Be(expectedError.ErrorCode);
            capturedError.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var capturedCompositeError = (CompositeError)capturedError;
                capturedCompositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    capturedCompositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    capturedCompositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_OnAnyNonSuccessAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public async Task When_all_Success_Does_not_invoke_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            Error? capturedError = null;

            await (resultA, resultB, resultC).OnAnyNonSuccessAsync(error =>
            {
                capturedError = error;
                return Task.CompletedTask;
            });

            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Invokes_callback(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            Error? capturedError = null;

            await (resultA, resultB, resultC).OnAnyNonSuccessAsync(error =>
            {
                capturedError = error;
                return Task.CompletedTask;
            });

            capturedError.Should().NotBeNull();
            capturedError!.Message.Should().Be(expectedError.Message);
            capturedError.ErrorCode.Should().Be(expectedError.ErrorCode);
            capturedError.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var capturedCompositeError = (CompositeError)capturedError;
                capturedCompositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    capturedCompositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    capturedCompositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_Match
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public void When_all_Success_Invokes_onAllSuccess_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            (DBNull?, int?, int?) capturedValues = default;
            Error? capturedError = null;

            var returnedValue = (resultA, resultB, resultC).Match(
                (a, b, c) =>
                {
                    capturedValues = (a, b, c);
                    return 1;
                },
                error =>
                {
                    capturedError = error;
                    return -1;
                });

            returnedValue.Should().Be(1);
            capturedValues.Should().Be((DBNull.Value, 123, 456));
            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Invokes_onAnyNonSuccess_callback(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            (DBNull?, int?, int?) capturedValues = default;
            Error? capturedError = null;

            var returnedValue = (resultA, resultB, resultC).Match(
                (a, b, c) =>
                {
                    capturedValues = (a, b, c);
                    return 1;
                },
                error =>
                {
                    capturedError = error;
                    return -1;
                });

            returnedValue.Should().Be(-1);
            capturedValues.Should().Be(default);
            capturedError.Should().NotBeNull();
            capturedError!.Message.Should().Be(expectedError.Message);
            capturedError.ErrorCode.Should().Be(expectedError.ErrorCode);
            capturedError.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var capturedCompositeError = (CompositeError)capturedError;
                capturedCompositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    capturedCompositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    capturedCompositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_MatchAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public async Task When_all_Success_Invokes_onAllSuccess_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            (DBNull?, int?, int?) capturedValues = default;
            Error? capturedError = null;

            var returnedValue = await (resultA, resultB, resultC).MatchAsync(
                (a, b, c) =>
                {
                    capturedValues = (a, b, c);
                    return Task.FromResult(1);
                },
                error =>
                {
                    capturedError = error;
                    return Task.FromResult(-1);
                });

            returnedValue.Should().Be(1);
            capturedValues.Should().Be((DBNull.Value, 123, 456));
            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Invokes_onAnyNonSuccess_callback(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            (DBNull?, int?, int?) capturedValues = default;
            Error? capturedError = null;

            var returnedValue = await (resultA, resultB, resultC).MatchAsync(
                (a, b, c) =>
                {
                    capturedValues = (a, b, c);
                    return Task.FromResult(1);
                },
                error =>
                {
                    capturedError = error;
                    return Task.FromResult(-1);
                });

            returnedValue.Should().Be(-1);
            capturedValues.Should().Be(default);
            capturedError.Should().NotBeNull();
            capturedError!.Message.Should().Be(expectedError.Message);
            capturedError.ErrorCode.Should().Be(expectedError.ErrorCode);
            capturedError.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var capturedCompositeError = (CompositeError)capturedError;
                capturedCompositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    capturedCompositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    capturedCompositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_Select
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public void When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = (resultA, resultB, resultC).Select((a, b, c) => b + c);

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(579);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = (resultA, resultB, resultC).Select((a, b, c) => b + c);

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_SelectAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public async Task When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = await (resultA, resultB, resultC).SelectAsync((a, b, c) => Task.FromResult(b + c));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(579);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = await (resultA, resultB, resultC).SelectAsync((a, b, c) => Task.FromResult(b + c));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_SelectMany
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public void Given_return_type_is_Result_When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = (resultA, resultB, resultC).SelectMany((a, b, c) => Result.Fail("x", -1));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be("x");
            actual.Error.ErrorCode.Should().Be(-1);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void Given_return_type_is_Result_When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = (resultA, resultB, resultC).SelectMany((a, b, c) => Result.Success());

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }

        [Fact]
        public void Given_return_type_is_Result_of_T_When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = (resultA, resultB, resultC).SelectMany((a, b, c) => Result<int>.Success(b + c));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(579);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void Given_return_type_is_Result_of_T_When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = (resultA, resultB, resultC).SelectMany((a, b, c) => Result<int>.Success(b + c));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }

        [Fact]
        public void Given_return_type_is_Maybe_of_T_When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = (resultA, resultB, resultC).SelectMany((a, b, c) => Maybe<int>.Success(b + c));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(579);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void Given_return_type_is_Maybe_of_T_When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = (resultA, resultB, resultC).SelectMany((a, b, c) => Maybe<int>.Success(b + c));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }

    public class For_Async_SelectMany
    {
        public static IEnumerable<object[]> NonSuccessResults => NonSuccessResultsWithExpectedError;

        [Fact]
        public async Task Given_return_type_is_Result_When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = await (resultA, resultB, resultC).SelectMany((a, b, c) => Task.FromResult(Result.Fail("x", -1)));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be("x");
            actual.Error.ErrorCode.Should().Be(-1);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task Given_return_type_is_Result_When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = await (resultA, resultB, resultC).SelectMany((a, b, c) => Task.FromResult(Result.Success()));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }

        [Fact]
        public async Task Given_return_type_is_Result_of_T_When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = await (resultA, resultB, resultC).SelectMany((a, b, c) => Task.FromResult(Result<int>.Success(b + c)));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(579);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task Given_return_type_is_Result_of_T_When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = await (resultA, resultB, resultC).SelectMany((a, b, c) => Task.FromResult(Result<int>.Success(b + c)));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }

        [Fact]
        public async Task Given_return_type_is_Maybe_of_T_When_all_Success_Returns_Success_result_from_function_evaluation()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var actual = await (resultA, resultB, resultC).SelectMany((a, b, c) => Task.FromResult(Maybe<int>.Success(b + c)));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(579);
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task Given_return_type_is_Maybe_of_T_When_not_all_Success_Returns_Fail_result(Result resultA, Result<int> resultB, Maybe<int> resultC, Error expectedError)
        {
            var actual = await (resultA, resultB, resultC).SelectMany((a, b, c) => Task.FromResult(Maybe<int>.Success(b + c)));

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Should().BeOfType(expectedError.GetType());

            if (expectedError is CompositeError expectedCompositeError)
            {
                var compositeError = (CompositeError)actual.Error;
                compositeError.Errors.Should().HaveSameCount(expectedCompositeError.Errors);
                for (int i = 0; i < expectedCompositeError.Errors.Count; i++)
                {
                    compositeError.Errors[i].Message.Should().Be(expectedCompositeError.Errors[i].Message);
                    compositeError.Errors[i].ErrorCode.Should().Be(expectedCompositeError.Errors[i].ErrorCode);
                }
            }
        }
    }
}

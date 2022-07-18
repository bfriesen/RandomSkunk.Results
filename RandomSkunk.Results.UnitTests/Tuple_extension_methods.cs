using System.Collections.Generic;
using System.Linq;

namespace RandomSkunk.Results.UnitTests;

public class Tuple_extension_methods
{
    public static IEnumerable<object[]> GenericNonSuccessResults
    {
        get
        {
            yield return new object[] { Result<int>.Fail("a", 1), Maybe<int>.Success(456) };
            yield return new object[] { Result<int>.Success(123), Maybe<int>.Fail("b", 2) };
            yield return new object[] { Result<int>.Success(123), Maybe<int>.None() };
            yield return new object[] { Result<int>.Fail("a", 1), Maybe<int>.Fail("b", 2) };
        }
    }

    public static IEnumerable<object[]> NonGenericNonSuccessResults
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

    public static IEnumerable<object[]> GenericNonSuccessResultsWithExpectedError =>
        GenericNonSuccessResults.Select((testData, index) =>
            new object[]
            {
                testData[0],
                testData[1],
                index switch
                {
                    0 => new Error("a") { ErrorCode = 1 },
                    1 => new Error("b") { ErrorCode = 2 },
                    2 => new Error("Not Found") { ErrorCode = ErrorCodes.NotFound },
                    _ => CompositeError.Create(new[] { new Error("a") { ErrorCode = 1 }, new Error("b") { ErrorCode = 2 } }),
                },
            });

    public static IEnumerable<object[]> NonGenericNonSuccessResultsWithExpectedError =>
        NonGenericNonSuccessResults.Select((testData, index) =>
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
                    3 => new Error("Not Found") { ErrorCode = ErrorCodes.NotFound },
                    _ => CompositeError.Create(new[] { new Error("a") { ErrorCode = 1 }, new Error("b") { ErrorCode = 2 }, new Error("c") { ErrorCode = 3 } }),
                },
            });

    public class For_generic_OnAllSuccess
    {
        public static IEnumerable<object[]> NonSuccessResults => GenericNonSuccessResults;

        [Fact]
        public void When_all_Success_Invokes_callback()
        {
            var resultA = Result<int>.Success(123);
            var resultB = Maybe<int>.Success(456);

            var capturedValues = (0, 0);

            (resultA, resultB).OnAllSuccess(
                (a, b) => capturedValues = (a, b));

            capturedValues.Should().Be((123, 456));
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Does_not_invoke_callback(IResult<int> resultA, IResult<int> resultB)
        {
            var capturedValues = (0, 0);

            (resultA, resultB).OnAllSuccess(
                (a, b) => capturedValues = (a, b));

            capturedValues.Should().Be((0, 0));
        }
    }

    public class For_generic_OnAllSuccessAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => GenericNonSuccessResults;

        [Fact]
        public async Task When_all_Success_Invokes_callback()
        {
            var resultA = Result<int>.Success(123);
            var resultB = Maybe<int>.Success(456);

            var capturedValues = (0, 0);

            await (resultA, resultB).OnAllSuccessAsync(
                (a, b) =>
                {
                    capturedValues = (a, b);
                    return Task.CompletedTask;
                });

            capturedValues.Should().Be((123, 456));
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Does_not_invoke_callback(IResult<int> resultA, IResult<int> resultB)
        {
            var capturedValues = (0, 0);

            await (resultA, resultB).OnAllSuccessAsync(
                (a, b) =>
                {
                    capturedValues = (a, b);
                    return Task.CompletedTask;
                });

            capturedValues.Should().Be((0, 0));
        }
    }

    public class For_non_generic_OnAllSuccess
    {
        public static IEnumerable<object[]> NonSuccessResults => NonGenericNonSuccessResults;

        [Fact]
        public void When_all_Success_Invokes_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);

            (resultA, resultB, resultC).OnAllSuccess((a, b, c) => capturedValues = (a, b, c));

            capturedValues.Should().Be((DBNull.Value, 123, 456));
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Does_not_invoke_callback(IResult resultA, IResult resultB, IResult resultC)
        {
            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);

            (resultA, resultB, resultC).OnAllSuccess((a, b, c) => capturedValues = (a, b, c));

            capturedValues.Item1.Should().Be(DBNull.Value);
            capturedValues.Item2.Should().Be(0);
            capturedValues.Item3.Should().Be(0);
        }
    }

    public class For_non_generic_OnAllSuccessAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => NonGenericNonSuccessResults;

        [Fact]
        public async Task When_all_Success_Invokes_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);

            await (resultA, resultB, resultC).OnAllSuccessAsync((a, b, c) =>
            {
                capturedValues = (a, b, c);
                return Task.CompletedTask;
            });

            capturedValues.Should().Be((DBNull.Value, 123, 456));
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Does_not_invoke_callback(IResult resultA, IResult resultB, IResult resultC)
        {
            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);

            await (resultA, resultB, resultC).OnAllSuccessAsync((a, b, c) =>
            {
                capturedValues = (a, b, c);
                return Task.CompletedTask;
            });

            capturedValues.Item1.Should().Be(DBNull.Value);
            capturedValues.Item2.Should().Be(0);
            capturedValues.Item3.Should().Be(0);
        }
    }

    public class For_OnAnyNonSuccess
    {
        public static IEnumerable<object[]> NonSuccessResults => NonGenericNonSuccessResultsWithExpectedError;

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
        public void When_not_all_Success_Invokes_callback(IResult resultA, IResult resultB, IResult resultC, Error expectedError)
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
        public static IEnumerable<object[]> NonSuccessResults => NonGenericNonSuccessResultsWithExpectedError;

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
        public async Task When_not_all_Success_Invokes_callback(IResult resultA, IResult resultB, IResult resultC, Error expectedError)
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

    public class For_generic_MatchAll
    {
        public static IEnumerable<object[]> NonSuccessResults => GenericNonSuccessResultsWithExpectedError;

        [Fact]
        public void When_all_Success_Invokes_onAllSuccess_callback()
        {
            var resultA = Result<int>.Success(123);
            var resultB = Maybe<int>.Success(456);

            var capturedValues = (0, 0);
            Error? capturedError = null;

            var expected = (resultA, resultB).MatchAll(
                (a, b) =>
                {
                    capturedValues = (a, b);
                    return 1;
                },
                error =>
                {
                    capturedError = error;
                    return -1;
                });

            expected.Should().Be(1);
            capturedValues.Should().Be((123, 456));
            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Invokes_onAnyNonSuccess_callback(IResult<int> resultA, IResult<int> resultB, Error expectedError)
        {
            var capturedValues = (0, 0);
            Error? capturedError = null;

            var expected = (resultA, resultB).MatchAll(
                (a, b) =>
                {
                    capturedValues = (a, b);
                    return 1;
                },
                error =>
                {
                    capturedError = error;
                    return -1;
                });

            expected.Should().Be(-1);
            capturedValues.Should().Be((0, 0));
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

    public class For_generic_MatchAllAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => GenericNonSuccessResultsWithExpectedError;

        [Fact]
        public async Task When_all_Success_Invokes_onAllSuccess_callback()
        {
            var resultA = Result<int>.Success(123);
            var resultB = Maybe<int>.Success(456);

            var capturedValues = (0, 0);
            Error? capturedError = null;

            var expected = await (resultA, resultB).MatchAllAsync(
                (a, b) =>
                {
                    capturedValues = (a, b);
                    return Task.FromResult(1);
                },
                error =>
                {
                    capturedError = error;
                    return Task.FromResult(-1);
                });

            expected.Should().Be(1);
            capturedValues.Should().Be((123, 456));
            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Invokes_onAnyNonSuccess_callback(IResult<int> resultA, IResult<int> resultB, Error expectedError)
        {
            var capturedValues = (0, 0);
            Error? capturedError = null;

            var expected = await (resultA, resultB).MatchAllAsync(
                (a, b) =>
                {
                    capturedValues = (a, b);
                    return Task.FromResult(1);
                },
                error =>
                {
                    capturedError = error;
                    return Task.FromResult(-1);
                });

            expected.Should().Be(-1);
            capturedValues.Should().Be((0, 0));
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

    public class For_non_generic_MatchAll
    {
        public static IEnumerable<object[]> NonSuccessResults => NonGenericNonSuccessResultsWithExpectedError;

        [Fact]
        public void When_all_Success_Invokes_onAllSuccess_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);
            Error? capturedError = null;

            var expected = (resultA, resultB, resultC).MatchAll(
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

            expected.Should().Be(1);
            capturedValues.Should().Be((DBNull.Value, 123, 456));
            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public void When_not_all_Success_Invokes_onAnyNonSuccess_callback(IResult resultA, IResult resultB, IResult resultC, Error expectedError)
        {
            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);
            Error? capturedError = null;

            var expected = (resultA, resultB, resultC).MatchAll(
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

            expected.Should().Be(-1);
            capturedValues.Item1.Should().Be(DBNull.Value);
            capturedValues.Item2.Should().Be(0);
            capturedValues.Item3.Should().Be(0);
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

    public class For_non_generic_MatchAllAsync
    {
        public static IEnumerable<object[]> NonSuccessResults => NonGenericNonSuccessResultsWithExpectedError;

        [Fact]
        public async Task When_all_Success_Invokes_onAllSuccess_callback()
        {
            var resultA = Result.Success();
            var resultB = Result<int>.Success(123);
            var resultC = Maybe<int>.Success(456);

            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);
            Error? capturedError = null;

            var expected = await (resultA, resultB, resultC).MatchAllAsync(
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

            expected.Should().Be(1);
            capturedValues.Should().Be((DBNull.Value, 123, 456));
            capturedError.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NonSuccessResults))]
        public async Task When_not_all_Success_Invokes_onAnyNonSuccess_callback(IResult resultA, IResult resultB, IResult resultC, Error expectedError)
        {
            var capturedValues = ((object)DBNull.Value, (object)0, (object)0);
            Error? capturedError = null;

            var expected = await (resultA, resultB, resultC).MatchAllAsync(
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

            expected.Should().Be(-1);
            capturedValues.Item1.Should().Be(DBNull.Value);
            capturedValues.Item2.Should().Be(0);
            capturedValues.Item3.Should().Be(0);
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
}

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Collections.Immutable;

namespace RandomSkunk.Results.Analyzers.Tests;

public class UnsafeResultUsageAnalyzer_class
{
    [Fact]
    public async Task GivenResultVariables_WhenNoChecks_ReportsDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result = GetResult();
                var resultOfInt = GetResultOfInt();
                var maybeOfInt = GetMaybeOfInt();

                Console.WriteLine([|result.Error|]);
                Console.WriteLine([|resultOfInt.Error|]);
                Console.WriteLine([|resultOfInt.Value|]);
                Console.WriteLine([|maybeOfInt.Error|]);
                Console.WriteLine([|maybeOfInt.Value|]);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }"
        ;

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenOutOfScopeChecks_ReportsDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();
                var maybeOfInt1 = GetMaybeOfInt();
                var maybeOfInt2 = GetMaybeOfInt();

                if (result.IsFail)
                    Console.WriteLine();

                if (resultOfInt1.IsFail)
                    Console.WriteLine();

                if (maybeOfInt1.IsFail)
                    Console.WriteLine();

                if (resultOfInt2.IsSuccess)
                    Console.WriteLine();

                if (maybeOfInt2.IsSuccess)
                    Console.WriteLine();

                Console.WriteLine([|result.Error|]);
                Console.WriteLine([|resultOfInt1.Error|]);
                Console.WriteLine([|maybeOfInt1.Error|]);
                Console.WriteLine([|resultOfInt2.Value|]);
                Console.WriteLine([|maybeOfInt2.Value|]);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenDirectlyCheckingIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();
                var maybeOfInt1 = GetMaybeOfInt();
                var maybeOfInt2 = GetMaybeOfInt();

                if (result.IsFail)
                    Console.WriteLine(result.Error);

                if (resultOfInt1.IsFail)
                    Console.WriteLine(resultOfInt1.Error);

                if (maybeOfInt1.IsFail)
                    Console.WriteLine(maybeOfInt1.Error);

                if (resultOfInt2.IsSuccess)
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt2.IsSuccess)
                    Console.WriteLine(maybeOfInt2.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenCheckingNotOppositeIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();

                if (!result.IsSuccess)
                    Console.WriteLine(result.Error);

                if (!resultOfInt1.IsSuccess)
                    Console.WriteLine(resultOfInt1.Error);

                if (!resultOfInt2.IsFail)
                    Console.WriteLine(resultOfInt2.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenCheckingIsPropertyAgainstTrue_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();
                var maybeOfInt1 = GetMaybeOfInt();
                var maybeOfInt2 = GetMaybeOfInt();

                if (result.IsFail == true)
                    Console.WriteLine(result.Error);

                if (resultOfInt1.IsFail == true)
                    Console.WriteLine(resultOfInt1.Error);

                if (maybeOfInt1.IsFail == true)
                    Console.WriteLine(maybeOfInt1.Error);

                if (resultOfInt2.IsSuccess == true)
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt2.IsSuccess == true)
                    Console.WriteLine(maybeOfInt2.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenCheckingOppositeIsPropertyAgainstFalse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();

                if (result.IsSuccess == false)
                    Console.WriteLine(result.Error);

                if (resultOfInt1.IsSuccess == false)
                    Console.WriteLine(resultOfInt1.Error);

                if (resultOfInt2.IsFail == false)
                    Console.WriteLine(resultOfInt2.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenEarlyExitingAfterCheckingNotIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1()
            {
                var result = GetResult();

                if (!result.IsFail)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2()
            {
                var result = GetResultOfInt();

                if (!result.IsFail)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3()
            {
                var result = GetResultOfInt();

                if (!result.IsSuccess)
                    return;

                Console.WriteLine(result.Value);
            }

            void Method4()
            {
                var result = GetMaybeOfInt();

                if (!result.IsFail)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method5()
            {
                var result = GetMaybeOfInt();

                if (!result.IsSuccess)
                    return;

                Console.WriteLine(result.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenEarlyExitingAfterCheckingOppositeIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1()
            {
                var result = GetResult();

                if (result.IsSuccess)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2()
            {
                var result = GetResultOfInt();

                if (result.IsSuccess)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3()
            {
                var result = GetResultOfInt();

                if (result.IsFail)
                    return;

                Console.WriteLine(result.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenEarlyExitingAfterCheckingIsPropertyAgainstFalse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1()
            {
                var result = GetResult();

                if (result.IsFail == false)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2()
            {
                var result = GetResultOfInt();

                if (result.IsFail == false)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3()
            {
                var result = GetResultOfInt();

                if (result.IsSuccess == false)
                    return;

                Console.WriteLine(result.Value);
            }

            void Method4()
            {
                var result = GetMaybeOfInt();

                if (result.IsFail == false)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method5()
            {
                var result = GetMaybeOfInt();

                if (result.IsSuccess == false)
                    return;

                Console.WriteLine(result.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenEarlyExitingAfterCheckingOppositeIsPropertyAgainstTrue_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1()
            {
                var result = GetResult();

                if (result.IsSuccess == true)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2()
            {
                var result = GetResultOfInt();

                if (result.IsSuccess == true)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3()
            {
                var result = GetResultOfInt();

                if (result.IsFail == true)
                    return;

                Console.WriteLine(result.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenUsingIsPropertiesInIfElse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result1 = GetResult();
                var result2 = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();
                var maybeOfInt1 = GetMaybeOfInt();
                var maybeOfInt2 = GetMaybeOfInt();
                var maybeOfInt3 = GetMaybeOfInt();
                var maybeOfInt4 = GetMaybeOfInt();
                var maybeOfInt5 = GetMaybeOfInt();
                var maybeOfInt6 = GetMaybeOfInt();

                if (result1.IsSuccess)
                    Console.WriteLine(""Success"");
                else
                    Console.WriteLine(result1.Error);

                if (result2.IsFail)
                    Console.WriteLine(result2.Error);
                else
                    Console.WriteLine(""Success"");

                if (resultOfInt1.IsSuccess)
                    Console.WriteLine(resultOfInt1.Value);
                else
                    Console.WriteLine(resultOfInt1.Error);

                if (resultOfInt2.IsFail)
                    Console.WriteLine(resultOfInt2.Error);
                else
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt1.IsSuccess)
                    Console.WriteLine(maybeOfInt1.Value);
                else if (maybeOfInt1.IsNone)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt1.Error);

                if (maybeOfInt2.IsSuccess)
                    Console.WriteLine(maybeOfInt2.Value);
                else if (maybeOfInt2.IsFail)
                    Console.WriteLine(maybeOfInt2.Error);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt3.IsFail)
                    Console.WriteLine(maybeOfInt3.Error);
                else if (maybeOfInt3.IsSuccess)
                    Console.WriteLine(maybeOfInt3.Value);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt4.IsFail)
                    Console.WriteLine(maybeOfInt4.Error);
                else if (maybeOfInt4.IsNone)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt4.Value);

                if (maybeOfInt5.IsNone)
                    Console.WriteLine(""None"");
                else if (maybeOfInt5.IsSuccess)
                    Console.WriteLine(maybeOfInt5.Value);
                else
                    Console.WriteLine(maybeOfInt5.Error);

                if (maybeOfInt6.IsNone)
                    Console.WriteLine(""None"");
                else if (maybeOfInt6.IsFail)
                    Console.WriteLine(maybeOfInt6.Error);
                else
                    Console.WriteLine(maybeOfInt6.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultVariables_WhenUsingIsPropertiesAgainstTrueInIfElse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var result1 = GetResult();
                var result2 = GetResult();
                var resultOfInt1 = GetResultOfInt();
                var resultOfInt2 = GetResultOfInt();
                var maybeOfInt1 = GetMaybeOfInt();
                var maybeOfInt2 = GetMaybeOfInt();
                var maybeOfInt3 = GetMaybeOfInt();
                var maybeOfInt4 = GetMaybeOfInt();
                var maybeOfInt5 = GetMaybeOfInt();
                var maybeOfInt6 = GetMaybeOfInt();

                if (result1.IsSuccess == true)
                    Console.WriteLine(""Success"");
                else
                    Console.WriteLine(result1.Error);

                if (result2.IsFail == true)
                    Console.WriteLine(result2.Error);
                else
                    Console.WriteLine(""Success"");

                if (resultOfInt1.IsSuccess == true)
                    Console.WriteLine(resultOfInt1.Value);
                else
                    Console.WriteLine(resultOfInt1.Error);

                if (resultOfInt2.IsFail == true)
                    Console.WriteLine(resultOfInt2.Error);
                else
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt1.IsSuccess == true)
                    Console.WriteLine(maybeOfInt1.Value);
                else if (maybeOfInt1.IsNone == true)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt1.Error);

                if (maybeOfInt2.IsSuccess == true)
                    Console.WriteLine(maybeOfInt2.Value);
                else if (maybeOfInt2.IsFail == true)
                    Console.WriteLine(maybeOfInt2.Error);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt3.IsFail == true)
                    Console.WriteLine(maybeOfInt3.Error);
                else if (maybeOfInt3.IsSuccess == true)
                    Console.WriteLine(maybeOfInt3.Value);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt4.IsFail == true)
                    Console.WriteLine(maybeOfInt4.Error);
                else if (maybeOfInt4.IsNone == true)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt4.Value);

                if (maybeOfInt5.IsNone == true)
                    Console.WriteLine(""None"");
                else if (maybeOfInt5.IsSuccess == true)
                    Console.WriteLine(maybeOfInt5.Value);
                else
                    Console.WriteLine(maybeOfInt5.Error);

                if (maybeOfInt6.IsNone == true)
                    Console.WriteLine(""None"");
                else if (maybeOfInt6.IsFail == true)
                    Console.WriteLine(maybeOfInt6.Error);
                else
                    Console.WriteLine(maybeOfInt6.Value);
            }

            Result GetResult() => Errors.NotImplemented();

            Result<int> GetResultOfInt() => Errors.NotImplemented();

            Maybe<int> GetMaybeOfInt() => Errors.NotImplemented();
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenNoChecks_ReportsDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result, Result<int> resultOfInt, Maybe<int> maybeOfInt)
            {
                Console.WriteLine([|result.Error|]);
                Console.WriteLine([|resultOfInt.Error|]);
                Console.WriteLine([|resultOfInt.Value|]);
                Console.WriteLine([|maybeOfInt.Error|]);
                Console.WriteLine([|maybeOfInt.Value|]);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenOutOfScopeChecks_ReportsDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result, Result<int> resultOfInt1, Result<int> resultOfInt2, Maybe<int> maybeOfInt1, Maybe<int> maybeOfInt2)
            {
                if (result.IsFail)
                    Console.WriteLine();

                if (resultOfInt1.IsFail)
                    Console.WriteLine();

                if (maybeOfInt1.IsFail)
                    Console.WriteLine();

                if (resultOfInt2.IsSuccess)
                    Console.WriteLine();

                if (maybeOfInt2.IsSuccess)
                    Console.WriteLine();

                Console.WriteLine([|result.Error|]);
                Console.WriteLine([|resultOfInt1.Error|]);
                Console.WriteLine([|maybeOfInt1.Error|]);
                Console.WriteLine([|resultOfInt2.Value|]);
                Console.WriteLine([|maybeOfInt2.Value|]);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenDirectlyCheckingIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result, Result<int> resultOfInt1, Result<int> resultOfInt2, Maybe<int> maybeOfInt1, Maybe<int> maybeOfInt2)
            {
                if (result.IsFail)
                    Console.WriteLine(result.Error);

                if (resultOfInt1.IsFail)
                    Console.WriteLine(resultOfInt1.Error);

                if (maybeOfInt1.IsFail)
                    Console.WriteLine(maybeOfInt1.Error);

                if (resultOfInt2.IsSuccess)
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt2.IsSuccess)
                    Console.WriteLine(maybeOfInt2.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenCheckingNotOppositeIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result, Result<int> resultOfInt1, Result<int> resultOfInt2)
            {
                if (!result.IsSuccess)
                    Console.WriteLine(result.Error);

                if (!resultOfInt1.IsSuccess)
                    Console.WriteLine(resultOfInt1.Error);

                if (!resultOfInt2.IsFail)
                    Console.WriteLine(resultOfInt2.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenCheckingIsPropertyAgainstTrue_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result, Result<int> resultOfInt1, Result<int> resultOfInt2, Maybe<int> maybeOfInt1, Maybe<int> maybeOfInt2)
            {
                if (result.IsFail == true)
                    Console.WriteLine(result.Error);

                if (resultOfInt1.IsFail == true)
                    Console.WriteLine(resultOfInt1.Error);

                if (maybeOfInt1.IsFail == true)
                    Console.WriteLine(maybeOfInt1.Error);

                if (resultOfInt2.IsSuccess == true)
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt2.IsSuccess == true)
                    Console.WriteLine(maybeOfInt2.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenCheckingOppositeIsPropertyAgainstFalse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result, Result<int> resultOfInt1, Result<int> resultOfInt2)
            {
                if (result.IsSuccess == false)
                    Console.WriteLine(result.Error);

                if (resultOfInt1.IsSuccess == false)
                    Console.WriteLine(resultOfInt1.Error);

                if (resultOfInt2.IsFail == false)
                    Console.WriteLine(resultOfInt2.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenEarlyExitingAfterCheckingNotIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1(Result result)
            {
                if (!result.IsFail)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2(Result<int> result)
            {
                if (!result.IsFail)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3(Result<int> result)
            {
                if (!result.IsSuccess)
                    return;

                Console.WriteLine(result.Value);
            }

            void Method4(Maybe<int> result)
            {
                if (!result.IsFail)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method5(Maybe<int> result)
            {
                if (!result.IsSuccess)
                    return;

                Console.WriteLine(result.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenEarlyExitingAfterCheckingOppositeIsProperty_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1(Result result)
            {
                if (result.IsSuccess)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2(Result<int> result)
            {
                if (result.IsSuccess)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3(Result<int> result)
            {
                if (result.IsFail)
                    return;

                Console.WriteLine(result.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenEarlyExitingAfterCheckingIsPropertyAgainstFalse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1(Result result)
            {
                if (result.IsFail == false)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2(Result<int> result)
            {
                if (result.IsFail == false)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3(Result<int> result)
            {
                if (result.IsSuccess == false)
                    return;

                Console.WriteLine(result.Value);
            }

            void Method4(Maybe<int> result)
            {
                if (result.IsFail == false)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method5(Maybe<int> result)
            {
                if (result.IsSuccess == false)
                    return;

                Console.WriteLine(result.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenEarlyExitingAfterCheckingOppositeIsPropertyAgainstTrue_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1(Result result)
            {
                if (result.IsSuccess == true)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method2(Result<int> result)
            {
                if (result.IsSuccess == true)
                    return;

                Console.WriteLine(result.Error);
            }

            void Method3(Result<int> result)
            {
                if (result.IsFail == true)
                    return;

                Console.WriteLine(result.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenUsingIsPropertiesInIfElse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result1, Result result2,
                Result<int> resultOfInt1, Result<int> resultOfInt2,
                Maybe<int> maybeOfInt1, Maybe<int> maybeOfInt2, Maybe<int> maybeOfInt3,
                Maybe<int> maybeOfInt4, Maybe<int> maybeOfInt5, Maybe<int> maybeOfInt6)
            {
                if (result1.IsSuccess)
                    Console.WriteLine(""Success"");
                else
                    Console.WriteLine(result1.Error);

                if (result2.IsFail)
                    Console.WriteLine(result2.Error);
                else
                    Console.WriteLine(""Success"");

                if (resultOfInt1.IsSuccess)
                    Console.WriteLine(resultOfInt1.Value);
                else
                    Console.WriteLine(resultOfInt1.Error);

                if (resultOfInt2.IsFail)
                    Console.WriteLine(resultOfInt2.Error);
                else
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt1.IsSuccess)
                    Console.WriteLine(maybeOfInt1.Value);
                else if (maybeOfInt1.IsNone)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt1.Error);

                if (maybeOfInt2.IsSuccess)
                    Console.WriteLine(maybeOfInt2.Value);
                else if (maybeOfInt2.IsFail)
                    Console.WriteLine(maybeOfInt2.Error);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt3.IsFail)
                    Console.WriteLine(maybeOfInt3.Error);
                else if (maybeOfInt3.IsSuccess)
                    Console.WriteLine(maybeOfInt3.Value);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt4.IsFail)
                    Console.WriteLine(maybeOfInt4.Error);
                else if (maybeOfInt4.IsNone)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt4.Value);

                if (maybeOfInt5.IsNone)
                    Console.WriteLine(""None"");
                else if (maybeOfInt5.IsSuccess)
                    Console.WriteLine(maybeOfInt5.Value);
                else
                    Console.WriteLine(maybeOfInt5.Error);

                if (maybeOfInt6.IsNone)
                    Console.WriteLine(""None"");
                else if (maybeOfInt6.IsFail)
                    Console.WriteLine(maybeOfInt6.Error);
                else
                    Console.WriteLine(maybeOfInt6.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task GivenResultParameters_WhenUsingIsPropertiesAgainstTrueInIfElse_DoesNotReportDiagnostics()
    {
        var test = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method(Result result1, Result result2,
                Result<int> resultOfInt1, Result<int> resultOfInt2,
                Maybe<int> maybeOfInt1, Maybe<int> maybeOfInt2, Maybe<int> maybeOfInt3,
                Maybe<int> maybeOfInt4, Maybe<int> maybeOfInt5, Maybe<int> maybeOfInt6)
            {
                if (result1.IsSuccess == true)
                    Console.WriteLine(""Success"");
                else
                    Console.WriteLine(result1.Error);

                if (result2.IsFail == true)
                    Console.WriteLine(result2.Error);
                else
                    Console.WriteLine(""Success"");

                if (resultOfInt1.IsSuccess == true)
                    Console.WriteLine(resultOfInt1.Value);
                else
                    Console.WriteLine(resultOfInt1.Error);

                if (resultOfInt2.IsFail == true)
                    Console.WriteLine(resultOfInt2.Error);
                else
                    Console.WriteLine(resultOfInt2.Value);

                if (maybeOfInt1.IsSuccess == true)
                    Console.WriteLine(maybeOfInt1.Value);
                else if (maybeOfInt1.IsNone == true)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt1.Error);

                if (maybeOfInt2.IsSuccess == true)
                    Console.WriteLine(maybeOfInt2.Value);
                else if (maybeOfInt2.IsFail == true)
                    Console.WriteLine(maybeOfInt2.Error);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt3.IsFail == true)
                    Console.WriteLine(maybeOfInt3.Error);
                else if (maybeOfInt3.IsSuccess == true)
                    Console.WriteLine(maybeOfInt3.Value);
                else
                    Console.WriteLine(""None"");

                if (maybeOfInt4.IsFail == true)
                    Console.WriteLine(maybeOfInt4.Error);
                else if (maybeOfInt4.IsNone == true)
                    Console.WriteLine(""None"");
                else
                    Console.WriteLine(maybeOfInt4.Value);

                if (maybeOfInt5.IsNone == true)
                    Console.WriteLine(""None"");
                else if (maybeOfInt5.IsSuccess == true)
                    Console.WriteLine(maybeOfInt5.Value);
                else
                    Console.WriteLine(maybeOfInt5.Error);

                if (maybeOfInt6.IsNone == true)
                    Console.WriteLine(""None"");
                else if (maybeOfInt6.IsFail == true)
                    Console.WriteLine(maybeOfInt6.Error);
                else
                    Console.WriteLine(maybeOfInt6.Value);
            }
        }
    }";

        await VerifyAnalyzerAsync(test);
    }

    private static async Task VerifyAnalyzerAsync(string code)
    {
        var test = new CSharpCodeFixTest<UnsafeResultUsageAnalyzer, EmptyCodeFixProvider, XUnitVerifier>
        {
            TestCode = code,
            ReferenceAssemblies = ReferenceAssemblies.Default
                .AddPackages(ImmutableArray.Create(
                    new PackageIdentity("RandomSkunk.Results", "1.3.1"))),
        };

        await test.RunAsync();
    }
}

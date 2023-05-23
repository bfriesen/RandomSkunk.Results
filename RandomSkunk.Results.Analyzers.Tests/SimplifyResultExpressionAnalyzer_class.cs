using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Collections.Immutable;

namespace RandomSkunk.Results.Analyzers.Tests;

public class SimplifyResultExpressionAnalyzer_class
{
    [Fact]
    public async Task GivenResultVariables_WhenAndExpressionAndEachSideComparesToFalse_ReportsDiagnosticsAndFixes()
    {
        var code = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var maybe1 = GetMaybe();
                var maybe2 = GetMaybe();

                if ([|!maybe1.IsFail && !maybe1.IsNone|])
                    Console.WriteLine(""Success"");

                if ([|!maybe2.IsSuccess && !maybe2.IsNone|])
                    Console.WriteLine(""Fail"");
            }

            Maybe<int> GetMaybe() => Errors.NotImplemented();
        }
    }";

        var fixedCode = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method()
            {
                var maybe1 = GetMaybe();
                var maybe2 = GetMaybe();

                if (maybe1.IsSuccess)
                    Console.WriteLine(""Success"");

                if (maybe2.IsFail)
                    Console.WriteLine(""Fail"");
            }

            Maybe<int> GetMaybe() => Errors.NotImplemented();
        }
    }";

        await VerifyCodeFixAsync(code, fixedCode);
    }

    [Fact]
    public async Task GivenResultVariables_WhenOrExpressionAndEachSideComparesToTrue_ReportsDiagnosticsAndFixes()
    {
        var code = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1()
            {
                var maybe = GetMaybe();

                if ([|maybe.IsFail || maybe.IsNone|])
                    return;

                Console.WriteLine(""Success"");
            }

            void Method2()
            {
                var maybe = GetMaybe();

                if ([|maybe.IsSuccess || maybe.IsNone|])
                    return;

                Console.WriteLine(""Fail"");
            }

            Maybe<int> GetMaybe() => Errors.NotImplemented();
        }
    }";

        var fixedCode = @"
    using RandomSkunk.Results;
    using System;

    namespace ConsoleApplication1
    {
        class Example
        {
            void Method1()
            {
                var maybe = GetMaybe();

                if (!maybe.IsSuccess)
                    return;

                Console.WriteLine(""Success"");
            }

            void Method2()
            {
                var maybe = GetMaybe();

                if (!maybe.IsFail)
                    return;

                Console.WriteLine(""Fail"");
            }

            Maybe<int> GetMaybe() => Errors.NotImplemented();
        }
    }";

        await VerifyCodeFixAsync(code, fixedCode);
    }

    private static async Task VerifyCodeFixAsync(string code, string fixedCode)
    {
        var test = new CSharpCodeFixTest<SimplifyResultExpressionAnalyzer, SimplifyResultExpressionCodeFixProvider, XUnitVerifier>
        {
            TestCode = code,
            FixedCode = fixedCode,
            ReferenceAssemblies = ReferenceAssemblies.Default
                .AddPackages(ImmutableArray.Create(
                    new PackageIdentity("RandomSkunk.Results", "1.3.1"))),
        };

        await test.RunAsync();
    }
}

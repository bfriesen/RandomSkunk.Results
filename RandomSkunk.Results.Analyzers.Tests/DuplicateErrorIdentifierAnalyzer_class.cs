using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Collections.Immutable;

namespace RandomSkunk.Results.Analyzers.Tests;

public class DuplicateErrorIdentifierAnalyzer_class
{
    [Fact]
    public async Task GivenDuplicateErrorIdentifiers_ReportsDiagnostics()
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
                Result.Fail(""My message"", 500, [|""My ID""|]);
                Result.Fail(new Exception(), ""My message"", 500, [|""My ID""|]);
                Result.Fail(""My message"", errorIdentifier: [|""My ID""|]);
                Result.Fail(new Exception(), errorIdentifier: [|""My ID""|]);
                Errors.BadRequest(""My message"", [|""My ID""|]);
                Errors.BadRequest(errorIdentifier: [|""My ID""|]);
                new Error { Identifier = [|""My ID""|] };
                new MyError { Identifier = [|""My ID""|] };
                GetError(""My message"", [|""My ID""|]);
                GetMyError(""My message"", [|""My ID""|]);
                GetError(errorIdentifier: [|""My ID""|]);
                GetMyError(errorIdentifier: [|""My ID""|]);

                Error error = new();
                error = error with { Identifier = [|""My ID""|] };

                MyError error2 = new();
                error2 = error2 with { Identifier = [|""My ID""|] };
            }

            private static Error GetError(string? message = null, string? errorIdentifier = null) => new Error { Message = message!, Identifier = errorIdentifier };

            private static MyError GetMyError(string? message = null, string? errorIdentifier = null) => new MyError { Message = message!, Identifier = errorIdentifier };

            private record class MyError : Error
            {
            }
        }
    }";

        await VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GivenNoDuplicateErrorIdentifiers_DoesNotReportDiagnostics()
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
                Result.Fail(""My message"", 500, ""My ID 1"");
                Result.Fail(new Exception(), ""My message"", 500, ""My ID 2"");
                Result.Fail(""My message"", errorIdentifier: ""My ID 3"");
                Result.Fail(new Exception(), errorIdentifier: ""My ID 4"");
                Errors.BadRequest(""My message"", ""My ID 5"");
                Errors.BadRequest(errorIdentifier: ""My ID 6"");
                new Error { Identifier = ""My ID 7"" };
                new MyError { Identifier = ""My ID 8"" };
                GetError(""My message"", ""My ID 9"");
                GetMyError(""My message"", ""My ID 10"");
                GetError(errorIdentifier: ""My ID 11"");
                GetMyError(errorIdentifier: ""My ID 12"");

                Error error = new();
                error = error with { Identifier = ""My ID 13"" };

                MyError error2 = new();
                error2 = error2 with { Identifier = ""My ID 14"" };
            }

            private static Error GetError(string? message = null, string? errorIdentifier = null) => new Error { Message = message!, Identifier = errorIdentifier };

            private static MyError GetMyError(string? message = null, string? errorIdentifier = null) => new MyError { Message = message!, Identifier = errorIdentifier };

            private record class MyError : Error
            {
            }
        }
    }";

        await VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GivenErrorIdentifiersNotProvided_DoesNotReportDiagnostics()
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
                Result.Fail(""My message"", 500);
                Result.Fail(new Exception(), ""My message"", 500);
                Errors.BadRequest(""My message"");
                new Error { Message = ""My message"" };
                new MyError { Message = ""My message"" };
                GetError(""My message"");
                GetMyError(""My message"");

                Error error = new();
                error = error with { Message = ""My message"" };

                MyError error2 = new();
                error2 = error2 with { Message = ""My message"" };
            }

            private static Error GetError(string? message = null, string? errorIdentifier = null) => new Error { Message = message!, Identifier = errorIdentifier };

            private static MyError GetMyError(string? message = null, string? errorIdentifier = null) => new MyError { Message = message!, Identifier = errorIdentifier };

            private record class MyError : Error
            {
            }
        }
    }";

        await VerifyAnalyzerAsync(code);
    }

    private static async Task VerifyAnalyzerAsync(string code)
    {
        var test = new CSharpAnalyzerTest<DuplicateErrorIdentifierAnalyzer, XUnitVerifier>
        {
            TestCode = code,
            ReferenceAssemblies = ReferenceAssemblies.Default
                .AddPackages(ImmutableArray.Create(
                    new PackageIdentity("RandomSkunk.Results", "1.3.1"))),
        };

        await test.RunAsync();
    }
}

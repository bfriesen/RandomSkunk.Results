using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RandomSkunk.Results;

namespace RandomSkunk.Results.Analyzers.Tests
{
    public class TryCatchGenerator_class
    {
        private static string GetGeneratedCode(string inputCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(inputCode);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => (MetadataReference)MetadataReference.CreateFromFile(a.Location));

            var compilation = CSharpCompilation.Create(
                nameof(TryCatchGenerator_class),
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new TryCatchGenerator();

            CSharpGeneratorDriver.Create(generator)
                .RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

            outputCompilation.SyntaxTrees.Should().HaveCount(compilation.SyntaxTrees.Length + 1);

            var generatedCode = outputCompilation.SyntaxTrees.Last().ToString();
            return generatedCode;
        }

        public class GivenNoSpecificCaughtException
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await SourceValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = SourceValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await SourceValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result Fred(out System.Boolean waldo, ref System.Boolean thud, in System.Boolean xyxxy)
        {
            try
            {
                SourceValue.Fred(out waldo, ref thud, in xyxxy);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch]
        public void Foo(int garply)
        {
        }

        [TryCatch]
        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch]
        public int Baz(int garply)
        {
            return 123;
        }

        [TryCatch]
        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        [TryCatch]
        public static void Garply()
        {
        }

        [TryCatch]
        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Fred))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExample
    {
        public static RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                Example.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    public static class Example
    {
        [TryCatch]
        public static void Foo(int garply)
        {
        }

        [TryCatch]
        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch]
        public static int Baz(int garply)
        {
            return 123;
        }

        [TryCatch]
        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample<T>
        where T : class, new()
    {
        internal TryExample(Example<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example<T> SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example<T>.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example{T}""/>.
    /// </summary>
    public static class Example_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample<T> Try<T>(this Example<T> sourceValue)
            where T : class, new()
        {
            return new TryExample<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        [TryCatch]
        public void Foo(int garply)
        {
        }

        [TryCatch]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Foo))]
[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Bar))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetMethodsAreGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                SourceValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch]
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        [TryCatch]
        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreInternal
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    internal struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    internal static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    internal class Example
    {
        [TryCatch]
        public void Foo(int garply)
        {
        }

        [TryCatch]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExampleExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                example.SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await example.SourceValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    [TryCatch]
    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        [TryCatch]
        public static void Foo(this Example example, int garply)
        {
        }

        [TryCatch]
        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Foo))]
[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Bar))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreGenericExtensionMethodsWithDifferentConstraints
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo<T>(this System.Collections.Generic.TryIEnumerable<T> foo)
            where T : new()
        {
            try
            {
                foo.SourceValue.Foo<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Bar<T>(this System.Collections.Generic.TryIEnumerable<T> bar)
            where T : struct
        {
            try
            {
                bar.SourceValue.Bar<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch]
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        [TryCatch]
        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Bar))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreClosedGenericExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this System.Collections.Generic.TryIEnumerable<System.Int32> foo)
        {
            try
            {
                foo.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch]
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsNested
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Extensions_TryBar bar)
        {
            try
            {
                bar.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Baz(Test.Extensions.Bar bar)
        {
            try
            {
                Extensions.Baz(bar);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct Extensions_TryBar
    {
        internal Extensions_TryBar(Extensions.Bar sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Extensions.Bar SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Extensions.Bar""/>.
    /// </summary>
    public static class Extensions_BarTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""Extensions_TryBar""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static Extensions_TryBar Try(this Extensions.Bar sourceValue)
        {
            return new Extensions_TryBar(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch]
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        [TryCatch]
        public static void Foo(this Bar bar)
        {
        }

        [TryCatch]
        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Foo))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Baz))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }
        }

        public class GivenOneSpecificCaughtException
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await SourceValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = SourceValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await SourceValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result Fred(out System.Boolean waldo, ref System.Boolean thud, in System.Boolean xyxxy)
        {
            try
            {
                SourceValue.Fred(out waldo, ref thud, in xyxxy);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch(typeof(InvalidOperationException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch(typeof(InvalidOperationException))]
        public int Baz(int garply)
        {
            return 123;
        }

        [TryCatch(typeof(InvalidOperationException))]
        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static void Garply()
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Fred), typeof(InvalidOperationException))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExample
    {
        public static RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                Example.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public static class Example
    {
        [TryCatch(typeof(InvalidOperationException))]
        public static void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static int Baz(int garply)
        {
            return 123;
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply), typeof(InvalidOperationException))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample<T>
        where T : class, new()
    {
        internal TryExample(Example<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example<T> SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example<T>.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example{T}""/>.
    /// </summary>
    public static class Example_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample<T> Try<T>(this Example<T> sourceValue)
            where T : class, new()
        {
            return new TryExample<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        [TryCatch(typeof(InvalidOperationException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), typeof(InvalidOperationException))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Bar), typeof(InvalidOperationException))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetMethodsAreGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                SourceValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch(typeof(InvalidOperationException))]
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreInternal
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    internal struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    internal static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    internal class Example
    {
        [TryCatch(typeof(InvalidOperationException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExampleExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                example.SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await example.SourceValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    [TryCatch(typeof(InvalidOperationException))]
    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        [TryCatch(typeof(InvalidOperationException))]
        public static void Foo(this Example example, int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), typeof(InvalidOperationException))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Bar), typeof(InvalidOperationException))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreGenericExtensionMethodsWithDifferentConstraints
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo<T>(this System.Collections.Generic.TryIEnumerable<T> foo)
            where T : new()
        {
            try
            {
                foo.SourceValue.Foo<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Bar<T>(this System.Collections.Generic.TryIEnumerable<T> bar)
            where T : struct
        {
            try
            {
                bar.SourceValue.Bar<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch(typeof(InvalidOperationException))]
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        [TryCatch(typeof(InvalidOperationException))]
        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), (typeof(InvalidOperationException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo), (typeof(InvalidOperationException)))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Bar), (typeof(InvalidOperationException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreClosedGenericExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this System.Collections.Generic.TryIEnumerable<System.Int32> foo)
        {
            try
            {
                foo.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch(typeof(InvalidOperationException))]
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), (typeof(InvalidOperationException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo), (typeof(InvalidOperationException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsNested
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Extensions_TryBar bar)
        {
            try
            {
                bar.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Baz(Test.Extensions.Bar bar)
        {
            try
            {
                Extensions.Baz(bar);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct Extensions_TryBar
    {
        internal Extensions_TryBar(Extensions.Bar sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Extensions.Bar SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Extensions.Bar""/>.
    /// </summary>
    public static class Extensions_BarTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""Extensions_TryBar""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static Extensions_TryBar Try(this Extensions.Bar sourceValue)
        {
            return new Extensions_TryBar(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException))]
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        [TryCatch(typeof(InvalidOperationException)]
        public static void Foo(this Bar bar)
        {
        }

        [TryCatch(typeof(InvalidOperationException)]
        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Foo), typeof(InvalidOperationException))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Baz), typeof(InvalidOperationException))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }
        }

        public class GivenTwoSpecificCaughtExceptions
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await SourceValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = SourceValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await SourceValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result Fred(out System.Boolean waldo, ref System.Boolean thud, in System.Boolean xyxxy)
        {
            try
            {
                SourceValue.Fred(out waldo, ref thud, in xyxxy);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public int Baz(int garply)
        {
            return 123;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Garply()
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Fred), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExample
    {
        public static RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                Example.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public static class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static int Baz(int garply)
        {
            return 123;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample<T>
        where T : class, new()
    {
        internal TryExample(Example<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example<T> SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example<T>.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example{T}""/>.
    /// </summary>
    public static class Example_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample<T> Try<T>(this Example<T> sourceValue)
            where T : class, new()
        {
            return new TryExample<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetMethodsAreGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                SourceValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreInternal
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    internal struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    internal static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    internal class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExampleExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                example.SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await example.SourceValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Foo(this Example example, int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreGenericExtensionMethodsWithDifferentConstraints
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo<T>(this System.Collections.Generic.TryIEnumerable<T> foo)
            where T : new()
        {
            try
            {
                foo.SourceValue.Foo<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Bar<T>(this System.Collections.Generic.TryIEnumerable<T> bar)
            where T : struct
        {
            try
            {
                bar.SourceValue.Bar<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException), typeof(DivideByZeroException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException)))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreClosedGenericExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this System.Collections.Generic.TryIEnumerable<System.Int32> foo)
        {
            try
            {
                foo.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException), typeof(DivideByZeroException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsNested
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Extensions_TryBar bar)
        {
            try
            {
                bar.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Baz(Test.Extensions.Bar bar)
        {
            try
            {
                Extensions.Baz(bar);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct Extensions_TryBar
    {
        internal Extensions_TryBar(Extensions.Bar sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Extensions.Bar SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Extensions.Bar""/>.
    /// </summary>
    public static class Extensions_BarTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""Extensions_TryBar""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static Extensions_TryBar Try(this Extensions.Bar sourceValue)
        {
            return new Extensions_TryBar(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Foo(this Bar bar)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException))]
        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Foo), typeof(InvalidOperationException), typeof(DivideByZeroException))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Baz), typeof(InvalidOperationException), typeof(DivideByZeroException))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }
        }

        public class GivenThreeSpecificCaughtExceptions
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await SourceValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = SourceValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await SourceValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public RandomSkunk.Results.Result Fred(out System.Boolean waldo, ref System.Boolean thud, in System.Boolean xyxxy)
        {
            try
            {
                SourceValue.Fred(out waldo, ref thud, in xyxxy);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public int Baz(int garply)
        {
            return 123;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Garply()
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public event EventHandler Grault
        {
            add { }
            remove { }
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }

        [Obsolete]
        public void Waldo()
        {
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Fred), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example
    {
        public void Foo(int garply)
        {
        }

        public Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public int Baz(int garply)
        {
            return 123;
        }

        public Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }

        public static void Garply()
        {
        }

        public void Fred(out bool waldo, ref bool thud, in bool xyxxy)
        {
            waldo = true;
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExample
    {
        public static RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                Example.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public static class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static int Baz(int garply)
        {
            return 123;
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Baz), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Qux), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Garply), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public static class Example
    {
        public static void Foo(int garply)
        {
        }

        public static Task Bar(int garply)
        {
            return Task.CompletedTask;
        }

        public static int Baz(int garply)
        {
            return 123;
        }

        public static Task<int> Qux(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample<T>
        where T : class, new()
    {
        internal TryExample(Example<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example<T> SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example<T>.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example{T}""/>.
    /// </summary>
    public static class Example_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample<T> Try<T>(this Example<T> sourceValue)
            where T : class, new()
        {
            return new TryExample<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example<>), nameof(Example<object>.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example<T>
        where T : class, new()
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetMethodsAreGeneric
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                SourceValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example
    {
        public void Foo<T>(int garply)
            where T : class, new()
        {
        }

        public static Task<int> Bar<T>(int garply)
            where T : class, new()
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreInternal
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    internal struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    internal static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    internal class Example
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public void Foo(int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Example), nameof(Example.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    internal class Example
    {
        public void Foo(int garply)
        {
        }

        public static Task<int> Bar(int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExampleExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                example.SourceValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var returnValueForSuccessResult = await example.SourceValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(returnValueForSuccessResult);
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Example SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Example""/>.
    /// </summary>
    public static class ExampleTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryExample""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryExample Try(this Example sourceValue)
        {
            return new TryExample(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Foo(this Example example, int garply)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System.Threading.Tasks;
using System;
using Test;

[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(ExampleExtensions), nameof(ExampleExtensions.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public class Example
    {
    }

    public static class ExampleExtensions
    {
        public static void Foo(this Example example, int garply)
        {
        }

        public static Task<int> Bar(this Example example, int garply)
        {
            return Task.FromResult(123);
        }
    }
}";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreGenericExtensionMethodsWithDifferentConstraints
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo<T>(this System.Collections.Generic.TryIEnumerable<T> foo)
            where T : new()
        {
            try
            {
                foo.SourceValue.Foo<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Bar<T>(this System.Collections.Generic.TryIEnumerable<T> bar)
            where T : struct
        {
            try
            {
                bar.SourceValue.Bar<T>();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException)))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Bar), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo<T>(this IEnumerable<T> foo)
            where T : new()
        {
        }

        public static void Bar<T>(this IEnumerable<T> bar)
            where T : struct
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetsAreClosedGenericExtensionMethods
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this System.Collections.Generic.TryIEnumerable<System.Int32> foo)
        {
            try
            {
                foo.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }
}

namespace System.Collections.Generic
{
    /// <inheritdoc cref=""IEnumerable{T}""/>
    public struct TryIEnumerable<T>
    {
        internal TryIEnumerable(IEnumerable<T> sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal IEnumerable<T> SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""IEnumerable{T}""/>.
    /// </summary>
    public static class IEnumerable_TTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""TryIEnumerable{T}""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static TryIEnumerable<T> Try<T>(this IEnumerable<T> sourceValue)
        {
            return new TryIEnumerable<T>(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Extensions.Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException)))]

namespace Test
{
    public static class Extensions
    {
        public static void Foo(this IEnumerable<int> foo)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }

            public class AndTargetTypeIsNested
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public static class TryExtensions
    {
        public static RandomSkunk.Results.Result Foo(this Extensions_TryBar bar)
        {
            try
            {
                bar.SourceValue.Foo();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }

        public static RandomSkunk.Results.Result Baz(Test.Extensions.Bar bar)
        {
            try
            {
                Extensions.Baz(bar);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.DivideByZeroException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
            catch (System.ArithmeticException caughtExceptionForFailResult)
            {
                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);
            }
        }
    }

    public struct Extensions_TryBar
    {
        internal Extensions_TryBar(Extensions.Bar sourceValue)
        {
            SourceValue = sourceValue;
        }

        internal Extensions.Bar SourceValue { get; }
    }

    /// <summary>
    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=""Extensions.Bar""/>.
    /// </summary>
    public static class Extensions_BarTryExtensionMethod
    {
        /// <summary>
        /// Gets a <em>try object</em> for the specified value.
        /// </summary>
        /// <param name=""sourceValue"">The source value of the <em>try object</em>.</param>
        /// <returns>A <see cref=""Extensions_TryBar""/> object.</returns>
        /// <remarks>
        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception
        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if
        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the
        /// details of the thrown exception is returned.
        /// </remarks>
        public static Extensions_TryBar Try(this Extensions.Bar sourceValue)
        {
            return new Extensions_TryBar(sourceValue);
        }
    }
}
";

                [Fact]
                public void WhenTargetTypeIsDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenTargetMethodsAreDecoratedWithTryCatchAttribute_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Foo(this Bar bar)
        {
        }

        [TryCatch(typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributeWithTargetType_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }

                [Fact]
                public void WhenAssemblyIsDecoratedWithTryCatchThirdPartyAttributesWithTargetMethod_GeneratedCodeIsCorrect()
                {
                    const string inputCode = @"using RandomSkunk.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Foo), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]
[assembly: TryCatchThirdParty(typeof(Extensions), nameof(Baz), typeof(InvalidOperationException), typeof(DivideByZeroException), typeof(ArithmeticException))]

namespace Test
{
    public static class Extensions
    {
        public class Bar
        {
        }

        public static void Foo(this Bar bar)
        {
        }

        public static void Baz(Bar bar)
        {
        }
    }
}
";

                    var generatedCode = GetGeneratedCode(inputCode);

                    generatedCode.Should().Be(_expectedGeneratedCode);
                }
            }
        }
    }
}

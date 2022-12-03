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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await BackingValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = BackingValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await BackingValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
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
        internal TryExample(Example<T> example)
        {
            BackingValue = example;
        }

        internal Example<T> BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample<T> Try<T>(this Example<T> example)
            where T : class, new()
        {
            return new TryExample<T>(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                BackingValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var methodReturnValue = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    internal static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
                example.BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await example.BackingValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.Exception ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        }

        public class GivenOneSpecificCaughtException
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await BackingValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = BackingValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await BackingValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
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
        internal TryExample(Example<T> example)
        {
            BackingValue = example;
        }

        internal Example<T> BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample<T> Try<T>(this Example<T> example)
            where T : class, new()
        {
            return new TryExample<T>(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                BackingValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var methodReturnValue = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    internal static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
                example.BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await example.BackingValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        }

        public class GivenTwoSpecificCaughtExceptions
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await BackingValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = BackingValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await BackingValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
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
        internal TryExample(Example<T> example)
        {
            BackingValue = example;
        }

        internal Example<T> BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample<T> Try<T>(this Example<T> example)
            where T : class, new()
        {
            return new TryExample<T>(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                BackingValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var methodReturnValue = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    internal static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
                example.BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await example.BackingValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        }

        public class GivenThreeSpecificCaughtExceptions
        {
            public class AndTargetTypeIsNonStaticClass
            {
                private const string _expectedGeneratedCode = @"namespace Test
{
    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await BackingValue.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = BackingValue.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await BackingValue.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result Garply()
        {
            try
            {
                Example.Garply();
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result> Bar(System.Int32 garply)
        {
            try
            {
                await Example.Bar(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static RandomSkunk.Results.Result<System.Int32> Baz(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = Example.Baz(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Qux(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Qux(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
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
        internal TryExample(Example<T> example)
        {
            BackingValue = example;
        }

        internal Example<T> BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample<T> Try<T>(this Example<T> example)
            where T : class, new()
        {
            return new TryExample<T>(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                BackingValue.Foo<T>(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar<T>(System.Int32 garply)
            where T : class, new()
        {
            try
            {
                var methodReturnValue = await Example.Bar<T>(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }

        public RandomSkunk.Results.Result Foo(System.Int32 garply)
        {
            try
            {
                BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await Example.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    internal static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
                example.BackingValue.Foo(garply);
                return RandomSkunk.Results.Result.Success();
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result.Fail(ex);
            }
        }

        public static async System.Threading.Tasks.Task<RandomSkunk.Results.Result<System.Int32>> Bar(this Test.TryExample example, System.Int32 garply)
        {
            try
            {
                var methodReturnValue = await example.BackingValue.Bar(garply);
                return RandomSkunk.Results.Result<System.Int32>.FromValue(methodReturnValue);
            }
            catch (System.InvalidOperationException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.DivideByZeroException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
            catch (System.ArithmeticException ex)
            {
                return RandomSkunk.Results.Result<System.Int32>.Fail(ex);
            }
        }
    }

    public struct TryExample
    {
        internal TryExample(Example example)
        {
            BackingValue = example;
        }

        internal Example BackingValue { get; }
    }

    public static class ExampleTryExtensionMethod
    {
        public static TryExample Try(this Example example)
        {
            return new TryExample(example);
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
        }
    }
}

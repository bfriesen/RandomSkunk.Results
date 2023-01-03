using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace RandomSkunk.Results.Analyzers;

/// <summary>
/// Defines a source generator that creates extension methods for <c>FailFactory&lt;TResult&gt;</c> based on types decorated with the
/// <c>[ErrorFactory]</c> attribute.
/// </summary>
[Generator]
public class FailFactoryExtensionsGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var errorFactoryClasses = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: IsDecoratedWithErrorFactoryAttribute,
            transform: GetDecoratedClass)
            .Where(item => item is not null)
            .Select((item, cancellationToken) => item!)
            .Collect();

        var sourceBuilder = errorFactoryClasses.Select(GetSourceBuilder);

        context.RegisterSourceOutput(sourceBuilder, (context, builder) =>
            context.AddSource("FailFactoryExtensions.generated.cs", builder.Build(context.CancellationToken)));
    }

    private static bool IsDecoratedWithErrorFactoryAttribute(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (!syntaxNode.IsKind(SyntaxKind.ClassDeclaration)
            || syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return false;
        }

        var errorFactoryAttribute = typeDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes)
            .Where(x => x.Name.ToString() is "ErrorFactory" or "ErrorFactoryAttribute")
            .FirstOrDefault();

        return errorFactoryAttribute is not null;
    }

    private static INamedTypeSymbol? GetDecoratedClass(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
            return null;

        if (context.SemanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol errorFactoryClass)
            return null;

        var attribute = errorFactoryClass.GetAttributes().FirstOrDefault(a =>
            a.AttributeClass?.Name == "ErrorFactoryAttribute"
            && a.AttributeClass?.ContainingNamespace?.ToString() == "RandomSkunk.Results");

        if (attribute is null)
            return null;

        if (!errorFactoryClass.IsStatic)
            return null;

        return errorFactoryClass;
    }

    private static SourceBuilder GetSourceBuilder(ImmutableArray<INamedTypeSymbol> errorFactoryClasses, CancellationToken cancellationToken)
    {
        var builder = new SourceBuilder();

        foreach (var errorFactoryClass in errorFactoryClasses)
            builder.Add(errorFactoryClass);

        return builder;
    }

    private static bool IsErrorOrInheritor(ITypeSymbol type)
    {
        if (IsExactType(type, "Error", "RandomSkunk.Results"))
            return true;
        if (type.BaseType is null)
            return false;
        return IsErrorOrInheritor(type.BaseType);
    }

    private static bool IsExactType(ITypeSymbol? typeSymbol, string typeName, string typeNamespace)
    {
        if (typeSymbol is null)
            return false;

        return typeSymbol.Name == typeName
            && typeSymbol.ContainingNamespace.ToString() == typeNamespace;
    }

    private static bool IsMarkedObsolete(ISymbol symbol)
    {
        return symbol.GetAttributes().Any(attribute =>
            attribute.AttributeClass?.Name == nameof(ObsoleteAttribute)
            && attribute.AttributeClass?.ContainingNamespace?.ToString() == typeof(ObsoleteAttribute).Namespace);
    }

    private static bool IsErrorFactoryMethod(ISymbol member, IEnumerable<ISymbol> allMembers)
    {
        // It can't be an error factory method if it isn't a public static method that returns Error or its inheritor.
        if (member.Kind != SymbolKind.Method
            || member.DeclaredAccessibility != Accessibility.Public
            || !member.IsStatic
            || member.IsImplicitlyDeclared
            || member.Name == ".ctor"
            || member is not IMethodSymbol methodSymbol
            || !IsErrorOrInheritor(methodSymbol.ReturnType))
        {
            return false;
        }

        // It can't be an error factory method if it's a generated property getter or setter method.
        if (member.Name.StartsWith("get_") || member.Name.StartsWith("set_"))
        {
            var properties = allMembers.Where(m => m.Kind == SymbolKind.Property);
            var propertyName = member.Name.Substring(4);
            if (properties.Any(p => p.Name == propertyName))
                return false;
        }

        // It can't be an error factory method if it's a generated event adder or remover method.
        if (member.Name.StartsWith("add_") || member.Name.StartsWith("remove_"))
        {
            var events = allMembers.Where(m => m.Kind == SymbolKind.Event);
            var eventName = member.Name.StartsWith("add_")
                ? member.Name.Substring(4)
                : member.Name.Substring(7);
            if (events.Any(p => p.Name == eventName))
                return false;
        }

        return true;
    }

    private class SourceBuilder
    {
        private readonly List<INamedTypeSymbol> _errorFactoryClasses = new();

        public void Add(INamedTypeSymbol errorFactoryClass)
        {
            _errorFactoryClasses.Add(errorFactoryClass);
        }

        public string Build(CancellationToken cancellationToken)
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();

            foreach (var errorFactoryClass in _errorFactoryClasses)
            {
                var classDeclaration = SyntaxFactory.ClassDeclaration($"FailFactoryExtensionsFor{errorFactoryClass.Name}")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword));

                ImmutableArray<ISymbol> allMembers;

                if (errorFactoryClass.Arity > 0 && errorFactoryClass.TypeParameters.Any(p => p.Kind == SymbolKind.TypeParameter))
                    allMembers = errorFactoryClass.OriginalDefinition.GetMembers();
                else
                    allMembers = errorFactoryClass.GetMembers();

                var methods = allMembers.Where(member => IsErrorFactoryMethod(member, allMembers) && !IsMarkedObsolete(member))
                    .Cast<IMethodSymbol>();

                foreach (var method in methods)
                {
                    var methodSyntax = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("TResult"), method.Name)
                        .WithTypeParameterList(
                            SyntaxFactory.TypeParameterList(
                                SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.TypeParameter("TResult"))));

                    var parameters = new List<ParameterSyntax>
                    {
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("failWith"))
                            .WithType(SyntaxFactory.ParseTypeName("FailFactory<TResult>"))
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                    };

                    foreach (var parameter in method.Parameters)
                    {
                        //parameter.
                    }

                    methodSyntax = methodSyntax.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)));

                    System.Diagnostics.Debugger.Break();

                    classDeclaration = classDeclaration.AddMembers(methodSyntax);

                    ////var methodSyntax = method.DeclaringSyntaxReferences
                    ////    .Select(reference => reference.GetSyntax(cancellationToken))
                    ////    .OfType<MethodDeclarationSyntax>()
                    ////    .FirstOrDefault();

                    ////if (methodSyntax is null)
                    ////    continue;

                    ////methodSyntax = methodSyntax.WithTypeParameterList(
                    ////    SyntaxFactory.TypeParameterList(
                    ////        SyntaxFactory.SingletonSeparatedList(
                    ////            SyntaxFactory.TypeParameter("TResult"))));

                    ////System.Diagnostics.Debugger.Break();

                    ////methodSyntax = methodSyntax.WithReturnType(SyntaxFactory.ParseTypeName("TResult"));

                    ////System.Diagnostics.Debugger.Break();

                    ////var failWithParameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier("failWith"))
                    ////    .WithType(SyntaxFactory.ParseTypeName("FailFactory<TResult>"))
                    ////    .AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword));

                    ////var parameters = Enumerable.Repeat(failWithParameter, 1).Concat(methodSyntax.ParameterList.Parameters);

                    ////methodSyntax = methodSyntax.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)));

                    ////System.Diagnostics.Debugger.Break();

                    ////classDeclaration = classDeclaration.AddMembers(methodSyntax);

                    // TODO: Add the fail factory extension methods here
                    /*
                    public static TResult BadRequest<TResult>(
                        this FailFactory<TResult> failWith,
                        string errorMessage = "get default value constant from existing method parameter",
                        string? errorIdentifier = null, // get default value constant from existing method parameter
                        bool isSensitive = false, // get default value constant from existing method parameter
                        IReadOnlyDictionary<string, object>? extensions = null, // get default value constant from existing method parameter
                        Error? innerError = null) // get default value constant from existing method parameter
                    {
                        return failWith.Error(RandomSkunk.Errors.BadRequest(errorMessage, errorIdentifier, isSensitive, extensions, innerError));
                    }
                     */
                }

                if (errorFactoryClass.ContainingNamespace is null or { Name: "" })
                {
                    compilationUnit = compilationUnit.AddMembers(classDeclaration);
                }
                else
                {
                    var namespaceSyntax = SyntaxFactory.NamespaceDeclaration(
                        SyntaxFactory.ParseName(errorFactoryClass.ContainingNamespace.ToString()))
                        .AddMembers(classDeclaration);

                    compilationUnit = compilationUnit.AddMembers(namespaceSyntax);
                }
            }

            var code = compilationUnit.NormalizeWhitespace().ToFullString();
            return code;
        }
    }
}

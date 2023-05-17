using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RandomSkunk.Results.Analyzers;

/// <summary>
/// Defines a source generator that creates a "Try" adapter for types decorated with a [TryCatch] attribute.
/// </summary>
[Generator]
public class TryCatchGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var decoratedMemberSymbolData = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: IsDecoratedWithTryCatchAttribute,
            transform: GetTargetDataFromDecoratedMember)
            .Where(item => item is (not null, not null))
            .Collect();

        var decoratedAssemblySymbolData = context.CompilationProvider.Select((compilation, cancellationToken) =>
            compilation.Assembly.GetAttributes()
                .Where(IsTryCatchThirdPartyAttribute)
                .Select(GetTargetDataFromAssemblyAttribute)
                .Where(item => item is (not null, not null))
                .ToImmutableArray());

        var sourceBuilder = decoratedMemberSymbolData.Combine(decoratedAssemblySymbolData)
            .Select((d, _) => d.Left.Concat(d.Right).ToImmutableArray())
            .Select(GetSourceBuilder);

        context.RegisterSourceOutput(sourceBuilder, (context, builder) =>
            context.AddSource("TryCatch.generated.cs", builder.Build(context.CancellationToken)));
    }

    private static bool IsDecoratedWithTryCatchAttribute(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        SyntaxList<AttributeListSyntax> attributeLists;

        if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
            attributeLists = typeDeclarationSyntax.AttributeLists;
        else if (syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax)
            attributeLists = methodDeclarationSyntax.AttributeLists;
        else
            return false;

        var tryCatchAttribute = attributeLists.SelectMany(x => x.Attributes)
            .Where(x => x.Name.ToString() is "TryCatch" or "TryCatchAttribute")
            .FirstOrDefault();

        return tryCatchAttribute is not null;
    }

    private static TargetData GetTargetDataFromDecoratedMember(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node, cancellationToken);

        if (symbol is null)
            return default;

        var attribute = symbol.GetAttributes().FirstOrDefault(a =>
            a.AttributeClass?.Name == "TryCatchAttribute"
            && a.AttributeClass?.ContainingNamespace?.ToString() == "RandomSkunk.Results");

        if (attribute is null)
            return default;

        var tryCatchInfo = new TryCatchInfo();

        foreach (var namedArgument in attribute.NamedArguments)
        {
            if (namedArgument.Key == "AsMaybe")
                tryCatchInfo.AsMaybe = (bool)namedArgument.Value.Value!;
        }

        if (attribute.ConstructorArguments.Length == 0)
        {
            tryCatchInfo.TExceptions = Array.Empty<INamedTypeSymbol>();
            return new(symbol, tryCatchInfo);
        }

        if (attribute.ConstructorArguments.Any(a => a.IsNull || a.Value is not INamedTypeSymbol type))
            return default;

        var namedTypes = attribute.ConstructorArguments.Select(a => (INamedTypeSymbol)a.Value!).ToArray();

        if (!IsException(namedTypes[0]))
            return default;

        for (int i = 1; i < namedTypes.Length; i++)
        {
            if (!IsException(namedTypes[i]))
                return default;

            if (IsLeftTheSameOrABaseTypeOfRight(namedTypes[i - 1], namedTypes[i]))
                return default;
        }

        tryCatchInfo.TExceptions = namedTypes;
        return new(symbol, tryCatchInfo);
    }

    private static bool IsTryCatchThirdPartyAttribute(AttributeData a)
    {
        return a.AttributeClass?.Name == "TryCatchThirdPartyAttribute"
            && a.AttributeClass?.ContainingNamespace?.ToString() == "RandomSkunk.Results";
    }

    private static TargetData GetTargetDataFromAssemblyAttribute(AttributeData attribute)
    {
        var targetTypeArgument = attribute.ConstructorArguments[0];
        if (targetTypeArgument.IsNull || targetTypeArgument.Value is not INamedTypeSymbol targetType)
            return default;

        string? methodName = null;
        if (attribute.ConstructorArguments.Length > 1 && attribute.ConstructorArguments[1].Type!.Name == "String")
        {
            var methodNameArgument = attribute.ConstructorArguments[1];
            methodName = (string?)methodNameArgument.Value;
        }

        var target = new AttributeTarget(targetType, methodName);

        var tryCatchInfo = new TryCatchInfo();

        foreach (var namedArgument in attribute.NamedArguments)
        {
            if (namedArgument.Key == "AsMaybe")
                tryCatchInfo.AsMaybe = (bool)namedArgument.Value.Value!;
        }

        if (attribute.ConstructorArguments.Length == 1
            || (attribute.ConstructorArguments.Length == 2 && attribute.ConstructorArguments[1].Type!.Name == "String"))
        {
            tryCatchInfo.TExceptions = Array.Empty<INamedTypeSymbol>();
            return new(target, tryCatchInfo);
        }

        var nonExceptionArgs = attribute.ConstructorArguments[1].Type!.Name == "String" ? 2 : 1;

        if (attribute.ConstructorArguments.Skip(nonExceptionArgs).Any(a => a.IsNull || a.Value is not INamedTypeSymbol type))
            return default;

        var namedTypes = attribute.ConstructorArguments.Skip(nonExceptionArgs).Select(a => (INamedTypeSymbol)a.Value!).ToArray();

        if (!IsException(namedTypes[0]))
            return default;

        for (int i = 1; i < namedTypes.Length; i++)
        {
            if (!IsException(namedTypes[i]))
                return default;

            if (IsLeftTheSameOrABaseTypeOfRight(namedTypes[i - 1], namedTypes[i]))
                return default;
        }

        tryCatchInfo.TExceptions = namedTypes;

        return new(target, tryCatchInfo);
    }

    private static SourceBuilder GetSourceBuilder(ImmutableArray<TargetData> items, CancellationToken cancellationToken)
    {
        var builder = new SourceBuilder();

        foreach (var item in items)
        {
            if (item.Target is AttributeTarget target)
            {
                ImmutableArray<ISymbol> allMembers;

                if (target.Type.Arity > 0 && target.Type.TypeParameters.Any(p => p.Kind == SymbolKind.TypeParameter))
                    allMembers = target.Type.OriginalDefinition.GetMembers();
                else
                    allMembers = target.Type.GetMembers();

                var methods = allMembers.Where(symbol => IsPublicMethod(symbol, allMembers) && !IsMarkedObsolete(symbol))
                    .Cast<IMethodSymbol>();

                if (target.MethodName is null)
                {
                    builder.AddTryCatchDefinition(target.Type, methods, item.TryCatchInfo);
                }
                else
                {
                    foreach (var method in methods.Where(m => m.Name == target.MethodName))
                        builder.AddTryCatchDefinition(method, item.TryCatchInfo);
                }
            }
            else if (item.Target is ISymbol symbol)
            {
                if (symbol.Kind == SymbolKind.NamedType)
                {
                    var namedType = (INamedTypeSymbol)symbol;

                    var allMembers = namedType.GetMembers();
                    var methods = allMembers.Where(symbol => IsPublicMethod(symbol, allMembers) && !IsMarkedObsolete(symbol))
                        .Cast<IMethodSymbol>();

                    builder.AddTryCatchDefinition(namedType, methods, item.TryCatchInfo);
                }
                else if (symbol.Kind == SymbolKind.Method)
                {
                    var method = (IMethodSymbol)symbol;
                    builder.AddTryCatchDefinition(method, item.TryCatchInfo);
                }
            }
        }

        return builder;
    }

    private static bool IsException(INamedTypeSymbol type)
    {
        if (IsExactType(type, typeof(Exception)))
            return true;
        if (type.BaseType is null)
            return false;
        return IsException(type.BaseType);
    }

    private static bool IsExactType(ITypeSymbol? typeSymbol, Type type)
    {
        if (typeSymbol is null)
            return false;

        return typeSymbol.Name == type.Name
            && typeSymbol.ContainingNamespace.ToString() == type.Namespace;
    }

    private static bool IsLeftTheSameOrABaseTypeOfRight(INamedTypeSymbol left, INamedTypeSymbol right)
    {
        if (left.Equals(right, SymbolEqualityComparer.Default))
            return true;
        if (right.BaseType is null)
            return false;
        return IsLeftTheSameOrABaseTypeOfRight(left, right.BaseType);
    }

    private static bool IsMarkedObsolete(ISymbol symbol)
    {
        return symbol.GetAttributes().Any(attribute =>
            attribute.AttributeClass?.Name == nameof(ObsoleteAttribute)
            && attribute.AttributeClass?.ContainingNamespace?.ToString() == typeof(ObsoleteAttribute).Namespace);
    }

    private static bool IsPublicMethod(ISymbol symbol, IEnumerable<ISymbol> allMembers)
    {
        if (symbol.Kind != SymbolKind.Method
            || symbol.DeclaredAccessibility != Accessibility.Public
            || symbol.IsImplicitlyDeclared
            || symbol.Name == ".ctor")
        {
            return false;
        }

        if (symbol.Name.StartsWith("get_") || symbol.Name.StartsWith("set_"))
        {
            var properties = allMembers.Where(m => m.Kind == SymbolKind.Property);
            var propertyName = symbol.Name.Substring(4);
            if (properties.Any(p => p.Name == propertyName))
                return false;
        }

        if (symbol.Name.StartsWith("add_") || symbol.Name.StartsWith("remove_"))
        {
            var events = allMembers.Where(m => m.Kind == SymbolKind.Event);
            var eventName = symbol.Name.StartsWith("add_")
                ? symbol.Name.Substring(4)
                : symbol.Name.Substring(7);
            if (events.Any(p => p.Name == eventName))
                return false;
        }

        return true;
    }

    private record struct TargetData(object Target, TryCatchInfo TryCatchInfo);

    private record struct MethodData(IMethodSymbol Method, TryCatchInfo TryCatchInfo);

    private class TryCatchInfo
    {
        public bool AsMaybe { get; set; }

        public INamedTypeSymbol[] TExceptions { get; set; } = null!;
    }

    private class SourceBuilder
    {
        private readonly Dictionary<
            (string? TargetTypeNamespace, string TargetTypeName, string? TargetTypeTypeParameters),
            (INamedTypeSymbol TargetType, List<MethodData> Methods)> _tryCatchDefinitions = new();

        public void AddTryCatchDefinition(INamedTypeSymbol targetType, IEnumerable<IMethodSymbol> methods, TryCatchInfo tryCatchInfo)
        {
            var key = (targetType.ContainingNamespace?.ToString(), GetName(targetType), GetGenericParameters(targetType));
            if (_tryCatchDefinitions.ContainsKey(key))
            {
                var (_, existingMethodData) = _tryCatchDefinitions[key];

                foreach (var method in methods)
                {
                    if (!existingMethodData.Any(existingData => method.Equals(existingData.Method, SymbolEqualityComparer.Default)))
                        existingMethodData.Add(new MethodData(method, tryCatchInfo));

                    if (method.IsExtensionMethod)
                        EnsureTryCatchDefinitionExists(method.Parameters[0].Type);
                }
            }
            else
            {
                _tryCatchDefinitions[key] = (targetType, methods.Select(method => new MethodData(method, tryCatchInfo)).ToList());
                foreach (var method in methods.Where(m => m.IsExtensionMethod))
                    EnsureTryCatchDefinitionExists(method.Parameters[0].Type);
            }
        }

        public void AddTryCatchDefinition(IMethodSymbol method, TryCatchInfo tryCatchInfo)
        {
            var targetType = method.ContainingType;
            var key = (targetType.ContainingNamespace?.ToString(), GetName(targetType), GetGenericParameters(targetType));
            if (_tryCatchDefinitions.ContainsKey(key))
            {
                var (_, existingMethodData) = _tryCatchDefinitions[key];

                var methodData = new MethodData(method, tryCatchInfo);
                var index = existingMethodData.FindIndex(existingData => method.Equals(existingData.Method, SymbolEqualityComparer.Default));

                if (index == -1)
                    existingMethodData.Add(methodData);
                else
                    existingMethodData[index] = methodData;
            }
            else
            {
                _tryCatchDefinitions[key] = (targetType, new List<MethodData> { new MethodData(method, tryCatchInfo) });
            }

            if (method.IsExtensionMethod)
                EnsureTryCatchDefinitionExists(method.Parameters[0].Type);
        }

        public string Build(CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();

            foreach (var tryCatchDefinitionsByNamespace in _tryCatchDefinitions.GroupBy(item => item.Key.TargetTypeNamespace))
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                var targetNamespace = tryCatchDefinitionsByNamespace.Key;
                sb.AppendLine($"namespace {targetNamespace}");
                sb.Append("{");

                foreach (var tryCatchDefinition in tryCatchDefinitionsByNamespace)
                {
                    var targetTypeName = tryCatchDefinition.Key.TargetTypeName;
                    var tryTypeName = GetTryName(targetTypeName);

                    var typeParameters = tryCatchDefinition.Key.TargetTypeTypeParameters;
                    var targetType = targetTypeName + typeParameters;
                    var tryType = tryTypeName + typeParameters;

                    var targetTypeDocComments = targetType.Replace('<', '{').Replace('>', '}');
                    var tryTypeDocComments = tryType.Replace('<', '{').Replace('>', '}');

                    var targetTypeAccessibility = GetAccessibility(tryCatchDefinition.Value.TargetType.DeclaredAccessibility);

                    sb.AppendLine();
                    AppendDocComments(sb, tryCatchDefinition.Value.TargetType, cancellationToken, isMethod: false, targetTypeDocComments);

                    if (tryCatchDefinition.Value.TargetType.IsStatic)
                    {
                        sb.AppendLine($"    {targetTypeAccessibility} static class {tryType}");
                        AppendGenericConstraints(sb, tryCatchDefinition.Value.TargetType, tryCatchDefinition.Value.TargetType.Arity, tryCatchDefinition.Value.TargetType.TypeParameters);
                        sb.Append("    {");
                    }
                    else
                    {
                        sb.AppendLine($"    {targetTypeAccessibility} struct {tryType}");
                        AppendGenericConstraints(sb, tryCatchDefinition.Value.TargetType, tryCatchDefinition.Value.TargetType.Arity, tryCatchDefinition.Value.TargetType.TypeParameters);
                        sb.AppendLine("    {");
                        sb.AppendLine($"        internal {tryTypeName}({targetType} sourceValue)");
                        sb.AppendLine("        {");
                        sb.AppendLine($"            SourceValue = sourceValue;");
                        sb.AppendLine("        }");
                        sb.AppendLine();
                        sb.AppendLine($"        internal {targetType} SourceValue {{ get; }}");
                    }

                    foreach (var methodData in tryCatchDefinition.Value.Methods)
                    {
                        var @static = methodData.Method.IsStatic ? "static " : null;

                        var methodName = methodData.Method.Name;
                        var returnType = methodData.Method.ReturnType;

                        sb.AppendLine();
                        AppendDocComments(sb, methodData.Method, cancellationToken, isMethod: true);

                        if (IsExactType(returnType, typeof(void)))
                        {
                            sb.Append($"        public {@static}RandomSkunk.Results.Result {methodName}");
                            AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                            AppendGenericConstraints(sb, methodData.Method, methodData.Method.Arity, methodData.Method.TypeParameters);
                            sb.AppendLine("        {");
                            sb.AppendLine("            try");
                            sb.AppendLine("            {");
                            if (methodData.Method.IsStatic)
                            {
                                if (methodData.Method.IsExtensionMethod)
                                {
                                    if (isTryAdapterExtensionMethod)
                                        sb.Append($"                {methodData.Method.Parameters[0].Name}.SourceValue.{methodName}");
                                    else
                                        sb.Append($"                {methodData.Method.Parameters[0].Name}.{methodName}");
                                }
                                else
                                {
                                    sb.Append($"                {targetType}.{methodName}");
                                }
                            }
                            else
                            {
                                sb.Append($"                SourceValue.{methodName}");
                            }

                            AppendArguments(sb, methodData.Method);
                            sb.AppendLine("                return RandomSkunk.Results.Result.Success();");
                            sb.AppendLine("            }");
                            sb.AppendLine("            catch (System.Threading.Tasks.TaskCanceledException caughtExceptionForFailResult)");
                            sb.AppendLine("            {");
                            sb.AppendLine("                return RandomSkunk.Results.Errors.Canceled(caughtExceptionForFailResult);");
                            sb.AppendLine("            }");

                            if (methodData.TryCatchInfo.TExceptions.Length == 0)
                            {
                                sb.AppendLine("            catch (System.Exception caughtExceptionForFailResult)");
                                sb.AppendLine("            {");
                                sb.AppendLine("                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);");
                                sb.AppendLine("            }");
                            }
                            else
                            {
                                foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                {
                                    sb.AppendLine($"            catch ({GetFullName(exception)} caughtExceptionForFailResult)");
                                    sb.AppendLine("            {");
                                    sb.AppendLine("                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);");
                                    sb.AppendLine("            }");
                                }
                            }

                            sb.AppendLine("        }");
                        }
                        else
                        {
                            var resultType = methodData.TryCatchInfo.AsMaybe ? "RandomSkunk.Results.Maybe" : "RandomSkunk.Results.Result";

                            if ((returnType.Name == "Task" && returnType.ContainingNamespace?.ToString() == "System.Threading.Tasks")
                                || (returnType.Name == "ValueTask" && returnType.ContainingNamespace?.ToString() == "System.Threading.Tasks"))
                            {
                                var namedType = (INamedTypeSymbol)returnType;

                                if (namedType.Arity == 0)
                                {
                                    sb.Append($"        public {@static}async System.Threading.Tasks.Task<RandomSkunk.Results.Result> {methodName}");
                                    AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                                    AppendGenericConstraints(sb, methodData.Method, methodData.Method.Arity, methodData.Method.TypeParameters);
                                    sb.AppendLine("        {");
                                    sb.AppendLine("            try");
                                    sb.AppendLine("            {");
                                    if (methodData.Method.IsStatic)
                                    {
                                        if (methodData.Method.IsExtensionMethod)
                                        {
                                            if (isTryAdapterExtensionMethod)
                                                sb.Append($"                await {methodData.Method.Parameters[0].Name}.SourceValue.{methodName}");
                                            else
                                                sb.Append($"                await {methodData.Method.Parameters[0].Name}.{methodName}");
                                        }
                                        else
                                        {
                                            sb.Append($"                await {targetType}.{methodName}");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append($"                await SourceValue.{methodName}");
                                    }

                                    AppendArguments(sb, methodData.Method);
                                    sb.AppendLine("                return RandomSkunk.Results.Result.Success();");
                                    sb.AppendLine("            }");
                                    sb.AppendLine("            catch (System.Threading.Tasks.TaskCanceledException caughtExceptionForFailResult)");
                                    sb.AppendLine("            {");
                                    sb.AppendLine("                return RandomSkunk.Results.Errors.Canceled(caughtExceptionForFailResult);");
                                    sb.AppendLine("            }");

                                    if (methodData.TryCatchInfo.TExceptions.Length == 0)
                                    {
                                        sb.AppendLine("            catch (System.Exception caughtExceptionForFailResult)");
                                        sb.AppendLine("            {");
                                        sb.AppendLine("                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);");
                                        sb.AppendLine("            }");
                                    }
                                    else
                                    {
                                        foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                        {
                                            sb.AppendLine($"            catch ({GetFullName(exception)} caughtExceptionForFailResult)");
                                            sb.AppendLine("            {");
                                            sb.AppendLine("                return RandomSkunk.Results.Result.Fail(caughtExceptionForFailResult);");
                                            sb.AppendLine("            }");
                                        }
                                    }

                                    sb.AppendLine("        }");
                                }
                                else
                                {
                                    returnType = namedType.TypeArguments[0];

                                    sb.Append($"        public {@static}async System.Threading.Tasks.Task<{resultType}<{GetFullName(returnType)}>> {methodName}");
                                    AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                                    AppendGenericConstraints(sb, methodData.Method, methodData.Method.Arity, methodData.Method.TypeParameters);
                                    sb.AppendLine("        {");
                                    sb.AppendLine("            try");
                                    sb.AppendLine("            {");
                                    if (methodData.Method.IsStatic)
                                    {
                                        if (methodData.Method.IsExtensionMethod)
                                        {
                                            if (isTryAdapterExtensionMethod)
                                                sb.Append($"                return await {methodData.Method.Parameters[0].Name}.SourceValue.{methodName}");
                                            else
                                                sb.Append($"                return await {methodData.Method.Parameters[0].Name}.{methodName}");
                                        }
                                        else
                                        {
                                            sb.Append($"                return await {targetType}.{methodName}");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append($"                return await SourceValue.{methodName}");
                                    }

                                    AppendArguments(sb, methodData.Method);

                                    sb.AppendLine("            }");
                                    sb.AppendLine("            catch (System.Threading.Tasks.TaskCanceledException caughtExceptionForFailResult)");
                                    sb.AppendLine("            {");
                                    sb.AppendLine($"                return RandomSkunk.Results.Errors.Canceled(caughtExceptionForFailResult);");
                                    sb.AppendLine("            }");

                                    if (methodData.TryCatchInfo.TExceptions.Length == 0)
                                    {
                                        sb.AppendLine("            catch (System.Exception caughtExceptionForFailResult)");
                                        sb.AppendLine("            {");
                                        sb.AppendLine($"                return {resultType}<{GetFullName(returnType)}>.Fail(caughtExceptionForFailResult);");
                                        sb.AppendLine("            }");
                                    }
                                    else
                                    {
                                        foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                        {
                                            sb.AppendLine($"            catch ({GetFullName(exception)} caughtExceptionForFailResult)");
                                            sb.AppendLine("            {");
                                            sb.AppendLine($"                return {resultType}<{GetFullName(returnType)}>.Fail(caughtExceptionForFailResult);");
                                            sb.AppendLine("            }");
                                        }
                                    }

                                    sb.AppendLine("        }");
                                }
                            }
                            else
                            {
                                sb.Append($"        public {@static}{resultType}<{GetFullName(returnType)}> {methodName}");
                                AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                                AppendGenericConstraints(sb, methodData.Method, methodData.Method.Arity, methodData.Method.TypeParameters);
                                sb.AppendLine("        {");
                                sb.AppendLine("            try");
                                sb.AppendLine("            {");
                                if (methodData.Method.IsStatic)
                                {
                                    if (methodData.Method.IsExtensionMethod)
                                    {
                                        if (isTryAdapterExtensionMethod)
                                            sb.Append($"                return {methodData.Method.Parameters[0].Name}.SourceValue.{methodName}");
                                        else
                                            sb.Append($"                return {methodData.Method.Parameters[0].Name}.{methodName}");
                                    }
                                    else
                                    {
                                        sb.Append($"                return {targetType}.{methodName}");
                                    }
                                }
                                else
                                {
                                    sb.Append($"                return SourceValue.{methodName}");
                                }

                                AppendArguments(sb, methodData.Method);

                                sb.AppendLine("            }");
                                sb.AppendLine("            catch (System.Threading.Tasks.TaskCanceledException caughtExceptionForFailResult)");
                                sb.AppendLine("            {");
                                sb.AppendLine($"                return RandomSkunk.Results.Errors.Canceled(caughtExceptionForFailResult);");
                                sb.AppendLine("            }");

                                if (methodData.TryCatchInfo.TExceptions.Length == 0)
                                {
                                    sb.AppendLine("            catch (System.Exception caughtExceptionForFailResult)");
                                    sb.AppendLine("            {");
                                    sb.AppendLine($"                return {resultType}<{GetFullName(returnType)}>.Fail(caughtExceptionForFailResult);");
                                    sb.AppendLine("            }");
                                }
                                else
                                {
                                    foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                    {
                                        sb.AppendLine($"            catch ({GetFullName(exception)} caughtExceptionForFailResult)");
                                        sb.AppendLine("            {");
                                        sb.AppendLine($"                return {resultType}<{GetFullName(returnType)}>.Fail(caughtExceptionForFailResult);");
                                        sb.AppendLine("            }");
                                    }
                                }

                                sb.AppendLine("        }");
                            }
                        }
                    }

                    sb.AppendLine("    }");

                    if (!tryCatchDefinition.Value.TargetType.IsStatic)
                    {
                        var tryExtensionMethodGenericParameters =
                            GetGenericParameters(tryCatchDefinition.Value.TargetType);

                        sb.AppendLine();
                        sb.AppendLine("    /// <summary>");
                        sb.AppendLine($"    /// Defines an extension method for getting <c>Try Objects</c> for type <see cref=\"{targetTypeDocComments}\"/>.");
                        sb.AppendLine("    /// </summary>");
                        sb.AppendLine($"    {targetTypeAccessibility} static class {targetType.Replace('.', '_').Replace('<', '_').Replace(", ", "_").Replace(">", null)}TryExtensionMethod");
                        sb.AppendLine("    {");
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine("        /// Gets a <em>try object</em> for the specified value.");
                        sb.AppendLine("        /// </summary>");
                        sb.AppendLine("        /// <param name=\"sourceValue\">The source value of the <em>try object</em>.</param>");
                        sb.AppendLine($"        /// <returns>A <see cref=\"{tryTypeDocComments}\"/> object.</returns>");
                        sb.AppendLine("        /// <remarks>");
                        sb.AppendLine("        /// A <em>try object</em> behaves almost identically to the object it targets, except its methods won't throw an exception");
                        sb.AppendLine("        /// and instead return a <c>Result</c>. Each <em>try object</em> method calls its target method inside a try/catch block: if");
                        sb.AppendLine("        /// no exception is thrown, a <c>Success</c> result is returned; otherwise, a <c>Fail</c> result with an error capturing the");
                        sb.AppendLine("        /// details of the thrown exception is returned.");
                        sb.AppendLine("        /// </remarks>");
                        sb.AppendLine($"        public static {tryType} Try{tryExtensionMethodGenericParameters}(this {targetType} sourceValue)");
                        AppendGenericConstraints(sb, null, tryCatchDefinition.Value.TargetType.Arity, tryCatchDefinition.Value.TargetType.TypeParameters);
                        sb.AppendLine("        {");
                        sb.AppendLine($"            return new {tryType}(sourceValue);");
                        sb.AppendLine("        }");
                        sb.AppendLine("    }");
                    }
                }

                sb.AppendLine("}");
            }

            var source = sb.ToString();
            return source;
        }

        private static string GetTryName(string targetTypeName)
        {
            var openAngleBracketIndex = targetTypeName.IndexOf('<');
            if (openAngleBracketIndex == -1)
                openAngleBracketIndex = targetTypeName.Length - 1;

            var lastDotIndex = targetTypeName.LastIndexOf('.', openAngleBracketIndex);
            return (targetTypeName.Substring(0, lastDotIndex + 1) + "Try" + targetTypeName.Substring(lastDotIndex + 1))
                .Replace('.', '_');
        }

        private static string GetAccessibility(Accessibility declaredAccessibility)
        {
            return declaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                _ => string.Empty,
            };
        }

        private static StringBuilder AppendDocComments(StringBuilder sb, ISymbol targetSymbol, CancellationToken cancellationToken, bool isMethod, string? targetTypeDocComments = null)
        {
            var indent = isMethod ? "        " : "    ";

            if (targetSymbol.Locations.Any(location => location.IsInSource))
            {
                foreach (var syntaxReference in targetSymbol.DeclaringSyntaxReferences)
                {
                    var syntaxNode = syntaxReference.GetSyntax(cancellationToken);
                    var trivia = syntaxNode.GetLeadingTrivia();
                    var index = trivia.IndexOf(SyntaxKind.SingleLineDocumentationCommentTrivia);
                    if (index != -1)
                    {
                        var t = trivia[index];

                        var docComments = Regex.Replace(t.ToString().Trim(), @"\n\s*", '\n' + indent);

                        sb.Append(indent).Append("/// ");
                        sb.Append(docComments);
                        return sb.AppendLine();
                    }
                }
            }
            else if (targetSymbol.Locations.Any(location => location.IsInMetadata))
            {
                sb.AppendLine($"{indent}/// <inheritdoc cref=\"{targetTypeDocComments ?? targetSymbol.GetDocumentationCommentId()}\"/>");
            }

            return sb;
        }

        private static StringBuilder AppendArguments(StringBuilder sb, IMethodSymbol method)
        {
            if (method.Arity > 0)
            {
                var typeArguments = string.Join(", ", method.TypeParameters.Select(GetFullName));
                sb.Append('<').Append(typeArguments).Append('>');
            }

            sb.Append('(');

            var first = true;
            foreach (var parameter in method.Parameters.Skip(method.IsExtensionMethod ? 1 : 0))
            {
                if (first) first = false;
                else sb.Append(", ");

                AppendRefKind(sb, parameter.RefKind);
                sb.Append(parameter.Name);
            }

            sb.Append(");");
            return sb.AppendLine();
        }

        private static bool HasGenericConstraints(ITypeParameterSymbol typeParameter)
        {
            return typeParameter.ConstraintTypes.Length > 0
                || typeParameter.HasConstructorConstraint
                || typeParameter.HasNotNullConstraint
                || typeParameter.HasReferenceTypeConstraint
                || typeParameter.HasUnmanagedTypeConstraint
                || typeParameter.HasValueTypeConstraint;
        }

        private static StringBuilder AppendGenericConstraints(StringBuilder sb, ISymbol? targetSymbol, int arity, IReadOnlyList<ITypeParameterSymbol> typeParameters)
        {
            if (arity == 0)
                return sb;

            string spaces;

            if (targetSymbol?.Kind == SymbolKind.NamedType)
            {
                var targetType = (INamedTypeSymbol)targetSymbol;
                if (!targetType.TypeParameters.Any(HasGenericConstraints))
                    return sb;
                spaces = "        ";
            }
            else
            {
                spaces = "            ";
            }

            foreach (var typeParameter in typeParameters)
            {
                if (HasGenericConstraints(typeParameter))
                {
                    sb.Append($"{spaces}where {typeParameter.Name} : ");

                    string? comma = null;

                    if (typeParameter.HasReferenceTypeConstraint)
                        sb.Append(GetComma()).Append("class");

                    if (typeParameter.HasValueTypeConstraint)
                        sb.Append(GetComma()).Append("struct");

                    if (typeParameter.HasNotNullConstraint)
                        sb.Append(GetComma()).Append("notnull");

                    if (typeParameter.HasUnmanagedTypeConstraint)
                        sb.Append(GetComma()).Append("unmanaged");

                    foreach (var constraintType in typeParameter.ConstraintTypes)
                        sb.Append(GetComma()).Append(GetFullName(constraintType));

                    if (typeParameter.HasConstructorConstraint)
                        sb.Append(GetComma()).Append("new()");

                    sb.AppendLine();

                    string? GetComma()
                    {
                        if (comma is not null)
                            return comma;

                        comma = ", ";
                        return null;
                    }
                }
            }

            return sb;
        }

        private static bool IsEnum(ITypeSymbol type)
        {
            if (IsExactType(type, typeof(Enum)))
                return true;

            if (type.BaseType is null)
                return false;

            return IsEnum(type.BaseType);
        }

        private static string? GetGenericParameters(ITypeSymbol type)
        {
            if (type is not INamedTypeSymbol namedType)
                return null;

            if (namedType.Arity == 0)
                return null;

            var typeParameters = string.Join(", ", namedType.TypeParameters.Select(GetFullName));
            return $"<{typeParameters}>";
        }

        private static string GetFullName(ITypeSymbol type) => GetFullName(type, null);

        private static string GetFullName(ITypeSymbol type, string? typeNamePrefix, bool skipNamespace = false)
        {
            if (type.Kind == SymbolKind.TypeParameter)
            {
                return type.Name;
            }

            if (type is IArrayTypeSymbol arrayType)
            {
                var commas = arrayType.Rank == 1 ? null : string.Join(string.Empty, Enumerable.Repeat(',', arrayType.Rank - 1));
                var elementType = GetFullName(arrayType.ElementType, typeNamePrefix);
                return $"{elementType}[{commas}]";
            }

            var sb = new StringBuilder();
            sb.Append(typeNamePrefix);
            sb.Append(type.Name);

            var t = type;
            while (t.ContainingType is not null)
            {
                sb.Insert(0, '.');
                sb.Insert(0, t.ContainingType.Name);

                t = t.ContainingType;
            }

            if (!skipNamespace && type.ContainingNamespace is not null)
            {
                sb.Insert(0, '.');
                sb.Insert(0, type.ContainingNamespace);
            }

            if (type is INamedTypeSymbol { Arity: > 0 } namedType)
            {
                var typeArguments = string.Join(", ", namedType.TypeArguments.Select(ta => GetFullName(ta)));
                return $"{sb}<{typeArguments}>";
            }

            return sb.ToString();
        }

        private static string GetName(ITypeSymbol type, string? typeNamePrefix = null)
        {
            if (type.Kind == SymbolKind.TypeParameter)
            {
                return type.Name;
            }

            var sb = new StringBuilder();
            sb.Append(typeNamePrefix);
            sb.Append(type.Name);

            var t = type;
            while (t.ContainingType is not null)
            {
                sb.Insert(0, '.');
                sb.Insert(0, t.ContainingType.Name);

                t = t.ContainingType;
            }

            return sb.ToString();
        }

        private static StringBuilder AppendRefKind(StringBuilder sb, RefKind refKind)
        {
            return refKind switch
            {
                RefKind.In => sb.Append("in "),
                RefKind.Out => sb.Append("out "),
                RefKind.Ref => sb.Append("ref "),
                _ => sb,
            };
        }

        private StringBuilder AppendParameters(StringBuilder sb, IMethodSymbol method, out bool isTryAdapterExtensionMethod)
        {
            isTryAdapterExtensionMethod = false;

            if (method.Arity > 0)
            {
                var typeParameters = string.Join(", ", method.TypeParameters.Select(GetFullName));
                sb.Append('<').Append(typeParameters).Append('>');
            }

            sb.Append('(');

            var first = true;
            foreach (var parameter in method.Parameters)
            {
                if (first)
                {
                    first = false;
                    if (method.IsExtensionMethod)
                    {
                        sb.Append("this ");

                        var key = (parameter.Type.ContainingNamespace?.ToString(), GetName(parameter.Type), GetGenericParameters(parameter.Type));
                        if (_tryCatchDefinitions.ContainsKey(key))
                        {
                            isTryAdapterExtensionMethod = true;

                            if (parameter.Type.ContainingType is not null)
                            {
                                var tryTypeName = GetTryName(key.Item2);
                                var tryType = tryTypeName + key.Item3;

                                AppendRefKind(sb, parameter.RefKind);
                                sb.Append($"{tryType} {parameter.Name}");
                            }
                            else
                            {
                                AppendRefKind(sb, parameter.RefKind);
                                sb.Append($"{GetFullName(parameter.Type, "Try")} {parameter.Name}");
                            }
                        }
                        else
                        {
                            // TODO: This should probably be an error or something.
                            AppendRefKind(sb, parameter.RefKind);
                            sb.Append($"{GetFullName(parameter.Type)} {parameter.Name}");
                        }
                    }
                    else
                    {
                        AppendRefKind(sb, parameter.RefKind);
                        sb.Append($"{GetFullName(parameter.Type)} {parameter.Name}");
                    }
                }
                else
                {
                    sb.Append(", ");
                    AppendRefKind(sb, parameter.RefKind);
                    sb.Append($"{GetFullName(parameter.Type)} {parameter.Name}");
                }

                if (parameter.HasExplicitDefaultValue)
                {
                    sb.Append($" = ");
                    if (parameter.ExplicitDefaultValue is null)
                        sb.Append("default");
                    else if (parameter.ExplicitDefaultValue is string s)
                        sb.Append('"').Append(s.Replace("\\", "\\\\").Replace("\"", "\\\"")).Append('"');
                    else if (parameter.ExplicitDefaultValue is bool b)
                        sb.Append(b ? "true" : "false");
                    else if (IsEnum(parameter.Type))
                        sb.Append($"({GetFullName(parameter.Type)})({parameter.ExplicitDefaultValue})");
                    else
                        sb.Append(parameter.ExplicitDefaultValue.ToString());
                }
            }

            sb.Append(')');
            return sb.AppendLine();
        }

        private void EnsureTryCatchDefinitionExists(ITypeSymbol type)
        {
            if (type is not INamedTypeSymbol targetType)
                return;

            var key = (targetType.ContainingNamespace?.ToString(), GetName(targetType), GetGenericParameters(targetType));
            if (!_tryCatchDefinitions.ContainsKey(key))
                _tryCatchDefinitions[key] = (targetType, new List<MethodData>());
        }
    }

    private class AttributeTarget
    {
        public AttributeTarget(INamedTypeSymbol type, string? methodName)
        {
            Type = type;
            MethodName = methodName;
        }

        public INamedTypeSymbol Type { get; }

        public string? MethodName { get; }
    }
}

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
            context.AddSource("TryExtensions.g.cs", builder.Build(context.CancellationToken)));
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

        var tryCatchtAttribute = attributeLists.SelectMany(x => x.Attributes)
            .Where(x => x.Name.ToString() is "TryCatch" or "TryCatchAttribute")
            .FirstOrDefault();

        return tryCatchtAttribute is not null;
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
                var allMembers = target.Type.GetMembers();
                var methods = allMembers.Where(symbol => IsPublicMethod(symbol, allMembers)).Select(symbol => (IMethodSymbol)symbol);

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
                    var methods = allMembers.Where(symbol => IsPublicMethod(symbol, allMembers)).Select(symbol => (IMethodSymbol)symbol);

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
            var key = (targetType.ContainingNamespace?.ToString(), targetType.Name, GetGenericParameters(targetType));
            if (_tryCatchDefinitions.ContainsKey(key))
            {
                var (_, existingMethodData) = _tryCatchDefinitions[key];

                foreach (var method in methods)
                {
                    if (!existingMethodData.Any(existingData => method.Equals(existingData.Method, SymbolEqualityComparer.Default)))
                        existingMethodData.Add(new MethodData(method, tryCatchInfo));
                }
            }
            else
            {
                _tryCatchDefinitions[key] = (targetType, methods.Select(method => new MethodData(method, tryCatchInfo)).ToList());
            }
        }

        public void AddTryCatchDefinition(IMethodSymbol method, TryCatchInfo tryCatchInfo)
        {
            var targetType = method.ContainingType;
            var key = (targetType.ContainingNamespace?.ToString(), targetType.Name, GetGenericParameters(targetType));
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
                    var variableName = GetVariableName(targetTypeName);
                    var fieldName = '_' + variableName;
                    var targetTypeTypeParameters = tryCatchDefinition.Key.TargetTypeTypeParameters;
                    var targetType = targetTypeName + targetTypeTypeParameters;
                    var targetTypeDocComments = targetType.Replace('<', '{').Replace('>', '}');

                    sb.AppendLine();

                    if (tryCatchDefinition.Value.TargetType.IsStatic)
                    {
                        AppendDocComments(sb, tryCatchDefinition.Value.TargetType.DeclaringSyntaxReferences, cancellationToken, isMethod: false);
                        sb.AppendLine($"    public static class Try{targetType}");
                        AppendGenericConstraints(sb, tryCatchDefinition.Value.TargetType.Arity, tryCatchDefinition.Value.TargetType.TypeParameters);
                        sb.Append("    {");
                    }
                    else
                    {
                        AppendDocComments(sb, tryCatchDefinition.Value.TargetType.DeclaringSyntaxReferences, cancellationToken, isMethod: false);
                        sb.AppendLine($"    public struct Try{targetType}");
                        AppendGenericConstraints(sb, tryCatchDefinition.Value.TargetType.Arity, tryCatchDefinition.Value.TargetType.TypeParameters);
                        sb.AppendLine("    {");
                        sb.AppendLine($"        private readonly {targetType} {fieldName};");
                        sb.AppendLine();
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine($"        /// Initializes a new instance of the <see cref=\"Try{targetTypeDocComments}\"/> class.");
                        sb.AppendLine("        /// </summary>");
                        sb.AppendLine($"        /// <param name=\"{variableName}\">The backing instance of <see cref=\"{targetTypeDocComments}\"/>.</param>");
                        sb.AppendLine($"        public Try{targetTypeName}({targetType} {variableName})");
                        sb.AppendLine("        {");
                        sb.AppendLine($"            {fieldName} = {variableName};");
                        sb.AppendLine("        }");
                        sb.AppendLine();
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine($"        /// Gets the backing instance of <see cref=\"{targetTypeDocComments}\"/>.");
                        sb.AppendLine("        /// </summary>");
                        sb.AppendLine($"        public {targetType} BackingValue => {fieldName};");
                    }

                    foreach (var methodData in tryCatchDefinition.Value.Methods)
                    {
                        var @static = methodData.Method.IsStatic ? "static " : null;

                        var methodName = methodData.Method.Name;
                        var returnType = methodData.Method.ReturnType;

                        sb.AppendLine();

                        AppendDocComments(sb, methodData.Method.DeclaringSyntaxReferences, cancellationToken, isMethod: true);

                        if (IsExactType(returnType, typeof(void)))
                        {
                            sb.Append($"        public {@static}RandomSkunk.Results.Result {methodName}");
                            AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                            AppendGenericConstraints(sb, methodData.Method.Arity, methodData.Method.TypeParameters);
                            sb.AppendLine("        {");
                            sb.AppendLine("            try");
                            sb.AppendLine("            {");
                            if (methodData.Method.IsStatic)
                            {
                                if (methodData.Method.IsExtensionMethod)
                                {
                                    if (isTryAdapterExtensionMethod)
                                        sb.Append($"                {methodData.Method.Parameters[0].Name}.BackingValue.{methodName}");
                                    else
                                        sb.Append($"                {methodData.Method.Parameters[0].Name}.{methodName}");
                                }
                                else
                                {
                                    sb.Append($"                {targetTypeName}.{methodName}");
                                }
                            }
                            else
                            {
                                sb.Append($"                {fieldName}.{methodName}");
                            }

                            AppendArguments(sb, methodData.Method);
                            sb.AppendLine("                return RandomSkunk.Results.Result.Success();");
                            sb.AppendLine("            }");

                            if (methodData.TryCatchInfo.TExceptions.Length == 0)
                            {
                                sb.AppendLine("            catch (Exception ex)");
                                sb.AppendLine("            {");
                                sb.AppendLine("                return RandomSkunk.Results.Result.Fail(ex);");
                                sb.AppendLine("            }");
                            }
                            else
                            {
                                foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                {
                                    sb.AppendLine($"            catch ({GetTypeName(exception)} ex)");
                                    sb.AppendLine("            {");
                                    sb.AppendLine("                return RandomSkunk.Results.Result.Fail(ex);");
                                    sb.AppendLine("            }");
                                }
                            }

                            sb.AppendLine("        }");
                        }
                        else
                        {
                            var resultType = methodData.TryCatchInfo.AsMaybe ? "RandomSkunk.Results.Maybe" : "RandomSkunk.Results.Result";

                            if (returnType.Name == "Task" && returnType.ContainingNamespace?.ToString() == "System.Threading.Tasks")
                            {
                                var namedType = (INamedTypeSymbol)returnType;

                                if (namedType.Arity == 0)
                                {
                                    sb.Append($"        public {@static}async System.Threading.Tasks.Task<RandomSkunk.Results.Result> {methodName}");
                                    AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                                    AppendGenericConstraints(sb, methodData.Method.Arity, methodData.Method.TypeParameters);
                                    sb.AppendLine("        {");
                                    sb.AppendLine("            try");
                                    sb.AppendLine("            {");
                                    if (methodData.Method.IsStatic)
                                    {
                                        if (methodData.Method.IsExtensionMethod)
                                        {
                                            if (isTryAdapterExtensionMethod)
                                                sb.Append($"                await {methodData.Method.Parameters[0].Name}.BackingValue.{methodName}");
                                            else
                                                sb.Append($"                await {methodData.Method.Parameters[0].Name}.{methodName}");
                                        }
                                        else
                                        {
                                            sb.Append($"                await {targetTypeName}.{methodName}");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append($"                await {fieldName}.{methodName}");
                                    }

                                    AppendArguments(sb, methodData.Method);
                                    sb.AppendLine("                return RandomSkunk.Results.Result.Success();");
                                    sb.AppendLine("            }");

                                    if (methodData.TryCatchInfo.TExceptions.Length == 0)
                                    {
                                        sb.AppendLine("            catch (Exception ex)");
                                        sb.AppendLine("            {");
                                        sb.AppendLine("                return RandomSkunk.Results.Result.Fail(ex);");
                                        sb.AppendLine("            }");
                                    }
                                    else
                                    {
                                        foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                        {
                                            sb.AppendLine($"            catch ({GetTypeName(exception)} ex)");
                                            sb.AppendLine("            {");
                                            sb.AppendLine("                return RandomSkunk.Results.Result.Fail(ex);");
                                            sb.AppendLine("            }");
                                        }
                                    }

                                    sb.AppendLine("        }");
                                }
                                else
                                {
                                    returnType = namedType.TypeArguments[0];

                                    sb.Append($"        public {@static}async System.Threading.Tasks.Task<{resultType}<{GetTypeName(returnType)}>> {methodName}");
                                    AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                                    AppendGenericConstraints(sb, methodData.Method.Arity, methodData.Method.TypeParameters);
                                    sb.AppendLine("        {");
                                    sb.AppendLine("            try");
                                    sb.AppendLine("            {");
                                    if (methodData.Method.IsStatic)
                                    {
                                        if (methodData.Method.IsExtensionMethod)
                                        {
                                            if (isTryAdapterExtensionMethod)
                                                sb.Append($"                var methodReturnValue = await {methodData.Method.Parameters[0].Name}.BackingValue.{methodName}");
                                            else
                                                sb.Append($"                var methodReturnValue = await {methodData.Method.Parameters[0].Name}.{methodName}");
                                        }
                                        else
                                        {
                                            sb.Append($"                var methodReturnValue = await {targetTypeName}.{methodName}");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append($"                var methodReturnValue = await {fieldName}.{methodName}");
                                    }

                                    AppendArguments(sb, methodData.Method);
                                    sb.AppendLine($"                return {resultType}<{GetTypeName(returnType)}>.FromValue(methodReturnValue);");
                                    sb.AppendLine("            }");

                                    if (methodData.TryCatchInfo.TExceptions.Length == 0)
                                    {
                                        sb.AppendLine("            catch (Exception ex)");
                                        sb.AppendLine("            {");
                                        sb.AppendLine($"                return {resultType}<{GetTypeName(returnType)}>.Fail(ex);");
                                        sb.AppendLine("            }");
                                    }
                                    else
                                    {
                                        foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                        {
                                            sb.AppendLine($"            catch ({GetTypeName(exception)} ex)");
                                            sb.AppendLine("            {");
                                            sb.AppendLine($"                return {resultType}<{GetTypeName(returnType)}>.Fail(ex);");
                                            sb.AppendLine("            }");
                                        }
                                    }

                                    sb.AppendLine("        }");
                                }
                            }
                            else
                            {
                                sb.Append($"        public {@static}{resultType}<{GetTypeName(returnType)}> {methodName}");
                                AppendParameters(sb, methodData.Method, out var isTryAdapterExtensionMethod);
                                AppendGenericConstraints(sb, methodData.Method.Arity, methodData.Method.TypeParameters);
                                sb.AppendLine("        {");
                                sb.AppendLine("            try");
                                sb.AppendLine("            {");
                                if (methodData.Method.IsStatic)
                                {
                                    if (methodData.Method.IsExtensionMethod)
                                    {
                                        if (isTryAdapterExtensionMethod)
                                            sb.Append($"                var methodReturnValue = {methodData.Method.Parameters[0].Name}.BackingValue.{methodName}");
                                        else
                                            sb.Append($"                var methodReturnValue = {methodData.Method.Parameters[0].Name}.{methodName}");
                                    }
                                    else
                                    {
                                        sb.Append($"                var methodReturnValue = {targetTypeName}.{methodName}");
                                    }
                                }
                                else
                                {
                                    sb.Append($"                var methodReturnValue = {fieldName}.{methodName}");
                                }

                                AppendArguments(sb, methodData.Method);
                                sb.AppendLine($"                return {resultType}<{GetTypeName(returnType)}>.FromValue(methodReturnValue);");
                                sb.AppendLine("            }");

                                if (methodData.TryCatchInfo.TExceptions.Length == 0)
                                {
                                    sb.AppendLine("            catch (Exception ex)");
                                    sb.AppendLine("            {");
                                    sb.AppendLine($"                return {resultType}<{GetTypeName(returnType)}>.Fail(ex);");
                                    sb.AppendLine("            }");
                                }
                                else
                                {
                                    foreach (var exception in methodData.TryCatchInfo.TExceptions)
                                    {
                                        sb.AppendLine($"            catch ({GetTypeName(exception)} ex)");
                                        sb.AppendLine("            {");
                                        sb.AppendLine($"                return {resultType}<{GetTypeName(returnType)}>.Fail(ex);");
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
                        sb.AppendLine();
                        sb.AppendLine($"    public static class {targetTypeName}TryExtensionMethod");
                        sb.AppendLine("    {");
                        sb.AppendLine($"        public static Try{targetType} Try{targetTypeTypeParameters}(this {targetType} {variableName})");
                        AppendGenericConstraints(sb, tryCatchDefinition.Value.TargetType.Arity, tryCatchDefinition.Value.TargetType.TypeParameters);
                        sb.AppendLine("        {");
                        sb.AppendLine($"            return new Try{targetType}({variableName});");
                        sb.AppendLine("        }");
                        sb.AppendLine("    }");
                    }
                }

                sb.AppendLine("}");
            }

            var source = sb.ToString();
            return source;
        }

        private static string GetVariableName(string typeName)
        {
            return char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
        }

        private static StringBuilder AppendDocComments(StringBuilder sb, IEnumerable<SyntaxReference> declaringSyntaxReferences, CancellationToken cancellationToken, bool isMethod)
        {
            var indent = isMethod ? "        " : "    ";
            foreach (var syntaxReference in declaringSyntaxReferences)
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

            return sb;
        }

        private static StringBuilder AppendArguments(StringBuilder sb, IMethodSymbol method)
        {
            if (method.Arity > 0)
            {
                var typeArguments = string.Join(", ", method.TypeParameters.Select(GetTypeName));
                sb.Append('<').Append(typeArguments).Append('>');
            }

            sb.Append('(');

            var first = true;
            foreach (var parameter in method.Parameters.Skip(method.IsExtensionMethod ? 1 : 0))
            {
                if (first) first = false;
                else sb.Append(", ");

                sb.Append(parameter.Name);
            }

            sb.Append(");");
            return sb.AppendLine();
        }

        private static StringBuilder AppendGenericConstraints(StringBuilder sb, int arity, IEnumerable<ITypeParameterSymbol> typeParameters)
        {
            if (arity == 0)
                return sb;

            foreach (var typeParameter in typeParameters)
            {
                if (typeParameter.ConstraintTypes.Length > 0
                    || typeParameter.HasConstructorConstraint
                    || typeParameter.HasNotNullConstraint
                    || typeParameter.HasReferenceTypeConstraint
                    || typeParameter.HasUnmanagedTypeConstraint
                    || typeParameter.HasValueTypeConstraint)
                {
                    sb.Append($"            where {typeParameter.Name} : ");
                }

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
                    sb.Append(GetComma()).Append(GetTypeName(constraintType));

                if (typeParameter.HasConstructorConstraint)
                    sb.Append(GetComma()).Append("new()");

                string? GetComma()
                {
                    if (comma is not null)
                        return comma;

                    comma = ", ";
                    return null;
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

            var typeParameters = string.Join(", ", namedType.TypeParameters.Select(GetTypeName));
            return $"<{typeParameters}>";
        }

        private static string GetTypeName(ITypeSymbol type) => GetTypeName(type, null);

        private static string GetTypeName(ITypeSymbol type, string? typeNamePrefix)
        {
            if (type.Kind == SymbolKind.TypeParameter)
            {
                return type.Name;
            }

            if (type is IArrayTypeSymbol arrayType)
            {
                var commas = arrayType.Rank == 1 ? null : string.Join(string.Empty, Enumerable.Repeat(',', arrayType.Rank - 1));
                var elementType = GetTypeName(arrayType.ElementType, typeNamePrefix);
                return $"{elementType}[{commas}]";
            }

            if (type is not INamedTypeSymbol { Arity: > 0 } namedType)
                return $"{type.ContainingNamespace}.{typeNamePrefix}{type.Name}";

            var typeArguments = string.Join(", ", namedType.TypeArguments.Select(ta => GetTypeName(ta, typeNamePrefix)));
            return $"{type.ContainingNamespace}.{typeNamePrefix}{type.Name}<{typeArguments}>";
        }

        private StringBuilder AppendParameters(StringBuilder sb, IMethodSymbol method, out bool isTryAdapterExtensionMethod)
        {
            isTryAdapterExtensionMethod = false;

            if (method.Arity > 0)
            {
                var typeParameters = string.Join(", ", method.TypeParameters.Select(GetTypeName));
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

                        var key = (parameter.Type.ContainingNamespace?.ToString(), parameter.Type.Name, GetGenericParameters(parameter.Type));
                        if (_tryCatchDefinitions.ContainsKey(key))
                        {
                            isTryAdapterExtensionMethod = true;
                            sb.Append($"{GetTypeName(parameter.Type, "Try")} {parameter.Name}");
                        }
                        else
                        {
                            sb.Append($"{GetTypeName(parameter.Type)} {parameter.Name}");
                        }
                    }
                    else
                    {
                        sb.Append($"{GetTypeName(parameter.Type)} {parameter.Name}");
                    }
                }
                else
                {
                    sb.Append(", ");
                    sb.Append($"{GetTypeName(parameter.Type)} {parameter.Name}");
                }

                if (parameter.HasExplicitDefaultValue)
                {
                    sb.Append($" = ");
                    if (parameter.ExplicitDefaultValue is null)
                        sb.Append("default");
                    else if (parameter.ExplicitDefaultValue is string s)
                        sb.Append('"').Append(s.Replace("\\", "\\\\").Replace("\"", "\\\"")).Append('"');
                    else if (IsEnum(parameter.Type))
                        sb.Append($"({GetTypeName(parameter.Type)})({parameter.ExplicitDefaultValue})");
                    else
                        sb.Append(parameter.ExplicitDefaultValue.ToString());
                }
            }

            sb.Append(')');
            return sb.AppendLine();
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

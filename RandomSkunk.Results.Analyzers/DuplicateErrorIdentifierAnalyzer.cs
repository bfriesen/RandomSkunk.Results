using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace RandomSkunk.Results.Analyzers;

/// <summary>
/// An analyzer that ensures that access to the result properties <c>Value</c> and <c>Error</c> is done safely.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DuplicateErrorIdentifierAnalyzer : DiagnosticAnalyzer
{
    private const string _title = "Duplicate error identifier";
    private const string _messageFormat = "The error identifier '{0}' is used more than once in the solution";
    private const string _category = "Usage";

    private static readonly DiagnosticDescriptor _rule = new(
        DiagnosticIds.DuplicateErrorIdentifier,
        _title,
        _messageFormat,
        _category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: string.Format(CultureInfo.InvariantCulture, HelpLinkUri.Format, DiagnosticIds.DuplicateErrorIdentifier));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(OnCompilationStart);
    }

    private void OnCompilationStart(CompilationStartAnalysisContext context)
    {
        var errorType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Error");
        if (errorType is null)
            return;

        var resultType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Result");
        if (resultType is null)
            return;

        var resultOfTType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Result`1");
        if (resultOfTType is null)
            return;

        var maybeOfTType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Maybe`1");
        if (maybeOfTType is null)
            return;

        var errorIdentifierTracker = new ErrorIdentifierTracker(_rule, errorType, resultType, resultOfTType, maybeOfTType);

        context.RegisterOperationAction(
            errorIdentifierTracker.OnOperation,
            OperationKind.Invocation,
            OperationKind.ObjectCreation,
            OperationKind.With);
    }

    private class ErrorIdentifierTracker
    {
        private readonly Dictionary<string, (SyntaxNode Node, bool AlreadyReported)> _nodesByErrorIdentifier = new();

        private readonly DiagnosticDescriptor _rule;
        private readonly INamedTypeSymbol _errorType;
        private readonly INamedTypeSymbol _resultType;
        private readonly INamedTypeSymbol _resultOfTType;
        private readonly INamedTypeSymbol _maybeOfTType;

        public ErrorIdentifierTracker(
            DiagnosticDescriptor rule,
            INamedTypeSymbol errorType,
            INamedTypeSymbol resultType,
            INamedTypeSymbol resultOfTType,
            INamedTypeSymbol maybeOfTType)
        {
            _rule = rule;
            _errorType = errorType;
            _resultType = resultType;
            _resultOfTType = resultOfTType.ConstructUnboundGenericType();
            _maybeOfTType = maybeOfTType.ConstructUnboundGenericType();
        }

        public void OnOperation(OperationAnalysisContext context)
        {
            if (context.Operation.Kind == OperationKind.Invocation)
                OnInvocationOperation(context);
            else if (context.Operation.Kind == OperationKind.ObjectCreation)
                OnObjectCreationOperation(context);
            else if (context.Operation.Kind == OperationKind.With)
                OnWithOperation(context);
        }

        private static ILiteralOperation? GetLiteralForItentifierInitializer(IObjectOrCollectionInitializerOperation? initializer)
        {
            var identifierInitializer = initializer?.Initializers
                .OfType<ISimpleAssignmentOperation>()
                .FirstOrDefault(initializer =>
                    initializer.Target is IPropertyReferenceOperation property
                        && property.Member.Name == "Identifier"
                        && initializer.Value is ILiteralOperation);

            return (ILiteralOperation?)identifierInitializer?.Value;
        }

        private void OnInvocationOperation(OperationAnalysisContext context)
        {
            var invocationOperation = (IInvocationOperation)context.Operation;

            var errorIdentifierArgument = invocationOperation.Arguments.FirstOrDefault(arg =>
                arg.Parameter?.Name == "errorIdentifier" && arg.Value is ILiteralOperation);

            if (errorIdentifierArgument is null)
                return;

            if (invocationOperation.TargetMethod.Name == "Fail")
            {
                var potentialResultType = invocationOperation.TargetMethod.ContainingType;

                if (potentialResultType.IsGenericType && !potentialResultType.IsUnboundGenericType)
                    potentialResultType = potentialResultType.ConstructUnboundGenericType();

                if (!SymbolEqualityComparer.Default.Equals(potentialResultType, _resultType)
                    && !SymbolEqualityComparer.Default.Equals(potentialResultType, _resultOfTType)
                    && !SymbolEqualityComparer.Default.Equals(potentialResultType, _maybeOfTType))
                {
                    return;
                }
            }
            else
            {
                if (!IsErrorType(invocationOperation.TargetMethod.ReturnType))
                    return;
            }

            var errorIdentifierLiteral = (ILiteralOperation)errorIdentifierArgument.Value;
            ProcessErrorIdentifierLiteral(errorIdentifierLiteral, context);
        }

        private void OnObjectCreationOperation(OperationAnalysisContext context)
        {
            var objectCreationOperation = (IObjectCreationOperation)context.Operation;

            if (!IsErrorType(objectCreationOperation.Type) || objectCreationOperation.Initializer is null)
                return;

            var errorIdentifierLiteral = GetLiteralForItentifierInitializer(objectCreationOperation.Initializer);
            ProcessErrorIdentifierLiteral(errorIdentifierLiteral, context);
        }

        private void OnWithOperation(OperationAnalysisContext context)
        {
            var withOperation = (IWithOperation)context.Operation;

            if (!IsErrorType(withOperation.Type))
                return;

            var errorIdentifierLiteral = GetLiteralForItentifierInitializer(withOperation.Initializer);
            ProcessErrorIdentifierLiteral(errorIdentifierLiteral, context);
        }

        private bool IsErrorType(ITypeSymbol? targetType)
        {
            if (targetType is null)
                return false;

            if (SymbolEqualityComparer.Default.Equals(_errorType, targetType))
                return true;

            if (IsErrorType(targetType.BaseType))
                return true;

            foreach (var interfaceType in targetType.AllInterfaces)
            {
                if (SymbolEqualityComparer.Default.Equals(targetType, interfaceType))
                    return true;
            }

            return false;
        }

        private void ProcessErrorIdentifierLiteral(ILiteralOperation? errorIdentifierLiteral, OperationAnalysisContext context)
        {
            if (errorIdentifierLiteral is null)
                return;

            var identifier = (string)errorIdentifierLiteral.ConstantValue.Value!;

            if (_nodesByErrorIdentifier.TryGetValue(identifier, out var t))
            {
                if (!t.AlreadyReported)
                {
                    _nodesByErrorIdentifier[identifier] = (t.Node, true);
                    context.ReportDiagnostic(Diagnostic.Create(_rule, t.Node.GetLocation(), identifier));
                }

                context.ReportDiagnostic(Diagnostic.Create(_rule, errorIdentifierLiteral.Syntax.GetLocation()));
            }
            else
            {
                _nodesByErrorIdentifier.Add(identifier, (errorIdentifierLiteral.Syntax, false));
            }
        }
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Globalization;

namespace RandomSkunk.Results.Analyzers;

/// <summary>
/// An analyzer that ensures that detects a condition for maybe that can be simplified.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SimplifyResultExpressionAnalyzer : DiagnosticAnalyzer
{
    private const string _title = "Result expression can be simplified";
    private const string _messageFormat = "The result expression can be simplified";
    private const string _category = "Usage";

    private static readonly DiagnosticDescriptor _rule = new(
        DiagnosticIds.ResultExpressionCanBeSimplified,
        _title,
        _messageFormat,
        _category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: string.Format(CultureInfo.InvariantCulture, HelpLinkUri.Format, DiagnosticIds.UnsafeUsageOfResultProperty));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(OnCompilationStart);
    }

    private static void OnCompilationStart(CompilationStartAnalysisContext context)
    {
        var maybeOfTType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Maybe`1");
        if (maybeOfTType is null)
            return;

        var analyzer = new ResultExpressionAnalyzer(maybeOfTType);
        context.RegisterOperationAction(analyzer.Analyze, OperationKind.Binary);
    }

    private class ResultExpressionAnalyzer
    {
        private readonly INamedTypeSymbol _typeofMaybeOfT;

        public ResultExpressionAnalyzer(INamedTypeSymbol maybeOfTType)
        {
            _typeofMaybeOfT = maybeOfTType.ConstructUnboundGenericType();
        }

        public void Analyze(OperationAnalysisContext context)
        {
            var binaryOperation = (IBinaryOperation)context.Operation;

            switch (binaryOperation.OperatorKind)
            {
                case BinaryOperatorKind.ConditionalAnd:
                case BinaryOperatorKind.ConditionalOr:
                case BinaryOperatorKind.And:
                case BinaryOperatorKind.Or:
                    break;
                default:
                    return;
            }

            var leftWalker = new ConditionWalker(_typeofMaybeOfT);
            leftWalker.Process(binaryOperation.LeftOperand);

            var rightWalker = new ConditionWalker(_typeofMaybeOfT);
            rightWalker.Process(binaryOperation.RightOperand);

            if (leftWalker.IsResultPropertyCondition && rightWalker.IsResultPropertyCondition)
            {
                if ((binaryOperation.OperatorKind is BinaryOperatorKind.ConditionalAnd or BinaryOperatorKind.And
                    && !leftWalker.Value && !rightWalker.Value)
                    || (leftWalker.Value && rightWalker.Value))
                {
                    if (leftWalker.ResultProperty == "IsNone" || rightWalker.ResultProperty == "IsNone")
                    {
                        var otherWalker = leftWalker.ResultProperty == "IsNone" ? rightWalker : leftWalker;

                        if (otherWalker.ResultProperty != "IsNone")
                        {
                            string propertyName;
                            if (otherWalker.ResultProperty == "IsSuccess")
                                propertyName = "IsFail";
                            else
                                propertyName = "IsSuccess";

                            var builder = ImmutableDictionary.CreateBuilder<string, string?>();
                            builder.Add("PropertyName", propertyName);
                            builder.Add("InstanceName", leftWalker.InstanceName);
                            builder.Add("PropertyValue", leftWalker.Value ? "false" : "true");

                            context.ReportDiagnostic(Diagnostic.Create(_rule, binaryOperation.Syntax.GetLocation(), builder.ToImmutable()));
                        }
                    }
                }
            }
        }

        private class ConditionWalker : OperationWalker
        {
            private readonly INamedTypeSymbol _typeofMaybeOfT;
            private bool _isInBinaryEqualsOperation;
            private ILiteralOperation? _binaryLiteralOperation;

            public ConditionWalker(INamedTypeSymbol typeofMaybeOfT)
            {
                _typeofMaybeOfT = typeofMaybeOfT;
            }

            public bool Value { get; private set; } = true;

            public bool IsResultPropertyCondition { get; private set; }

            public string? ResultProperty { get; private set; }

            public string? InstanceName { get; private set; }

            public void Process(IOperation condition)
            {
                Visit(condition);
            }

            public override void VisitUnaryOperator(IUnaryOperation operation)
            {
                if (operation.OperatorKind == UnaryOperatorKind.Not)
                    Value = !Value;

                base.VisitUnaryOperator(operation);
            }

            public override void VisitBinaryOperator(IBinaryOperation operation)
            {
                if (operation.OperatorKind == BinaryOperatorKind.Equals)
                {
                    _isInBinaryEqualsOperation = true;
                }

                base.VisitBinaryOperator(operation);

                if (_binaryLiteralOperation is not null)
                {
                    Value = (bool)_binaryLiteralOperation.ConstantValue.Value!;
                    _binaryLiteralOperation = null;
                }

                _isInBinaryEqualsOperation = false;
            }

            public override void VisitLiteral(ILiteralOperation operation)
            {
                if (_isInBinaryEqualsOperation)
                {
                    _binaryLiteralOperation = operation;
                }

                base.VisitLiteral(operation);
            }

            public override void VisitPropertyReference(IPropertyReferenceOperation operation)
            {
                if (operation.Property.Name is "IsSuccess" or "IsFail" or "IsNone")
                {
                    if (operation.Instance?.Kind is OperationKind.LocalReference or OperationKind.ParameterReference)
                    {
                        var potentialResultType = operation.Property.ContainingType;

                        if (potentialResultType.IsGenericType && !potentialResultType.IsUnboundGenericType)
                            potentialResultType = potentialResultType.ConstructUnboundGenericType();

                        if (SymbolEqualityComparer.Default.Equals(potentialResultType, _typeofMaybeOfT))
                        {
                            IsResultPropertyCondition = true;
                            ResultProperty = operation.Property.Name;
                            InstanceName = operation.Instance.Syntax.ToString();
                        }
                    }
                }

                base.VisitPropertyReference(operation);
            }
        }
    }
}

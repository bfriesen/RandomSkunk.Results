using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;
using System.Globalization;

namespace RandomSkunk.Results.Analyzers;

/// <summary>
/// An analyzer that ensures that access to the result properties <c>Value</c> and <c>Error</c> is done safely.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UnsafeResultUsageAnalyzer : DiagnosticAnalyzer
{
    private const string _title = "Unsafe usage of result property";
    private const string _messageFormat = "The '{0}' property can only be safely accessed when '{1}' is true";
    private const string _category = "Usage";

    private static readonly DiagnosticDescriptor _rule = new(
        DiagnosticIds.UnsafeUsageOfResultProperty,
        _title,
        _messageFormat,
        _category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: string.Format(CultureInfo.InvariantCulture, HelpLinkUri.Format, DiagnosticIds.UnsafeUsageOfResultProperty));

    /// <summary>
    /// Represents the known state of a result.
    /// </summary>
    private interface IResultState
    {
        /// <summary>
        /// Gets a value indicating whether the result's IsSuccess property is known to be true, known to be false, or unknown.
        /// </summary>
        bool? IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the result's IsFail property is known to be true, known to be false, or unknown.
        /// </summary>
        bool? IsFail { get; }

        /// <summary>
        /// Sets the current state of a property.
        /// </summary>
        /// <param name="propertyName">The property (should be "IsSuccess", "IsFail", or "IsNone").</param>
        /// <param name="value">Whether the property is known to be true or false.</param>
        void SetState(string propertyName, bool value);

        /// <summary>
        /// Inverts the last state.
        /// </summary>
        void InvertLastState();

        /// <summary>
        /// Clears the state.
        /// </summary>
        void Clear();
    }

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
        var resultType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Result");
        if (resultType is null)
            return;

        var resultOfTType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Result`1");
        if (resultOfTType is null)
            return;

        var maybeOfTType = context.Compilation.GetTypeByMetadataName("RandomSkunk.Results.Maybe`1");
        if (maybeOfTType is null)
            return;

        var analyzer = new PropertyReferenceOperationAnalyzer(resultType, resultOfTType, maybeOfTType);
        context.RegisterOperationAction(analyzer.Analyze, OperationKind.PropertyReference);
    }

    private class PropertyReferenceOperationAnalyzer
    {
        private readonly INamedTypeSymbol _typeofResult;
        private readonly INamedTypeSymbol _typeofResultOfT;
        private readonly INamedTypeSymbol _typeofMaybeOfT;

        public PropertyReferenceOperationAnalyzer(INamedTypeSymbol resultType, INamedTypeSymbol resultOfTType, INamedTypeSymbol maybeOfTType)
        {
            _typeofResult = resultType;
            _typeofResultOfT = resultOfTType.ConstructUnboundGenericType();
            _typeofMaybeOfT = maybeOfTType.ConstructUnboundGenericType();
        }

        public void Analyze(OperationAnalysisContext context)
        {
            var propertyReferenceOperation = (IPropertyReferenceOperation)context.Operation;
            if (propertyReferenceOperation.Instance is null)
                return;

            if (propertyReferenceOperation.Property.Name is not "Value" and not "Error")
                return;

            var potentialResultType = propertyReferenceOperation.Property.ContainingType;

            if (potentialResultType.IsGenericType && !potentialResultType.IsUnboundGenericType)
                potentialResultType = potentialResultType.ConstructUnboundGenericType();

            IResultState state;
            if (SymbolEqualityComparer.Default.Equals(potentialResultType, _typeofResult)
                || SymbolEqualityComparer.Default.Equals(potentialResultType, _typeofResultOfT))
            {
                state = new ResultState();
            }
            else if (SymbolEqualityComparer.Default.Equals(potentialResultType, _typeofMaybeOfT))
            {
                state = new MaybeState();
            }
            else
            {
                return;
            }

            var methodBody = GetContainingMethodBody(propertyReferenceOperation);
            if (methodBody is null)
                return;

            var visitor = new ResultStateWalker(context, propertyReferenceOperation, state);
            visitor.Visit(methodBody);
        }

        private static IMethodBodyOperation? GetContainingMethodBody(IOperation operation)
        {
            var parent = operation;

            while (parent is not null)
            {
                if (parent is IMethodBodyOperation methodBodyOperation)
                    return methodBodyOperation;

                parent = parent.Parent;
            }

            return null;
        }

        private class ResultStateWalker : OperationWalker
        {
            private readonly OperationAnalysisContext _context;
            private readonly IPropertyReferenceOperation _propertyReferenceOperation;
            private readonly IResultState _state;

            public ResultStateWalker(OperationAnalysisContext context, IPropertyReferenceOperation propertyReferenceOperation, IResultState state)
            {
                _context = context;
                _propertyReferenceOperation = propertyReferenceOperation;
                _state = state;

                // TODO: Check to see if we're accessing the property directly from a method call. If so, immediately trigger the diagnostic.
            }

            public override void Visit(IOperation? operation)
            {
                if (operation == _propertyReferenceOperation)
                {
                    if (_propertyReferenceOperation.Property.Name == "Error")
                    {
                        if (_state.IsFail != true)
                        {
                            _context.ReportDiagnostic(Diagnostic.Create(_rule, _propertyReferenceOperation.Syntax.GetLocation(), "Error", "IsFail"));
                        }
                    }
                    else if (_propertyReferenceOperation.Property.Name == "Value")
                    {
                        if (_state.IsSuccess != true)
                        {
                            _context.ReportDiagnostic(Diagnostic.Create(_rule, _propertyReferenceOperation.Syntax.GetLocation(), "Value", "IsSuccess"));
                        }
                    }

                    return;
                }

                if (operation is IConditionalOperation conditionalOperation)
                {
                    var conditionWalker = new ConditionWalker(_propertyReferenceOperation, _state, this);

                    conditionWalker.ProcessCondition(conditionalOperation.Condition);
                    conditionWalker.ProcessWhenTrue(conditionalOperation.WhenTrue);
                    conditionWalker.ProcessWhenFalse(conditionalOperation.WhenFalse);

                    if (conditionWalker.IsResultPropertyCondition)
                        return;
                }

                base.Visit(operation);
            }

            private class ConditionWalker : OperationWalker
            {
                private readonly IPropertyReferenceOperation _propertyReferenceOperation;
                private readonly IResultState _state;
                private readonly ResultStateWalker _parent;
                private bool _isResultPropertyCondition;
                private string? _propertyName;
                private bool _value = true;

                private bool _isInBinaryEqualsOperation;
                private ILiteralOperation? _binaryLiteralOperation;

                private bool _hasEarlyReturn;

                public ConditionWalker(IPropertyReferenceOperation propertyReferenceOperation, IResultState state, ResultStateWalker parent)
                {
                    _propertyReferenceOperation = propertyReferenceOperation;
                    _state = state;
                    _parent = parent;
                }

                public bool IsResultPropertyCondition => _isResultPropertyCondition;

                public void ProcessCondition(IOperation condition)
                {
                    Visit(condition);
                }

                public void ProcessWhenTrue(IOperation whenTrue)
                {
                    if (!_isResultPropertyCondition)
                        return;

                    _state.SetState(_propertyName!, _value);

                    _hasEarlyReturn = false;

                    Visit(whenTrue);

                    if (_hasEarlyReturn)
                    {
                        _state.InvertLastState();
                    }

                    _parent.Visit(whenTrue);
                }

                public void ProcessWhenFalse(IOperation? whenFalse)
                {
                    if (!_isResultPropertyCondition)
                        return;

                    if (whenFalse is not null)
                    {
                        _state.InvertLastState();

                        _parent.Visit(whenFalse);
                    }

                    if (!_hasEarlyReturn)
                        _state.Clear();
                }

                public override void VisitUnaryOperator(IUnaryOperation operation)
                {
                    if (operation.OperatorKind == UnaryOperatorKind.Not)
                        _value = !_value;

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
                        _value = (bool)_binaryLiteralOperation.ConstantValue.Value!;
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
                        if ((operation.Instance is ILocalReferenceOperation conditionLocalReference
                            && _propertyReferenceOperation.Instance is ILocalReferenceOperation capturedLocalReference
                            && SymbolEqualityComparer.Default.Equals(conditionLocalReference.Local, capturedLocalReference.Local))
                            || (operation.Instance is IParameterReferenceOperation conditionParameterReference
                                && _propertyReferenceOperation.Instance is IParameterReferenceOperation capturedParameterReference
                                && SymbolEqualityComparer.Default.Equals(conditionParameterReference.Parameter, capturedParameterReference.Parameter)))
                        {
                            _isResultPropertyCondition = true;
                            _propertyName = operation.Property.Name;
                        }
                    }

                    base.VisitPropertyReference(operation);
                }

                public override void VisitReturn(IReturnOperation operation)
                {
                    _hasEarlyReturn = true;

                    base.VisitReturn(operation);
                }
            }
        }

        private class ResultState : IResultState
        {
            private bool? _isSuccess;
            private bool? _isFail;

            private string? _lastStateProperty;
            private bool _lastState;

            public bool? IsSuccess => _isSuccess;

            public bool? IsFail => _isFail;

            public void SetState(string propertyName, bool value)
            {
                switch (propertyName)
                {
                    case "IsSuccess":
                        _isSuccess = value;
                        _isFail = !value;
                        break;
                    case "IsFail":
                        _isFail = value;
                        _isSuccess = !value;
                        break;
                    default:
                        throw new ArgumentException();
                }

                _lastStateProperty = propertyName;
                _lastState = value;
            }

            public void InvertLastState()
            {
                switch (_lastStateProperty)
                {
                    case "IsSuccess":
                        _isSuccess = !_lastState;
                        _isFail = _lastState;
                        break;
                    case "IsFail":
                        _isFail = !_lastState;
                        _isSuccess = _lastState;
                        break;
                }

                _lastStateProperty = null;
            }

            public void Clear()
            {
                _isSuccess = null;
                _isFail = null;
                _lastStateProperty = null;
            }
        }

        private class MaybeState : IResultState
        {
            private bool? _isSuccess;
            private bool? _isFail;
            private bool? _isNone;

            private string? _lastStateProperty;
            private bool _lastState;
            private (bool? IsSuccess, bool? IsFail, bool? IsNone) _lastStateSnapshot;

            public bool? IsSuccess => _isSuccess;

            public bool? IsFail => _isFail;

            public bool? IsNone => _isNone;

            public void SetState(string propertyName, bool value)
            {
                switch (propertyName)
                {
                    case "IsSuccess":
                        _lastStateSnapshot = (_isSuccess, _isFail, _isNone);
                        _isSuccess = value;

                        if (value)
                        {
                            _isFail = false;
                            _isNone = false;
                        }
                        else
                        {
                            if (_isFail == false)
                                _isNone = true;
                            else if (_isNone == false)
                                _isFail = true;
                        }

                        break;
                    case "IsFail":
                        _lastStateSnapshot = (_isSuccess, _isFail, _isNone);
                        _isFail = value;

                        if (value)
                        {
                            _isSuccess = false;
                            _isNone = false;
                        }
                        else
                        {
                            if (_isSuccess == false)
                                _isNone = true;
                            else if (_isNone == false)
                                _isSuccess = true;
                        }

                        break;
                    case "IsNone":
                        _lastStateSnapshot = (_isSuccess, _isFail, _isNone);
                        _isNone = value;

                        if (value)
                        {
                            _isSuccess = false;
                            _isFail = false;
                        }
                        else
                        {
                            if (_isSuccess == false)
                                _isFail = true;
                            else if (_isFail == false)
                                _isSuccess = true;
                        }

                        break;
                    default:
                        throw new ArgumentException();
                }

                _lastStateProperty = propertyName;
                _lastState = value;
            }

            public void InvertLastState()
            {
                _isSuccess = _lastStateSnapshot.IsSuccess;
                _isFail = _lastStateSnapshot.IsFail;
                _isNone = _lastStateSnapshot.IsNone;

                SetState(_lastStateProperty!, !_lastState);

                _lastStateProperty = null;
                _lastStateSnapshot = default;
            }

            public void Clear()
            {
                _isSuccess = null;
                _isFail = null;
                _isNone = null;
                _lastStateProperty = null;
                _lastStateSnapshot = default;
            }
        }
    }
}

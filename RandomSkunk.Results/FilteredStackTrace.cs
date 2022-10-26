namespace RandomSkunk.Results;

internal class FilteredStackTrace : StackTrace
{
    private static readonly StackFrame _emptyStackFrame = new(int.MaxValue);
    private static readonly ConditionalWeakTable<MethodBase, MethodData> _methodDataLookup = new();

    private readonly int _skipFramesBeforeThisIndex;
    private readonly int _skipFramesAfterThisIndex = int.MaxValue;

    private FilteredStackTrace()
        : base(true)
    {
        var frameCount = FrameCount;

        for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
        {
            if (base.GetFrame(frameIndex)?.GetMethod() is MethodBase method)
            {
                var methodData = _methodDataLookup.GetValue(method, m => new MethodData(m));

                if (methodData.IsMarkedWithStackTraceHidden)
                {
                    _skipFramesBeforeThisIndex = frameIndex + 1;
                    continue;
                }

                if (methodData.IsMarkedWithStackTraceBoundary)
                {
                    _skipFramesAfterThisIndex = frameIndex;
                    break;
                }

                if (methodData.IsAspNetCoreMvcActionMethodExecutor)
                {
                    _skipFramesAfterThisIndex = frameIndex - 2;
                    break;
                }

                if (methodData.IsXunitTestInvoker)
                {
                    _skipFramesAfterThisIndex = frameIndex - 4;
                    break;
                }

                if (methodData.IsLinqPadExecutionModelClrQueryRunner)
                {
                    _skipFramesAfterThisIndex = frameIndex - 1;
                    break;
                }
            }
        }
    }

    public static string Create() => new FilteredStackTrace().ToString();

    public override StackFrame? GetFrame(int index)
    {
        if (index < _skipFramesBeforeThisIndex || index > _skipFramesAfterThisIndex)
            return _emptyStackFrame;

        return base.GetFrame(index);
    }

    public override string ToString() => base.ToString().TrimEnd();

    private static IEnumerable<CustomAttributeData> GetCustomAttributes(MemberInfo? member)
    {
        if (member is null)
            return Enumerable.Empty<CustomAttributeData>();

        try
        {
            return member.CustomAttributes;
        }
        catch (NotImplementedException)
        {
            return Enumerable.Empty<CustomAttributeData>();
        }
    }

    private class MethodData
    {
        public MethodData(MethodBase method)
        {
            var methodAttributes = GetCustomAttributes(method);
            var declaringTypeAttributes = GetCustomAttributes(method.DeclaringType);

            IsMarkedWithStackTraceHidden = methodAttributes.Any(a => a.AttributeType == typeof(StackTraceHiddenAttribute))
                || declaringTypeAttributes.Any(a => a.AttributeType == typeof(StackTraceHiddenAttribute));

            IsMarkedWithStackTraceBoundary = methodAttributes.Any(a => a.AttributeType == typeof(StackTraceBoundaryAttribute));

            IsAspNetCoreMvcActionMethodExecutor = method.Name == "MoveNext"
                && method.DeclaringType?.FullName?.StartsWith("Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor") == true;

            IsXunitTestInvoker = method.DeclaringType?.FullName == "Xunit.Sdk.TestInvoker`1";

            IsLinqPadExecutionModelClrQueryRunner = method.Name == "Run"
                && method.DeclaringType?.FullName == "LINQPad.ExecutionModel.ClrQueryRunner";
        }

        public bool IsMarkedWithStackTraceHidden { get; }

        public bool IsMarkedWithStackTraceBoundary { get; }

        public bool IsAspNetCoreMvcActionMethodExecutor { get; }

        public bool IsXunitTestInvoker { get; }

        public bool IsLinqPadExecutionModelClrQueryRunner { get; }
    }
}

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

                if (methodData.IsMarkedAsHidden)
                {
                    _skipFramesBeforeThisIndex = frameIndex + 1;
                    continue;
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
        private readonly MethodBase _method;

        private bool? _isMarkedAsHidden;
        private bool? _isXunitTestInvoker;
        private bool? _isMvcActionMethodExecutor;

        public MethodData(MethodBase method)
        {
            _method = method;
        }

        public bool IsMarkedAsHidden
        {
            get
            {
                if (!_isMarkedAsHidden.HasValue)
                {
                    _isMarkedAsHidden = GetCustomAttributes(_method).Any(a => a.AttributeType == typeof(StackTraceHiddenAttribute))
                        || GetCustomAttributes(_method.DeclaringType).Any(a => a.AttributeType == typeof(StackTraceHiddenAttribute));
                }

                return _isMarkedAsHidden.Value;
            }
        }

        public bool IsXunitTestInvoker
        {
            get
            {
                if (!_isXunitTestInvoker.HasValue)
                {
                    _isXunitTestInvoker = _method.DeclaringType?.FullName == "Xunit.Sdk.TestInvoker`1";
                }

                return _isXunitTestInvoker.Value;
            }
        }

        public bool IsAspNetCoreMvcActionMethodExecutor
        {
            get
            {
                if (!_isMvcActionMethodExecutor.HasValue)
                {
                    _isMvcActionMethodExecutor = _method.Name == "MoveNext"
                        && _method.DeclaringType?.FullName.StartsWith("Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor") == true;
                }

                return _isMvcActionMethodExecutor.Value;
            }
        }
    }
}

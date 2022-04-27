namespace RandomSkunk.Results.UnitTests;

public class ResultBase_T__should
{
    [Fact]
    public void Have_default_behavior()
    {
        ResultBase<int> result = new DefaultResultBase<int>();

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeFalse();
        result.CallSite.Should().Be(default(CallSite));

        Action accessingError = () => _ = result.Error;
        accessingError.Should().Throw<InvalidOperationException>();

        Action accessingValue = () => _ = result.Value;
        accessingValue.Should().Throw<InvalidOperationException>();
    }

    private class DefaultResultBase<T> : ResultBase<T>
    {
        public DefaultResultBase()
            : base(default)
        {
        }
    }
}

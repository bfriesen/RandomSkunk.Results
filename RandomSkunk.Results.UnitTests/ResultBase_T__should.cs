namespace RandomSkunk.Results.UnitTests;

public class ResultBase_T__should
{
    [Fact]
    public void Have_default_behavior()
    {
        ResultBase<int> result = new DefaultResultBase<int>();

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeFalse();
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
        Accessing.Value(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessValueUnlessSuccessMessage);
    }

    private class DefaultResultBase<T> : ResultBase<T>
    {
    }
}

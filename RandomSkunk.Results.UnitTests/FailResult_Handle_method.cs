namespace RandomSkunk.Results.UnitTests;

[Collection(nameof(FailResult))]
public class FailResult_Handle_method
{
    [CollectionDefinition(nameof(FailResult), DisableParallelization = true)]
    public class FailResultCollectionDefinition
    {
    }
}

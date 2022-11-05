<Query Kind="Program">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Each of the Result, Result<T>, and Maybe<T> types have the same three Fail method overloads.
    // The examples below all use Result (because it's the simplest), but Result<T> and Maybe<T>
    // both create Fail results in exactly the same way.
    
    // When no parameters are passed to the Fail method, a generic error is created.
    Result.Fail().Dump("Empty");

    // An instance of Error can be passed to the Fail method. This is the "main" Fail method, the
    // other two Fail methods below ultimately call this method.
    Error error = new Error { Message = "Uh, oh" };
    Result.Fail(error).Dump("Error provided");
    
    // Just the error's message can be provided, along with any additional optional components.
    Result.Fail("Not again!", errorCode: 404).Dump("Message and optional components provided");
    
    // When an exception is used to create the Fail result, the exception is mapped to the InnerError
    // of the Fail result's Error. The components of the outer error are provided through the
    // optional parameters.
    Exception exception = GetException();
    Result.Fail(exception, errorMessage: "My outer error.").Dump("Exception provided");
}

#region Support Code

private static Exception GetException()
{
    try
    {	        
        int i = 0, j = 0;
        int k = i / j;
        return null!;
    }
    catch (Exception ex)
    {
        return ex;
    }
}

#endregion

namespace RandomSkunk.Results;

/// <summary>
/// Defines a call site in source code.
/// </summary>
/// <param name="MemberName">The name of the member where the call originated.</param>
/// <param name="FilePath">The path to the source file where the call originated.</param>
/// <param name="LineNumber">The line number where the call originated.</param>
public record struct CallSite(string MemberName, string FilePath, int LineNumber);

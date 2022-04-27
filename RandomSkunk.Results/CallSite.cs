namespace RandomSkunk.Results;

/// <summary>
/// Defines a call site in source code.
/// </summary>
public struct CallSite
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CallSite"/> struct.
    /// </summary>
    /// <param name="memberName">The name of the member where the call originated.</param>
    /// <param name="filePath">The path to the source file where the call originated.</param>
    /// <param name="lineNumber">The line number where the call originated.</param>
    public CallSite(string memberName, string filePath, int lineNumber)
    {
        MemberName = memberName;
        FilePath = filePath;
        LineNumber = lineNumber;
    }

    /// <summary>
    /// Gets the name of the member where the call originated.
    /// </summary>
    public string MemberName { get; }

    /// <summary>
    /// Gets the path to the source file where the call originated.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets the line number where the call originated.
    /// </summary>
    public int LineNumber { get; }
}

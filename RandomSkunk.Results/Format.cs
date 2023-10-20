using System.Text.RegularExpressions;

namespace RandomSkunk.Results;

internal static class Format
{
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly Regex _wordBreak = new(
        @"
        _+                  # One or more underscores.

        |                   # ...or...

        (?<=\p{Ll})         # A position after a lowercase,
        (?=\p{Lu})          # then the same position before an uppercase.
                            #   Example: Between 'e' and 'R' in 'HomeRun'.

        |                   # ...or...

        (?<=\p{Lu})         # A position after an uppercase,
        (?=\p{Lu}\p{Ll})    # then the same position before an uppercase followed by a lowercase.
                            #   Example: Between 'L' and 'D' in 'XMLDocument'.

        |                   # ...or...

        (?<=\p{L})          # A position after a letter,
        (?=\p{N})           # then the same position before a number.
                            #   Example: Between 'c' and '1' in 'Abc123'.

        |                   # ...or...

        (?<=\p{N})          # A position after a number,
        (?=\p{L})           # then the same position before a letter.
                            #   Example: Between '3' and 'A' in '123Abc'.
",
        RegexOptions.IgnorePatternWhitespace);
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

    private static readonly MatchEvaluator _replaceWithSingleSpace = m => " ";

    public static string AsSentenceCase(string csharpIdentifier) =>
        _wordBreak.Replace(csharpIdentifier, _replaceWithSingleSpace);
}

#if NETSTANDARD2_0

using System.Runtime.CompilerServices;

namespace System;

internal readonly struct Range : IEquatable<Range>
{
    public Range(Index start, Index end)
    {
        Start = start;
        End = end;
    }

    public static Range All => new(Index.Start, Index.End);

    public Index Start { get; }

    public Index End { get; }

    public static Range StartAt(Index start) => new(start, Index.End);

    public static Range EndAt(Index end) => new(Index.Start, end);

    public override bool Equals(object? value) =>
        value is Range r &&
        r.Start.Equals(Start) &&
        r.End.Equals(End);

    public bool Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

    public override int GetHashCode()
    {
        return (Start.GetHashCode() * 31) + End.GetHashCode();
    }

    public override string ToString()
    {
        return Start + ".." + End;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        int start;
        var startIndex = Start;
        if (startIndex.IsFromEnd)
            start = length - startIndex.Value;
        else
            start = startIndex.Value;

        int end;
        var endIndex = End;
        if (endIndex.IsFromEnd)
            end = length - endIndex.Value;
        else
            end = endIndex.Value;

        if ((uint)end > (uint)length || (uint)start > (uint)end)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        return (start, end - start);
    }
}

#endif

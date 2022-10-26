#if NETSTANDARD2_0

using System.Runtime.CompilerServices;

namespace System;

internal readonly struct Index : IEquatable<Index>
{
    private readonly int _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Index(int value, bool fromEnd = false)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
        }

        if (fromEnd)
            _value = ~value;
        else
            _value = value;
    }

    private Index(int value)
    {
        _value = value;
    }

    public static Index Start => new(0);

    public static Index End => new(~0);

    public int Value
    {
        get
        {
            if (_value < 0)
            {
                return ~_value;
            }
            else
            {
                return _value;
            }
        }
    }

    public bool IsFromEnd => _value < 0;

    public static implicit operator Index(int value) => FromStart(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index FromStart(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
        }

        return new Index(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index FromEnd(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
        }

        return new Index(~value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetOffset(int length)
    {
        var offset = _value;
        if (IsFromEnd)
        {
            // offset = length - (~value)
            // offset = length + (~(~value) + 1)
            // offset = length + value + 1
            offset += length + 1;
        }

        return offset;
    }

    public override bool Equals(object? value) => value is Index index && _value == index._value;

    public bool Equals(Index other) => _value == other._value;

    public override int GetHashCode() => _value;

    public override string ToString()
    {
        if (IsFromEnd)
            return "^" + ((uint)Value).ToString();

        return ((uint)Value).ToString();
    }
}

#endif

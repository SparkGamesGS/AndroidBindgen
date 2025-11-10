// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public struct ExceptionTableEntry : IHasReference, IEquatable<ExceptionTableEntry>
{
    private readonly ushort _catchType;

    internal ExceptionTableEntry(ref BinaryReader reader)
    {
        Start = reader.ReadUnaligned<U2>();
        End = reader.ReadUnaligned<U2>();
        Handler = reader.ReadUnaligned<U2>();
        _catchType = reader.ReadUnaligned<U2>();
        CatchType = null;
    }

    public ushort Start { get; }
    public ushort End { get; }
    public ushort Handler { get; }
    public ConstantClass? CatchType { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        CatchType = classFile.ConstantPool.Span[_catchType]?.As<ConstantClass>();
    }

    #endregion

    public bool Equals(ExceptionTableEntry other)
    {
        return _catchType == other._catchType && Start == other.Start && End == other.End &&
               Handler == other.Handler && Equals(CatchType, other.CatchType);
    }

    public override bool Equals(object? obj)
    {
        return obj is ExceptionTableEntry other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_catchType, Start, End, Handler, CatchType);
    }

    public static bool operator ==(ExceptionTableEntry left, ExceptionTableEntry right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ExceptionTableEntry left, ExceptionTableEntry right)
    {
        return !left.Equals(right);
    }
}

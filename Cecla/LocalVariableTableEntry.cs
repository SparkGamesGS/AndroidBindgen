// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public struct LocalVariableTableEntry : IEquatable<LocalVariableTableEntry>
{
    internal LocalVariableTableEntry(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        Start = reader.ReadUnaligned<U2>();
        Length = reader.ReadUnaligned<U2>();
        Name = unresolvedConstantPool[reader.ReadUnaligned<U2>()].AsUtf8();
        Descriptor = unresolvedConstantPool[reader.ReadUnaligned<U2>()].AsUtf8();
        Index = reader.ReadUnaligned<U2>();
    }

    public ushort Start { get; }
    public ushort Length { get; }
    public string Name { get; }
    public string Descriptor { get; }
    public ushort Index { get; }

    public bool Equals(LocalVariableTableEntry other)
    {
        return Start == other.Start && Length == other.Length && Name == other.Name &&
               Descriptor == other.Descriptor && Index == other.Index;
    }

    public override bool Equals(object? obj)
    {
        return obj is LocalVariableTableEntry other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, Length, Name, Descriptor, Index);
    }

    public static bool operator ==(LocalVariableTableEntry left, LocalVariableTableEntry right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LocalVariableTableEntry left, LocalVariableTableEntry right)
    {
        return !left.Equals(right);
    }
}

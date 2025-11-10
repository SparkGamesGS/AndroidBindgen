// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class AttributeLocalVariableTable : IAttribute
{
    internal AttributeLocalVariableTable(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var localVariableTableLength = reader.ReadUnaligned<U2>().Value;
        var localVariableTable = new LocalVariableTableEntry[localVariableTableLength];
        for (var i = 0; i < localVariableTableLength; i++)
            localVariableTable[i] = new LocalVariableTableEntry(ref reader, unresolvedConstantPool);
        LocalVariableTable = localVariableTable;
    }

    public ReadOnlyMemory<LocalVariableTableEntry> LocalVariableTable { get; }

    #region IAttribute Members

    public string Name => "LocalVariableTable";

    #endregion
}

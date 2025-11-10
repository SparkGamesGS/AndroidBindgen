// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class AttributeUnknown : IAttribute
{
    internal AttributeUnknown(ref BinaryReader reader, string name)
    {
        Name = name;
        Data = reader.Read(checked((int)reader.Available)).ToArray();
    }

    public byte[] Data { get; set; }

    #region IAttribute Members

    public string Name { get; }

    #endregion
}

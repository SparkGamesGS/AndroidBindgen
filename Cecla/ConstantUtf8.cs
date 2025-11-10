// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Text;

namespace Cecla;

public class ConstantUtf8 : IConstant
{
    internal unsafe ConstantUtf8(ref BinaryReader reader)
    {
        var length = reader.ReadUnaligned<U2>().Value;
        if (length == 0)
        {
            Value = "";
            return;
        }

        fixed (byte* b = reader.Read(length))
        {
            Value = Encoding.UTF8.GetString(b, length);
        }
    }

    public string Value { get; }
}

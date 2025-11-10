// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Text;

namespace AndroidBindgen;

public class MarshalingContext
{
    private readonly string _postCallIndent;
    private readonly string _preCallIndent;
    public readonly StringBuilder PostCall = new();
    public readonly StringBuilder PreCall = new();
    private int _cursor;

    public MarshalingContext(string preCallIndent = "        ", string postCallIndent = "            ")
    {
        _preCallIndent = preCallIndent;
        _postCallIndent = postCallIndent;
    }

    public string AllocLocalName()
    {
        return $"__ABG_marshal_{_cursor++}";
    }

    public void AppendPreCallLine(string content)
    {
        PreCall.Append(_preCallIndent);
        PreCall.AppendLine(content);
    }

    public void AppendPostCallLine(string content)
    {
        PostCall.Append(_postCallIndent);
        PostCall.AppendLine(content);
    }
}

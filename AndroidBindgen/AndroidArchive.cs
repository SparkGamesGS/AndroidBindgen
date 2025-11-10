// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.IO.Compression;
using Cecla;

namespace AndroidBindgen;

public class AndroidArchive : JavaArchive
{
    private ZipArchive? _archive;

    public AndroidArchive(string path) : this(ZipFile.OpenRead(path))
    {
    }

    private AndroidArchive(ZipArchive aar) : base(new ZipArchive(aar.GetEntry("classes.jar")!.Open()))
    {
        _archive = aar;
    }

    public override void Dispose()
    {
        base.Dispose();
        _archive?.Dispose();
        _archive = null;
    }
}

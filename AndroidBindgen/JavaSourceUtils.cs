// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AndroidBindgen;

public class JavaSourceUtils
{
    private readonly string _output;
    private readonly Process _process;

    public JavaSourceUtils(string executableJarPath, string[] jars, string[] sources, out StreamReader stdout,
        out StreamReader stderr)
    {
        _output = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xml");
        _process = new Process
        {
            StartInfo = new ProcessStartInfo("java",
                $"-jar {executableJarPath} {string.Join("", jars.Select(j => $"--jar \"{j}\" "))}--output-javadoc {_output} {string.Join("", sources.Select(j => $"\"{j}\" "))}")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
        _process.Start();
        stdout = _process.StandardOutput;
        stderr = _process.StandardError;
    }

    public string GetResultPath()
    {
        _process.WaitForExit();
        if (_process.ExitCode != 0) throw new InvalidOperationException("Failed to generate javadoc xml.");
        return _output;
    }
}

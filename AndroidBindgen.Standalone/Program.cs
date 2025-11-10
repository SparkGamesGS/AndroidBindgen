// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using AndroidBindgen;
using Cecla;
using ConsoleAppFramework;

// ReSharper disable once RedundantLambdaParameterType
ConsoleApp.Run(args,
    (string[] classes, string[] source, string[]? jar = null, string[]? aar = null, bool ignoreDeprecated = true) =>
    {
        // javadoc
        Console.WriteLine("Generating javadoc...");
        var javaSourceUtils =
            new JavaSourceUtils("java-source-utils.jar", jar ?? [], source, out var stdout, out var stderr);

        _ = ReadStreamAsync(stdout);
        _ = ReadStreamAsync(stderr);

        var javadocPath = javaSourceUtils.GetResultPath();
        Console.WriteLine("Generated.");

        var javadocManager = new JavadocManager([javadocPath]);
        var classManager =
            new ClassManager(
                (jar?.Select(j => new JavaArchive(j)) ?? []).Concat(aar?.Select(a => new AndroidArchive(a)) ?? []));
        var generator = new Generator(classManager, ignoreDeprecated, javadocManager);

        Directory.CreateDirectory("out");

        foreach (var c in classes)
            generator.RequestFullMembers(c);

        generator.Generate("out");
    });
return;


async Task ReadStreamAsync(StreamReader reader)
{
    while (!reader.EndOfStream)
        Console.WriteLine(await reader.ReadLineAsync());
}

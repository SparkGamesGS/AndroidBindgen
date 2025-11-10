// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

internal static class ConstantExtensions
{
    public static T As<T>(this IConstant constant) where T : IConstant
    {
        return (T)constant ?? throw new InvalidClassException();
    }

    public static string AsUtf8(this IConstant constant)
    {
        return constant.As<ConstantUtf8>().Value;
    }
}

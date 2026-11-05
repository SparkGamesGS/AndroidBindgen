// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Globalization;

namespace AndroidBindgen;

internal static class Utils
{
    /// <summary>
    ///     The fully qualified C# type name of a Java class, rooted at <c>global::</c> so a binding namespace such as
    ///     <c>com.android</c> cannot shadow <c>android</c> inside another <c>com.*</c> namespace.
    /// </summary>
    public static string ToGlobalTypeName(this string javaClassName)
    {
        return $"global::{javaClassName.Replace('/', '.').Replace('$', '.').ToSafeIdentifier()}";
    }

    public static string ToSafeIdentifier(this string source)
    {
        var cursor = 0;
        while ((cursor = source.IndexOf('.', cursor)) >= 0)
        {
            cursor++;
            if (source[cursor] is >= '0' and <= '9')
                source = $"{source[..cursor]}@_{source[cursor..]}";
            else
                source = $"{source[..cursor]}@{source[cursor..]}";
        }

        return $"@{source}";
    }

    public static IParameterType ParseDescriptor(string descriptor, ref int cursor)
    {
        var next = descriptor[cursor++];

        switch (next)
        {
            case 'L':
                var start = cursor;
                // ReSharper disable once EmptyEmbeddedStatement
                while (descriptor[cursor++] != ';') ;
                var name = descriptor.Substring(start, cursor - start - 1);

                if (name == "java/lang/String")
                    return new ParameterTypeString();

                return new ParameterTypeObject(name);
            case '[':
                return new ParameterTypeArray(ParseDescriptor(descriptor, ref cursor));
            case 'B':
                return new ParameterTypeImmediate(ParamTypeKind.SByte);
            case 'C':
                return new ParameterTypeImmediate(ParamTypeKind.Char);
            case 'D':
                return new ParameterTypeImmediate(ParamTypeKind.Double);
            case 'F':
                return new ParameterTypeImmediate(ParamTypeKind.Float);
            case 'I':
                return new ParameterTypeImmediate(ParamTypeKind.Int);
            case 'J':
                return new ParameterTypeImmediate(ParamTypeKind.Long);
            case 'S':
                return new ParameterTypeImmediate(ParamTypeKind.Short);
            case 'Z':
                return new ParameterTypeImmediate(ParamTypeKind.Boolean);
            case 'V':
                return new ParameterTypeImmediate(ParamTypeKind.Void);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void RequestDependencyForType(this IParameterType type, IGenerator generator)
    {
        switch (type)
        {
            case ParameterTypeArray parameterTypeArray:
                RequestDependencyForType(parameterTypeArray.ElementType, generator);
                break;
            case ParameterTypeImmediate:
                break;
            case ParameterTypeObject parameterTypeObject:
                generator.Request(parameterTypeObject.ClassName, IApiFilter.FacadeFilter);
                break;
            case ParameterTypeString:
                generator.Request("java/lang/String", IApiFilter.FacadeFilter);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    public static string ToCSharpIdentifierCompatible(string value, bool applyStartCharacterConstraint = true)
    {
        Span<char> paramNameSpan = stackalloc char[value.Length];
        value.AsSpan().CopyTo(paramNameSpan);

        var insertStartUnderscore = applyStartCharacterConstraint && !IsCSharpIdentifierStart(paramNameSpan[0]);

        var part = insertStartUnderscore ? paramNameSpan : paramNameSpan[1..];

        for (var i = 0; i < part.Length; i++)
            if (!IsCSharpIdentifierPart(part[i]))
                part[i] = '_';

        Span<char> resultSpan = stackalloc char[paramNameSpan.Length + (insertStartUnderscore ? 1 : 0)];

        if (insertStartUnderscore)
        {
            resultSpan[0] = '_';
            paramNameSpan.CopyTo(resultSpan[1..]);
        }
        else
        {
            paramNameSpan.CopyTo(resultSpan);
        }

        return resultSpan.ToString();
    }

    private static bool IsCSharpIdentifierStart(char c)
    {
        return c == '_' || IsCSharpIdentifierLetter(c);
    }

    private static bool IsCSharpIdentifierPart(char c)
    {
        return IsCSharpIdentifierLetter(c) ||
               CharUnicodeInfo.GetUnicodeCategory(c) is
                   UnicodeCategory.DecimalDigitNumber or
                   UnicodeCategory.ConnectorPunctuation or
                   UnicodeCategory.NonSpacingMark or
                   UnicodeCategory.SpacingCombiningMark or
                   UnicodeCategory.Format;
    }

    private static bool IsCSharpIdentifierLetter(char c)
    {
        return CharUnicodeInfo.GetUnicodeCategory(c) is
            UnicodeCategory.LowercaseLetter or
            UnicodeCategory.ModifierLetter or
            UnicodeCategory.OtherLetter or
            UnicodeCategory.TitlecaseLetter or
            UnicodeCategory.UppercaseLetter or
            UnicodeCategory.LetterNumber;
    }
}

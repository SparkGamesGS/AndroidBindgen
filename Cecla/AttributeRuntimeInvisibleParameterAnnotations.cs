// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class AttributeRuntimeInvisibleParameterAnnotations : IAttribute, IHasReference
{
    internal AttributeRuntimeInvisibleParameterAnnotations(ref BinaryReader reader,
        ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var numParameters = reader.ReadUnaligned<byte>();
        var annotations = new ReadOnlyMemory<Annotation>[numParameters];
        for (var i = 0; i < numParameters; i++)
        {
            var numAnnotations = reader.ReadUnaligned<U2>().Value;
            var parameterAnnotations = new Annotation[numAnnotations];
            for (var j = 0; j < numAnnotations; j++)
                parameterAnnotations[j] = new Annotation(ref reader, unresolvedConstantPool);
            annotations[i] = parameterAnnotations;
        }

        Annotations = annotations;
    }

    public ReadOnlyMemory<ReadOnlyMemory<Annotation>> Annotations { get; }

    #region IAttribute Members

    public string Name => "RuntimeInvisibleParameterAnnotations";

    #endregion

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        foreach (var parameterAnnotations in Annotations.Span)
        foreach (var annotation in parameterAnnotations.Span)
            ((IHasReference)annotation).ResolveReference(classFile);
    }

    #endregion
}

// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class AttributeRuntimeVisibleAnnotations : IAttribute, IHasReference
{
    internal AttributeRuntimeVisibleAnnotations(ref BinaryReader reader,
        ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var numAnnotations = reader.ReadUnaligned<U2>().Value;
        var annotations = new Annotation[numAnnotations];
        for (var i = 0; i < numAnnotations; i++)
            annotations[i] = new Annotation(ref reader, unresolvedConstantPool);
        Annotations = annotations;
    }

    public ReadOnlyMemory<Annotation> Annotations { get; }

    #region IAttribute Members

    public string Name => "RuntimeVisibleAnnotations";

    #endregion

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        foreach (var annotation in Annotations.Span)
            ((IHasReference)annotation).ResolveReference(classFile);
    }

    #endregion
}

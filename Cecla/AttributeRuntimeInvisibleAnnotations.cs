// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class AttributeRuntimeInvisibleAnnotations : IAttribute, IHasReference
{
    public ReadOnlyMemory<Annotation> Annotations;

    internal AttributeRuntimeInvisibleAnnotations(ref BinaryReader reader,
        ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var numAnnotations = reader.ReadUnaligned<U2>().Value;
        var annotations = new Annotation[numAnnotations];
        for (var i = 0; i < numAnnotations; i++)
            annotations[i] = new Annotation(ref reader, unresolvedConstantPool);
        Annotations = annotations;
    }

    #region IAttribute Members

    public string Name => "RuntimeInvisibleAnnotations";

    #endregion

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        foreach (var annotation in Annotations.Span)
            ((IHasReference)annotation).ResolveReference(classFile);
    }

    #endregion
}

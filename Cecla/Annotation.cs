// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cecla;

public class Annotation : IHasReference
{
    private readonly Dictionary<string, IElement> _elementValuePairs;

    internal Annotation(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        Type = unresolvedConstantPool[reader.ReadUnaligned<U2>()].AsUtf8();
        var count = reader.ReadUnaligned<U2>().Value;
        _elementValuePairs = new Dictionary<string, IElement>(count);
        for (var i = 0; i < count; i++)
        {
            var elementName = unresolvedConstantPool[reader.ReadUnaligned<U2>()].AsUtf8();
            _elementValuePairs.Add(elementName, ClassFile.ReadElement(ref reader, unresolvedConstantPool));
        }
    }

    public string Type { get; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        foreach (var element in _elementValuePairs.Values.OfType<IHasReference>())
            element.ResolveReference(classFile);
    }

    #endregion
}

public interface IElement
{
}

public class ElementConstant : IElement, IHasReference
{
    private readonly ushort _constantValueIndex;
    public byte Tag;

    internal ElementConstant(ref BinaryReader reader, byte tag)
    {
        Tag = tag;
        _constantValueIndex = reader.ReadUnaligned<U2>();
    }

    public IConstant? ConstantValue { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        ConstantValue = classFile.ConstantPool.Span[_constantValueIndex];
    }

    #endregion
}

public class ElementEnumConstant : IElement, IHasReference
{
    private readonly ushort _constNameIndex;
    private readonly ushort _typeNameIndex;

    internal ElementEnumConstant(ref BinaryReader reader)
    {
        _typeNameIndex = reader.ReadUnaligned<U2>();
        _constNameIndex = reader.ReadUnaligned<U2>();
    }

    public string? TypeName { get; private set; }
    public string? ConstName { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        TypeName = classFile.ConstantPool.Span[_typeNameIndex].AsUtf8();
        ConstName = classFile.ConstantPool.Span[_constNameIndex].AsUtf8();
    }

    #endregion
}

public class ElementClass : IElement, IHasReference
{
    private readonly ushort _classInfoIndex;

    internal ElementClass(ref BinaryReader reader)
    {
        _classInfoIndex = reader.ReadUnaligned<U2>();
    }

    public string? ClassName { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        ClassName = classFile.ConstantPool.Span[_classInfoIndex].AsUtf8();
    }

    #endregion
}

public class ElementAnnotation : IElement, IHasReference
{
    internal ElementAnnotation(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        Annotation = new Annotation(ref reader, unresolvedConstantPool);
    }

    public Annotation Annotation { get; }

    #region IHasReference Members

    public void ResolveReference(ClassFile classFile)
    {
        ((IHasReference)Annotation)?.ResolveReference(classFile);
    }

    #endregion
}

public class ElementArray : IElement
{
    internal ElementArray(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var count = reader.ReadUnaligned<U2>().Value;
        Values = new IElement[count];
        for (var i = 0; i < count; i++)
            Values[i] = ClassFile.ReadElement(ref reader, unresolvedConstantPool);
    }

    public IElement[] Values { get; }
}

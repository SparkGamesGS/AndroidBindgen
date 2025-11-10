// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Compression;

namespace Cecla;

public class JavaArchive : IDisposable
{
    private readonly bool _disposeWithSelf;
    private readonly Dictionary<ZipArchiveEntry, ClassFile> _entryAndClassFiles = new();

    private readonly Dictionary<string, ClassFile?> _nameAndClassFiles = new();
    private ZipArchive? _zipArchive;

    public JavaArchive(string path)
    {
        _zipArchive = ZipFile.OpenRead(path);
        _disposeWithSelf = true;
    }

    public JavaArchive(ZipArchive zipArchive, bool disposeWithSelf = true)
    {
        _zipArchive = zipArchive;
        _disposeWithSelf = disposeWithSelf;
    }

    #region IDisposable Members

    public virtual void Dispose()
    {
        if (_disposeWithSelf)
        {
            _zipArchive?.Dispose();
            _zipArchive = null;
        }
    }

    #endregion

    public IEnumerable<(ClassFile @class, string name)> GetClasses()
    {
        foreach (var entry in _zipArchive!.Entries)
        {
            if (!entry.FullName.EndsWith(".class")) continue;

            ClassFile @class;
            try
            {
                @class = GetClass(entry) ?? throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to read {entry.FullName}: {e.Message}");
                //continue;
                throw;
            }

            yield return (@class, entry.FullName.Substring(0, entry.FullName.Length - 6));
        }
    }

    public ClassFile? GetClass(string name)
    {
        if (_nameAndClassFiles.TryGetValue(name, out var classFile))
            return classFile;

        var path = name.Replace('.', '/') + ".class";
        var entry = _zipArchive!.GetEntry(path);
        var @class = GetClass(entry);
        _nameAndClassFiles[name] = @class;
        return @class;
    }

    private ClassFile? GetClass(ZipArchiveEntry? entry)
    {
        if (entry == null)
            return null;

        if (_entryAndClassFiles.TryGetValue(entry, out var classFile))
            return classFile;

        using var stream = entry.Open();

        var length = entry.Length;
        var buffer = new byte[length];

        for (var cursor = 0; cursor < length;)
            cursor += stream.Read(buffer, cursor, checked((int)length) - cursor);

        var reader = new BinaryReader(buffer);
        classFile = new ClassFile(ref reader);
        _entryAndClassFiles[entry] = classFile;
        return classFile;
    }

    #region Nested type: ReadOnlySequenceSegment

    private class ReadOnlySequenceSegment : ReadOnlySequenceSegment<byte>
    {
        public ReadOnlySequenceSegment(ReadOnlyMemory<byte> memory)
        {
            Memory = memory;
            RunningIndex = 0;
        }

        private ReadOnlySequenceSegment(ReadOnlyMemory<byte> memory, long runningIndex)
        {
            Memory = memory;
            RunningIndex = runningIndex;
        }

        public ReadOnlySequenceSegment CreateNext(ReadOnlyMemory<byte> memory)
        {
            if (Next != null) throw new InvalidOperationException();
            var next = new ReadOnlySequenceSegment(memory, RunningIndex + Memory.Length);
            Next = next;
            return next;
        }
    }

    #endregion
}

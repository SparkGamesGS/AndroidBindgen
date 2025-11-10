// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cecla;

internal ref struct BinaryReader
{
    private static readonly Encoding Utf8 = new UTF8Encoding(false, true);

    private ReadOnlySequence<byte> _next;
    private ReadOnlySpan<byte> _first;
    private byte[]? _tempBuffer;

    public int Position { get; private set; }
    public long Available => _first.Length + _next.Length;

    public BinaryReader(in ReadOnlySequence<byte> buffer)
    {
        _first = buffer.First.Span;
        _next = buffer.Slice(_first.Length);
        _tempBuffer = null;
        Position = 0;
    }

    public BinaryReader(ReadOnlySpan<byte> buffer)
    {
        _next = ReadOnlySequence<byte>.Empty;
        _first = buffer;
        _tempBuffer = null;
        Position = 0;
    }

    public ref byte ReadRef(int size)
    {
        return ref MemoryMarshal.GetReference(Read(size));
    }

    public ReadOnlySpan<byte> Read(int size)
    {
        Position += size;

        if (size <= _first.Length)
        {
            var result = _first.Slice(0, size);
            _first = _first.Slice(size);

            if (_first.IsEmpty)
            {
                _first = _next.First.Span;
                _next = _next.Slice(_first.Length);
            }

            return result;
        }

        if (_tempBuffer != null)
        {
            ArrayPool<byte>.Shared.Return(_tempBuffer);
            _tempBuffer = null;
        }

        _tempBuffer = ArrayPool<byte>.Shared.Rent(size);

        var remains = size;
        var dest = _tempBuffer.AsSpan();
        while (remains > 0)
        {
            if (_first.Length == 0) throw new InvalidClassException();
            var consume = Math.Min(remains, _first.Length);

            _first.Slice(0, consume).CopyTo(dest);
            dest = dest.Slice(consume);
            _first = _first.Slice(consume);

            if (_first.IsEmpty)
            {
                _first = _next.First.Span;
                _next = _next.Slice(_first.Length);
            }

            remains -= consume;
        }

        return _tempBuffer.AsSpan(0, size);
    }

    public BinaryReader Clone()
    {
        var result = new BinaryReader
        {
            _first = _first,
            _next = _next,
            Position = Position
        };

        return result;
    }

    public void Dispose()
    {
        if (_tempBuffer != null)
        {
            ArrayPool<byte>.Shared.Return(_tempBuffer);
            _tempBuffer = null;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ReadUnaligned<T>() where T : struct

    {
        var read = Read(Unsafe.SizeOf<T>());

        return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(read));
    }
}

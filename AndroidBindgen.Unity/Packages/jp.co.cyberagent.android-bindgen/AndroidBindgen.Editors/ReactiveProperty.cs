// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace AndroidBindgen.Editors
{
    internal class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private readonly HashSet<Action<T, T>> _observers = new();
        private T _value;

        public ReactiveProperty(T value = default)
        {
            _value = value;
        }

        #region IReactiveProperty<T> Members

        public T Value
        {
            get => _value;
            set
            {
                if (typeof(T).IsEnum)
                {
                    var underlyingType = typeof(T).GetEnumUnderlyingType();
                    if (UnsafeUtility.SizeOf(underlyingType) switch
                        {
                            1 => UnsafeUtility.As<T, byte>(ref _value) == UnsafeUtility.As<T, byte>(ref value),
                            2 => UnsafeUtility.As<T, ushort>(ref _value) == UnsafeUtility.As<T, ushort>(ref value),
                            4 => UnsafeUtility.As<T, uint>(ref _value) == UnsafeUtility.As<T, uint>(ref value),
                            8 => UnsafeUtility.As<T, ulong>(ref _value) == UnsafeUtility.As<T, ulong>(ref value),
                            _ => throw new NotSupportedException()
                        }) return;
                }
                else if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                var oldValue = _value;
                _value = value;
                List<Exception> exceptions = null;
                foreach (var observer in _observers)
                    try
                    {
                        observer(oldValue, value);
                    }
                    catch (Exception e)
                    {
                        exceptions ??= new List<Exception>();
                        exceptions.Add(e);
                    }

                if (exceptions != null)
                    throw new AggregateException(exceptions);
            }
        }

        public IDisposable Subscribe(Action<T, T> onNext)
        {
            if (!_observers.Add(onNext)) return Disposable.Empty;
            return Disposable.Create(onNext, o => _observers.Remove(o));
        }

        #endregion
    }
}

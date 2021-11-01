using System;
using System.Collections.Generic;

public sealed class ReactiveProperty<T> where T : struct
{
    private readonly List<Action<T>> _listeners = new List<Action<T>>();
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;

            for(int i = 0; i < _listeners.Count; i++)
                _listeners[i](_value);
        }
    }

    public void Reset()
    {
        _listeners.Clear();
    }

    public void Subscribe(Action<T> callback)
    {
        _listeners.Add(callback);
    }

    public void Unsubscribe(Action<T> callback)
    {
        _listeners.Remove(callback);
    }
}
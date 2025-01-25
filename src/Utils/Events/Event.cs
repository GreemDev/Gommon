using System;
using System.Collections.Generic;

namespace Gommon;

public class Event<T>
{
    private readonly object _subLock = new();
    private readonly List<Action<T>> _subscriptions = [];
    
    public bool HasSubscribers
    {
        get
        {
            lock (_subLock)
                return _subscriptions.Count != 0;
        }
    }

    public IReadOnlyList<Action<T>> Subscriptions
    {
        get
        {
            lock (_subLock)
                return _subscriptions;
        }
    }

    public void Add(Action<T> subscriber)
    {
        Guard.Require(subscriber, nameof(subscriber));
        lock (_subLock)
            _subscriptions.Add(subscriber);
    }

    public void Remove(Action<T> subscriber)
    {
        Guard.Require(subscriber, nameof(subscriber));
        lock (_subLock)
            _subscriptions.Remove(subscriber);
    }

    public void Clear()
    {
        lock (_subLock)
            _subscriptions.Clear();
    }

    public void Call(T arg) 
        => Subscriptions.ForEach(action => action(arg));
}
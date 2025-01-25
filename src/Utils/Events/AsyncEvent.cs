using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gommon;

public class AsyncEvent<T>
{
    private readonly object _subLock = new();
    private readonly List<Func<T, Task>> _subscriptions = [];
    
    public bool HasSubscribers
    {
        get
        {
            lock (_subLock)
                return _subscriptions.Count != 0;
        }
    }

    public IReadOnlyList<Func<T, Task>> Subscriptions
    {
        get
        {
            lock (_subLock)
                return _subscriptions;
        }
    }

    public void Add(Func<T, Task> subscriber)
    {
        Guard.Require(subscriber, nameof(subscriber));
        lock (_subLock)
            _subscriptions.Add(subscriber);
    }

    public void Remove(Func<T, Task> subscriber)
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

    public Task CallAsync(T arg) 
        => Subscriptions.ForEachAsync(action => action(arg));
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gommon;

public class EventWithQueue<T>
{
    private readonly object _subLock = new();
    private readonly List<Action<T>> _subscriptions = [];

    private readonly Queue<T> _handlerlessEvents = [];
    
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
    {
        var subscribers = Subscriptions;
        
        if (subscribers.Count == 0)
        {
            _handlerlessEvents.Enqueue(arg);
            return;
        }
            
        if (_handlerlessEvents.Count > 0)
            while (_handlerlessEvents.TryDequeue(out var queuedArg))
                subscribers.ForEach(func => func(queuedArg));
        
        subscribers.ForEach(action => action(arg));
    }
}
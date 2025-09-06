using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gommon;

public class AsyncEventWithQueue<T>
{
    private readonly object _subLock = new();
    private readonly List<Func<T, Task>> _subscriptions = [];

    private readonly Queue<T> _handlerlessEvents = [];

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

    public async Task CallAsync(T arg)
    {
        IReadOnlyList<Func<T, Task>> subscribers;

        lock (_subLock)
        {
            subscribers = Subscriptions;
        }

        if (subscribers.Count == 0)
        {
            _handlerlessEvents.Enqueue(arg);
            return;
        }

        if (_handlerlessEvents.Count > 0)
            while (_handlerlessEvents.TryDequeue(out var queuedArg))
                await subscribers.ForEachAsync(func => func(queuedArg));

        await subscribers.ForEachAsync(action => action(arg));
    }
}
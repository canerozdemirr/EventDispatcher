using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class EventDispatcher
{
    private static readonly Lazy<EventDispatcher> _instance = new(() => new EventDispatcher());
    public static EventDispatcher Instance => _instance.Value;

    private readonly Dictionary<Type, List<Delegate>> _eventDictionary = new();

    private EventDispatcher()
    {
    }

    public void Subscribe<T>(Action<T> handler) where T: IEvent
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler), "The handler method cannot be null");

        if (_eventDictionary.TryGetValue(typeof(T), out var existingHandlers))
        {
            existingHandlers.Add(handler);
        }
        else
        {
            _eventDictionary[typeof(T)] = new List<Delegate> {handler};
        }
    }

    public void Unsubscribe<T>(Action<T> handler) where T: IEvent
    {
        if (handler == null) return;

        if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> existingHandlers))
        {
            existingHandlers.Remove(handler);
            if (existingHandlers.Count == 0)
            {
                _eventDictionary.Remove(typeof(T));
            }
        }
    }

    public void Dispatch<T>(T payload) where T: IEvent
    {
        if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> handlers))
        {
            foreach (Delegate handler in handlers)
            {
                try
                {
                    ((Action<T>) handler)?.Invoke(payload);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occurred while dispatching the event: {e.Message}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No subscribers for event type: {typeof(T)}");
        }
    }

    public void UnsubscribeAll<T>() where T: IEvent
    {
        _eventDictionary.Remove(typeof(T));
    }

    public void Clear()
    {
        _eventDictionary.Clear();
    }

    public void Dispatch<T>(params T[] events) where T: IEvent
    {
        foreach (var eventPayload in events)
        {
            Dispatch(eventPayload);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher
{
    private static EventDispatcher _instance;
    public static EventDispatcher Instance => _instance ??= new EventDispatcher();
    private EventDispatcher() { }

    private Dictionary<Type, List<Delegate>> _eventDictionary = new();

    /// <summary>
    /// Subscribes a handler to an event of type T.
    /// </summary>
    /// <param name="handler">The handler action.</param>
    /// <typeparam name="T">Event type</typeparam>
    public void Subscribe<T>(Action<T> handler)
    {
        if (handler == null) 
        {
            throw new ArgumentNullException("The handler method cannot be null");
        }
        
        if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> existingHandlers))
        {
            existingHandlers.Add(handler);
        }
        else
        {
            _eventDictionary[typeof(T)] = new List<Delegate> { handler };
        }
    }

    /// <summary>
    /// Unsubscribes a handler from an event of type T.
    /// </summary>
    /// <param name="handler">The handler action.</param>
    /// <typeparam name="T">Event type</typeparam>
    public void Unsubscribe<T>(Action<T> handler)
    {
        if (handler == null) 
            return;
        
        if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> existingHandlers))
        {
            existingHandlers.Remove(handler);
            if (existingHandlers.Count == 0)
            {
                _eventDictionary.Remove(typeof(T));
            }
        }
    }

    /// <summary>
    /// Dispatches a handler to with an event of type T.
    /// </summary>
    /// <param name="payload">The handler action.</param>
    /// <typeparam name="T">Event type</typeparam>
    public void Dispatch<T>(T payload)
    {
        if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> handlers))
        {
            if(handlers == null || handlers.Count == 0)
            {
                Debug.LogWarning($"No subscribers for event type: {typeof(T)}");
                return;
            }
            foreach (Delegate handler in handlers)
            {
                try
                { 
                    ((Action<T>)handler)?.Invoke(payload);
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
    
    /// <summary>
    /// Unsubscribes all handlers from an event of type T.
    /// </summary>
    /// <typeparam name="T">Event type</typeparam>
    public void UnsubscribeAll<T>()
    {
        _eventDictionary.Remove(typeof(T));
    }
    
    /// <summary>
    /// Clears all event subscriptions.
    /// </summary>
    public void Clear()
    {
        _eventDictionary.Clear();
    }
    
    /// <summary>
    /// Dispatches multiple events. Useful for ordered dispatching.
    /// </summary>
    /// <param name="events">The observer list.</param>
    /// <typeparam name="T">Event type</typeparam>
    public void BatchDispatch<T>(params T[] events)
    {
        foreach (T eventPayload in events)
        {
            Dispatch(eventPayload);
        }
    }
}


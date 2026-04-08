using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventDispatcher : Singleton<EventDispatcher>
{
    private readonly Dictionary<Type, List<object>> _eventHandlers = new();

    public void Subscribe<T>(Action<T> callback) where T : struct
    {
        var eventType = typeof(T);

        if (!_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] = new List<object>();
        }

        if (!_eventHandlers[eventType].Contains(callback))
        {
            _eventHandlers[eventType].Add(callback);
        }
    }

    public void Unsubscribe<T>(Action<T> callback) where T : struct
    {
        var eventType = typeof(T);

        if (!_eventHandlers.TryGetValue(eventType, out var handlers)) return;
        handlers.Remove(callback);
        if (handlers.Count == 0)
        {
            _eventHandlers.Remove(eventType);
        }
    }

    public void SendEvent<T>(T eventData) where T : struct
    {
        var eventType = typeof(T);

        if (!_eventHandlers.TryGetValue(eventType, out var eventHandler)) return;
        foreach (var handler in eventHandler.ToList())
        {
            ((Action<T>)handler).Invoke(eventData);
        }
    }

    public void ClearAll()
    {
        _eventHandlers.Clear();
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying) return;
        ClearAll();
    }
}

public static class NewEventDispatcherExtensions
{
    public static void Subscribe<T>(this MonoBehaviour instance, Action<T> callback) where T : struct
    {
        EventDispatcher.Instance.Subscribe<T>(callback);
    }

    public static void Unsubscribe<T>(this MonoBehaviour instance, Action<T> callback) where T : struct
    {
        if (!Application.isPlaying) return;
        if (EventDispatcher.Instance == null) return;
        EventDispatcher.Instance.Unsubscribe<T>(callback);
    }

    public static void SendEvent<T>(this MonoBehaviour instance, T eventData) where T : struct
    {
        EventDispatcher.Instance.SendEvent(eventData);
    }
    public static void SendEvent<T>(this ScriptableObject instance, T eventData) where T : struct
    {
        EventDispatcher.Instance.SendEvent(eventData);
    }
}
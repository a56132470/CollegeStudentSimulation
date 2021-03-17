using System;

namespace Framework.Event
{
    public interface IObservable<T> where T : IEvent
    {
        bool AddObserver<TEvent>(Action<TEvent> observer) where TEvent : T, new();
        bool RemoveObserver<TEvent>(Action<TEvent> observer) where TEvent : T, new();
    }
}
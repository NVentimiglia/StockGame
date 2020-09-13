using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Framework
{
    [Serializable]
    public class Observable<T> : UnityEvent<T>
    {
        public T Value;
        private bool _virgin = true;

        public void Set(T value)
        {
            if (!_virgin && EqualityComparer<T>.Default.Equals(Value, value))
            {
                return;
            }

            _virgin = false;
            Value = value;

            base.Invoke(value);
        }
    }

    [Serializable]
    public class ObservableBool : Observable<bool> { }

    [Serializable]
    public class ObservableInt : Observable<int> { }

    [Serializable]
    public class ObservableString : Observable<string> { }

    [Serializable]
    public class ObservableDateTime : Observable<DateTime> { }

    [Serializable]
    public class ObservableTimeSpan : Observable<TimeSpan> { }

    [Serializable]
    public class ObservableFloat : Observable<float> { }

    [Serializable]
    public class ObservableAction : Observable<Action> { }

    /// <summary>
    ///  Allows for multiple list views to bind to a shared data source
    /// </summary>
    [Serializable]
    public class ObservableStream<T> : Observable<IStream<T>>
    { 
    }

    /// <summary>
    /// Contract for data source for ObservableStream
    /// </summary>
    public interface IStream<T>
    {
        IEnumerable<T> Get();
        void Subscribe(Action<T> handler);
        void Unsubscribe(Action<T> handler);
    }
}
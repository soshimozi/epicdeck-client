using System.Collections.Concurrent;
using System;
using System.Linq;

namespace EpiDeckClient.Services
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly ConcurrentBag<IObserver<T>> _observers;
        private IObserver<T>? _observer;

        public Unsubscriber(ConcurrentBag<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }


        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.TryTake(out _observer);
        }
    }
}
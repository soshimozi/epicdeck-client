using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace EpiDeckClient.Services
{
    public class Producer<T> : IObservable<T>, IDisposable
    {
        private readonly ConcurrentBag<IObserver<T>> _observers;
        private readonly ConcurrentQueue<T> _dataQueue;
        private readonly CancellationTokenSource _cancelTokenSource;

        public Producer()
        {
            _observers = new ConcurrentBag<IObserver<T>>();
            _dataQueue = new ConcurrentQueue<T>();
            _cancelTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (!_cancelTokenSource.IsCancellationRequested)
                {
                    if (_dataQueue.TryDequeue(out var item))
                    {
                        foreach (var observer in _observers)
                        {
                            Task.Run(() => observer.OnNext(item), _cancelTokenSource.Token);
                        }
                    }

                    Thread.Sleep(1);

                    if (!_cancelTokenSource.Token.IsCancellationRequested) continue;


                    foreach (var observer in _observers)
                    {
                        observer.OnCompleted();
                    }

                    break;

                }
            }, _cancelTokenSource.Token);
        }

        public void Publish(T data)
        {
            _dataQueue.Enqueue(data);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<T>(_observers, observer);
        }

        public void EndTransmission()
        {
            _cancelTokenSource.Cancel();
        }

        public void Dispose()
        {
            _cancelTokenSource.Cancel();
            //_cancelTokenSource.Dispose();
        }
    }

}
using System;
using System.Collections.Concurrent;
using EpiDeckClient.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EpiDeckClient.Services
{
    public class ProducerFactory : IProducerFactory, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<IDisposable, byte> _disposables;

        public ProducerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _disposables = new ConcurrentDictionary<IDisposable, byte>();
        }

        public Producer<T> GetProducer<T>()
        {
            var producer = _serviceProvider.GetRequiredService<Producer<T>>();
            _disposables.TryAdd(producer, 0);
            return producer;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables.Keys)
            {
                disposable.Dispose();
            }
        }
    }
}
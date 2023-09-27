using System.Collections.Concurrent;
using System;

namespace EpiDeckClient.Services.Interfaces
{
    public interface IProducerFactory
    {
        public Producer<T> GetProducer<T>();
    }
}
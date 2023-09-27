using System;
using System.Reactive;

namespace EpiDeckClient.Services.Interfaces
{
    public interface IRawHIDDeviceService : IDisposable
    {
        IObservable<byte[]> DataUpdates { get; }
        IObservable<Exception> Errors { get; }
        IObservable<Unit> DeviceInserted { get; }
        IObservable<Unit> DeviceRemoved { get; }

        void StartListening();
        void StopListening();

        bool IsConnected { get; }

        void WriteReport(byte[] data, int length);
    }
    
}
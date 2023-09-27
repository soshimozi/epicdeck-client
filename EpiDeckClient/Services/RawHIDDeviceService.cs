using EpiDeckClient.Services.Interfaces;
using HidLibrary;
using System.Reactive.Subjects;
using System.Threading;
using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;

namespace EpiDeckClient.Services
{

    public class DeviceNotConnectedException : Exception
    {
        public DeviceNotConnectedException() : base() { }
        public DeviceNotConnectedException(string message): base(message) { }
    }

    public class RawHIDDeviceService : IRawHIDDeviceService, IDisposable
    {
        private readonly Subject<byte[]> _dataUpdateSubject = new();
        private readonly Subject<Exception> _errorSubject = new();
        private readonly Subject<Unit> _deviceInserted = new();
        private readonly Subject<Unit> _deviceRemoved = new();

        public IObservable<Unit> DeviceInserted => _deviceInserted.AsObservable();
        public IObservable<Unit> DeviceRemoved => _deviceRemoved.AsObservable();
        public IObservable<byte[]> DataUpdates => _dataUpdateSubject.AsObservable();
        public IObservable<Exception> Errors => _errorSubject.AsObservable();

        private HidDevice? _hidDevice;
        private readonly IHIDDeviceFactory _deviceFactory;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _listening;
        private bool _disposed;

        private readonly ILogger _logger;

        public RawHIDDeviceService(IHIDDeviceFactory deviceFactory, ILogger<RawHIDDeviceService> logger)
        {
            _deviceFactory = deviceFactory;
            _logger = logger;

        }

        public bool IsConnected => _listening;

        public void StartListening()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RawHIDDeviceService));

            if (_listening) return;

            _hidDevice = _deviceFactory.CreateDevice();
            if (_hidDevice == null) 
                throw new DeviceNotConnectedException("Device not found, or not connected.  " +
                                                      "Ensure you are using latest firmware and the device is connected to a USB port.");

            _hidDevice.MonitorDeviceEvents = true;
            _hidDevice.Inserted += OnDeviceInserted;
            _hidDevice.Removed += OnDeviceRemoved;

            _hidDevice.ReadReport(ReadReportCB);

            _listening = true;
            _cancellationTokenSource = new CancellationTokenSource();
            //Task.Run(() => PollHidEndpoint(_cancellationTokenSource.Token));
        }

        public void StopListening()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RawHIDDeviceService));

            if (!_listening) return;
            _cancellationTokenSource.Cancel();

            if (_hidDevice != null)
            {
                _hidDevice.Inserted -= OnDeviceInserted;
                _hidDevice.Removed -= OnDeviceRemoved;
                _hidDevice.MonitorDeviceEvents = false;
            }

            _listening = false;
            _hidDevice?.Dispose();
            _hidDevice = null;
        }

        public void WriteReport(byte[] data, int length)
        {
            if (_hidDevice is not { IsConnected: true }) return;

            try
            {
                var report = new HidReport(length)
                {
                    Data = data
                };

                _hidDevice.WriteReport(report);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to write data to device.", e);
            }
        }

        private void ReadReportCB(HidReport report)
        {
            if (_hidDevice == null || !_hidDevice.IsConnected) return;

            try
            {
                _dataUpdateSubject.OnNext(report.Data);

            }
            catch (Exception e)
            {
                _logger.LogError("Failed to publish data to listener.", e);
            }

            if(_hidDevice != null)
                _hidDevice.ReadReport(ReadReportCB);

        }

        private void OnDeviceRemoved()
        {
            _deviceRemoved.OnNext(Unit.Default);
        }

        private void OnDeviceInserted()
        {
            _deviceInserted.OnNext(Unit.Default);
        }

        //private void PollHidEndpoint(CancellationToken cancellationToken)
        //{
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        var report = _hidDevice?.Read();
        //        if (report?.Status == HidDeviceData.ReadStatus.Success)
        //        {
        //            _dataUpdateSubject.OnNext(report.Data);
        //        }
        //        else
        //        {
        //            // Send the error to the error subject.
        //            _errorSubject.OnNext(new Exception($"Failed to read from HID device. Status: {report?.Status}"));
        //        }

        //        Thread.Sleep(100);
        //    }
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed state (managed objects).
                StopListening();
                _cancellationTokenSource?.Dispose();

                if (_hidDevice != null)
                {
                    _hidDevice.Inserted -= OnDeviceInserted;
                    _hidDevice.Removed -= OnDeviceRemoved;
                }

                _dataUpdateSubject.Dispose();
                _errorSubject.Dispose();
                _hidDevice?.Dispose();

            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}
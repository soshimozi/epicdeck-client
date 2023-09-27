using EpiDeckClient.Framework;
using EpiDeckClient.Framework.Commands;
using EpiDeckClient.Services.Interfaces;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using EpiDeckClient.Services;
using System.Collections.ObjectModel;
using EpiDeckClient.Domain;
using HidLibrary;
using System.Windows.Markup;
using EpiDeckClient.Domain.EventMessages;
using Microsoft.Extensions.Logging;

namespace EpiDeckClient.ViewModels
{

    public class ConnectionViewModel : ViewModelBase, IDisposable
    {
        private readonly IRawHIDDeviceService _deviceService;
        private readonly CompositeDisposable _disposables = new();
        private readonly ILogger _logger;

        private ServerStatusType _serverStatus = ServerStatusType.Disconnected;

        public ObservableCollection<LedViewModel> LedList { get; set; }

        public ConnectionViewModel(IRawHIDDeviceService deviceService, IProducerFactory producerFactory, ILogger<ConnectionViewModel> logger)
        {
            ServerStatus = ServerStatusType.Disconnected;

            _deviceService = deviceService;
            _logger = logger;


            _deviceService.DataUpdates
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(OnDataUpdated)
                .DisposeWith(_disposables);

            // Subscribe to errors.
            _deviceService.Errors
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(OnErrorReceived)
                .DisposeWith(_disposables);

            _deviceService.DeviceInserted
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => OnDeviceInserted())
                .DisposeWith(_disposables);

            _deviceService.DeviceRemoved
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => OnDeviceRemoved())
                .DisposeWith(_disposables);

            ConnectCommand = new DelegateCommand
            {
                CommandAction = (o) =>
                {
                    if (_deviceService.IsConnected)
                    {
                        Disconnect();
                    }
                    else
                    {
                        Connect();
                    }
                }
            };

            LedList = new ObservableCollection<LedViewModel>();
            for (var i = 0; i < 16; i++)
            {
                LedList.Add(new LedViewModel());
            }

            LedList[0].IsOn = true;


            producerFactory.GetProducer<WindowClosing>().Subscribe((a) => { Disconnect(); });

        }

        private void Disconnect()
        {
            try
            {
                _deviceService.WriteReport(new byte[] { (byte)HidEventType.Goodbye }, 1);

                _deviceService.StopListening();
                ServerStatus = ServerStatusType.Disconnected;

            }
            catch (DeviceNotConnectedException e)
            {
                // here we know it's not connected, so tell user
                // to insert device and give them the information they need
            }

            catch (Exception e)
            {
                _logger.LogError("Failed to disconnect from hub device.", e);
            }

        }

        private void Connect()
        {
            try
            {
                _deviceService.StartListening();
                ServerStatus = ServerStatusType.Connected;

            }
            catch (DeviceNotConnectedException e)
            {
                // here we know it's not connected, so tell user
                // to insert device and give them the information they need
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to connect to hub device.", e);
            }
        }

        public string ServerStatusDescription
        {
            get
            {
                switch (ServerStatus)
                {
                    case ServerStatusType.Disconnected: return "Disconnected";
                    case ServerStatusType.Connected: return "Connected";
                    case ServerStatusType.Error: return "Server Error";
                    default: throw new ApplicationException();
                }
            }
        }
        public ServerStatusType ServerStatus
        {
            get => _serverStatus;
            set
            {
                SetProperty(ref _serverStatus, value);
                OnPropertyChanged(nameof(ServerStatusDescription));
            }
        }

        public DelegateCommand ConnectCommand { get; private set; }

        //public bool IsConnected
        //{
        //    get => _deviceConnected;
        //    set => SetProperty(ref _deviceConnected, value);
        //}
        public void Dispose()
        {
            _disposables?.Dispose();
            _deviceService.Dispose();
        }

        private void OnDeviceInserted()
        {
            // send helo
            byte[] data = new byte[] { (byte)HidEventType.Hello };
            _deviceService.WriteReport(data, 1);

        }

        private void OnDeviceRemoved()
        {
            _deviceService.WriteReport(new byte[] {(byte)HidEventType.Goodbye }, 1);
            //_deviceService.StopListening();
        }

        private void OnDataUpdated(byte[] data)
        {
            // Process the data, set properties, etc.
        }

        private void OnErrorReceived(Exception exception)
        {
            // Handle the error, possibly show a message to the user or log the error.
        }

    }
}
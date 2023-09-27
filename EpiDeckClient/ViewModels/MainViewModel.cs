using System;
using EpiDeckClient.Framework;
using EpiDeckClient.Framework.Routing;
using Microsoft.Extensions.Logging;
using System.Windows.Input;
using EpiDeckClient.Domain.EventMessages;
using EpiDeckClient.Framework.Commands;
using EpiDeckClient.Services.Interfaces;
using Prism.Events;
using EpiDeckClient.Services;

namespace EpiDeckClient.ViewModels
{
    public enum ViewType
    {
        Home,
        Palette,
        Color,
        Connection
    }

    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        private string _viewSource;

        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IProducerFactory _producerFactory;

        public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ILogger<MainViewModel> logger, IProducerFactory producerFactory)
        {
            _logger = logger;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator; 
            _producerFactory = producerFactory; 

            eventAggregator.GetEvent<ChangeViewMessage>().Subscribe((v) => SwitchViews(v));

            ConfigureCommands();
            SwitchViews(ViewType.Home);
        }

        private void ConfigureCommands()
        {
            HomeCommand = new RoutableCommand(() => { SwitchViews(ViewType.Home); }, (o) => true);
            PaletteSwitchCommand = new DelegateCommand
            {
                CanExecuteFunc = (o) => true,
                CommandAction = (o) =>
                {
                    SwitchViews(ViewType.Palette);
                }
            };

            ColorToolCommand = new DelegateCommand
            {
                CanExecuteFunc = (o) => true,
                CommandAction = (o) =>
                {
                    SwitchViews(ViewType.Color);
                }
            };

            NavigateBack = new DelegateCommand
            {
                CanExecuteFunc = (o) => _navigationService.CanNavigateBack,
                CommandAction = (o) =>
                {
                    SwitchViews(_navigationService.NavigateBackward(), false);
                }
            };

            NavigateForward = new DelegateCommand
            {
                CanExecuteFunc = (o) => _navigationService.CanNavigateForward,
                CommandAction = (o) =>
                {
                   SwitchViews(_navigationService.NavigateForward(), false);
                }
            };

            WindowClosing = new DelegateCommand
            {
                CommandAction = (o) =>
                {
                    _producerFactory.GetProducer<WindowClosing>().Publish(new WindowClosing());
                   //_eventAggregator.GetEvent<WindowClosing>().Publish(0);
                },
                CanExecuteFunc = (o) => true
            };
        }

        private void SwitchViews(ViewType view, bool navigateTo = true)
        {
            if (navigateTo)
            {
                _navigationService.NavigateTo(view);

                //OnPropertyChanged(nameof(NavigateForward));
                //OnPropertyChanged(nameof(NavigateBack));
            }

            ViewSource = view switch
            {
                ViewType.Home => "/Pages/HomePage.xaml",
                ViewType.Palette => "/Pages/PaletteSwitcher.xaml",
                ViewType.Color => "/Pages/ColorTool.xaml",
                ViewType.Connection => "/Pages/ConnectionView.xaml",
                _ => throw new ArgumentOutOfRangeException(nameof(view), view, null)
            };

        }

        public string ViewSource
        {
            get => _viewSource;
            set => SetProperty(ref _viewSource, value);
        }

        public ICommand HomeCommand { get; private set; }

        public DelegateCommand WindowClosing { get; private set; }
        public DelegateCommand PaletteSwitchCommand { get; private set; }
        public DelegateCommand ColorToolCommand { get; private set; }
        public DelegateCommand NavigateBack { get; private set; }
        public DelegateCommand NavigateForward { get; private set; }


    }
}
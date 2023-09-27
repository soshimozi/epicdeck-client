using System;
using EpiDeckClient.Framework;
using EpiDeckClient.Services.Interfaces;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using EpiDeckClient.Domain.EventMessages;
using EpiDeckClient.Framework.Commands;
using ReactiveUI;
using EpiDeckClient.Services;
using Prism.Events;

namespace EpiDeckClient.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        private readonly IEventAggregator _eventAggregator;
        public HomeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ConnectionCommand = new()

            {
                CommandAction = (o) =>
                {
                    eventAggregator.GetEvent<ChangeViewMessage>().Publish(ViewType.Connection);
                }
            };
        }

        public DelegateCommand ConnectionCommand { get; private set; }


    }
}
using EpiDeckClient.ViewModels;
using Prism.Events;

namespace EpiDeckClient.Domain.EventMessages
{
    public class ChangeViewMessage : PubSubEvent<ViewType>
    {
    }
}
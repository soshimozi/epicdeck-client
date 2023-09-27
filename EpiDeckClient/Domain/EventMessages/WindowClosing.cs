using EpiDeckClient.ViewModels;
using Prism.Events;

namespace EpiDeckClient.Domain.EventMessages
{
    public class WindowClosing : PubSubEvent<int>
    {
        
    }
}
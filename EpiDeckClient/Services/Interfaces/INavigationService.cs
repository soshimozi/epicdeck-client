using EpiDeckClient.ViewModels;

namespace EpiDeckClient.Services.Interfaces
{
    public interface INavigationService
    {
        bool CanNavigateForward { get; }
        bool CanNavigateBack { get; }

        ViewType? CurrentPage { get; }
        ViewType NavigateTo(ViewType page);
        ViewType NavigateForward();
        ViewType NavigateBackward();
    }
}
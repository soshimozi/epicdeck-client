using EpiDeckClient.Services.Interfaces;
using EpiDeckClient.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace EpiDeckClient.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<ViewType> _navigationStack = new Stack<ViewType>();
        private int _currentStackPointer = -1;
        private ViewType? _currentPage = null;

        public ViewType? CurrentPage => _currentPage;
        public bool CanNavigateForward => _currentStackPointer < _navigationStack.Count - 1;
        public bool CanNavigateBack => _currentStackPointer > 0;
        public ViewType NavigateTo(ViewType page)
        {

            // If navigating from a position that's not the top of the stack, truncate the stack
            while ((_currentStackPointer != -1) && _currentStackPointer < _navigationStack.Count - 1)
            {
                _navigationStack.Pop();
            }

            _currentStackPointer++;
            _navigationStack.Push(page);

            _currentPage = page;
            return _currentPage.Value;
        }

        public ViewType NavigateForward()
        {
            if (CanNavigateForward)
            {
                //_currentPage = _navigationStack.ElementAt(_currentStackPointer);
                _currentStackPointer++;
                _currentPage = _navigationStack.ElementAt(_navigationStack.Count - 1 - _currentStackPointer);
            }

            return _currentPage ?? ViewType.Home;
        }

        public ViewType NavigateBackward()
        {
            if (CanNavigateBack)
            {
                _currentStackPointer--;
                //_currentPage = _navigationStack.ElementAt(_currentStackPointer);
                _currentPage = _navigationStack.ElementAt(_navigationStack.Count - 1 - _currentStackPointer);
            }
        

            return _currentPage ?? ViewType.Home;
        }
    }
}
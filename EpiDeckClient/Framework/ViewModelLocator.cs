using CommunityToolkit.Mvvm.DependencyInjection;
using EpiDeckClient.ViewModels;

namespace EpiDeckClient.Framework
{
    public class ViewModelLocator
    {
        public MainViewModel MainWindowViewModel => Ioc.Default.GetService<MainViewModel>();
        public HomeViewModel HomeViewModel => Ioc.Default.GetService<HomeViewModel>();
        public ThemeSettingsViewModel ThemeSettingsViewModel => Ioc.Default.GetService<ThemeSettingsViewModel>();
        public ColorToolViewModel ColorToolViewModel => Ioc.Default.GetService<ColorToolViewModel>();
        public PaletteSwitcherViewModel PaletteSwitcherViewModel => Ioc.Default.GetService<PaletteSwitcherViewModel>();
        public ConnectionViewModel ConnectionViewModel => Ioc.Default.GetService<ConnectionViewModel>();
    }
}
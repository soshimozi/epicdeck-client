using EpiDeckClient.Framework;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System;
using EpiDeckClient.Framework.Commands;
using EpiDeckClient.Framework.Extensions;

namespace EpiDeckClient.ViewModels
{
    public class ColorToolViewModel : ViewModelBase
    {
        private readonly PaletteHelper _paletteHelper = new();

        private ColorScheme _activeScheme;
        public ColorScheme ActiveScheme
        {
            get => _activeScheme;
            set => SetProperty(ref _activeScheme, value);
        }

        private Color? _selectedColor;
        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (!SetProperty(ref _selectedColor, value)) return;

                // if we are triggering a change internally its a hue change and the colors will match
                // so we don't want to trigger a custom color change.
                var currentSchemeColor = ActiveScheme switch
                {
                    ColorScheme.Primary => _primaryColor,
                    ColorScheme.Secondary => _secondaryColor,
                    ColorScheme.PrimaryForeground => _primaryForegroundColor,
                    ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                    _ => throw new NotSupportedException($"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!")
                };

                if (_selectedColor != currentSchemeColor && value is { } color)
                {
                    ChangeCustomColor(color);
                }
            }
        }

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        public ICommand ChangeCustomHueCommand { get; }

        public ICommand ChangeHueCommand { get; }
        public ICommand ChangeToPrimaryCommand { get; }
        public ICommand ChangeToSecondaryCommand { get; }
        public ICommand ChangeToPrimaryForegroundCommand { get; }
        public ICommand ChangeToSecondaryForegroundCommand { get; }

        public ICommand ToggleBaseCommand { get; }

        private void ApplyBase(bool isDark)
        {
            var theme = _paletteHelper.GetTheme();
            theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light);
            _paletteHelper.SetTheme(theme);
        }

        public ColorToolViewModel()
        {
            ToggleBaseCommand = new DelegateCommand { CommandAction = o => ApplyBase((bool)o!) };
            ChangeHueCommand = new DelegateCommand { CommandAction = ChangeHue };
            ChangeCustomHueCommand = new DelegateCommand { CommandAction = ChangeCustomColor };
            ChangeToPrimaryCommand = new DelegateCommand { CommandAction = o => ChangeScheme(ColorScheme.Primary) };
            ChangeToSecondaryCommand = new DelegateCommand { CommandAction = o => ChangeScheme(ColorScheme.Secondary) };
            ChangeToPrimaryForegroundCommand = new DelegateCommand
            { CommandAction = o => ChangeScheme(ColorScheme.PrimaryForeground) };
            ChangeToSecondaryForegroundCommand = new DelegateCommand
            { CommandAction = o => ChangeScheme(ColorScheme.SecondaryForeground) };


            var theme = _paletteHelper.GetTheme();

            _primaryColor = theme.PrimaryMid.Color;
            _secondaryColor = theme.SecondaryMid.Color;

            SelectedColor = _primaryColor;
        }

        private void ChangeCustomColor(object? obj)
        {
            var color = (Color)obj!;

            switch (ActiveScheme)
            {
                case ColorScheme.Primary:
                    _paletteHelper.ChangePrimaryColor(color);
                    _primaryColor = color;
                    break;
                case ColorScheme.Secondary:
                    _paletteHelper.ChangeSecondaryColor(color);
                    _secondaryColor = color;
                    break;
                case ColorScheme.PrimaryForeground:
                    SetPrimaryForegroundToSingleColor(color);
                    _primaryForegroundColor = color;
                    break;
                case ColorScheme.SecondaryForeground:
                    SetSecondaryForegroundToSingleColor(color);
                    _secondaryForegroundColor = color;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeScheme(ColorScheme scheme)
        {
            ActiveScheme = scheme;
            SelectedColor = ActiveScheme switch
            {
                ColorScheme.Primary => _primaryColor,
                ColorScheme.Secondary => _secondaryColor,
                ColorScheme.PrimaryForeground => _primaryForegroundColor,
                ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                _ => SelectedColor
            };
        }

        private Color? _primaryColor;

        private Color? _secondaryColor;

        private Color? _primaryForegroundColor;

        private Color? _secondaryForegroundColor;

        private void ChangeHue(object? obj)
        {
            var hue = (Color)obj!;

            SelectedColor = hue;
            switch (ActiveScheme)
            {
                case ColorScheme.Primary:
                    _paletteHelper.ChangePrimaryColor(hue);
                    _primaryColor = hue;
                    _primaryForegroundColor = _paletteHelper.GetTheme().PrimaryMid.GetForegroundColor();
                    break;
                case ColorScheme.Secondary:
                    _paletteHelper.ChangeSecondaryColor(hue);
                    _secondaryColor = hue;
                    _secondaryForegroundColor = _paletteHelper.GetTheme().SecondaryMid.GetForegroundColor();
                    break;
                case ColorScheme.PrimaryForeground:
                    SetPrimaryForegroundToSingleColor(hue);
                    _primaryForegroundColor = hue;
                    break;
                case ColorScheme.SecondaryForeground:
                    SetSecondaryForegroundToSingleColor(hue);
                    _secondaryForegroundColor = hue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetPrimaryForegroundToSingleColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(theme.PrimaryLight.Color, color);
            theme.PrimaryMid = new ColorPair(theme.PrimaryMid.Color, color);
            theme.PrimaryDark = new ColorPair(theme.PrimaryDark.Color, color);

            _paletteHelper.SetTheme(theme);
        }

        private void SetSecondaryForegroundToSingleColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(theme.SecondaryLight.Color, color);
            theme.SecondaryMid = new ColorPair(theme.SecondaryMid.Color, color);
            theme.SecondaryDark = new ColorPair(theme.SecondaryDark.Color, color);

            _paletteHelper.SetTheme(theme);
        }

    }
}
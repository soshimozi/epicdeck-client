using EpiDeckClient.Framework;
using EpiDeckClient.Framework.Commands;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows.Input;
using System;

namespace EpiDeckClient.ViewModels
{
    public class PaletteSwitcherViewModel : ViewModelBase
    {
        public PaletteSwitcherViewModel()
        {

            Swatches = new SwatchesProvider().Swatches;

        }

        public IEnumerable<Swatch> Swatches
        {
            get;
        }

        public ICommand ApplyPrimaryCommand => new DelegateCommand { CommandAction = o => ApplyPrimary((Swatch)o!) };


        private static void ApplyPrimary(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));
        }

        public ICommand ApplyAccentCommand
        {
            get;
        } = new DelegateCommand { CommandAction = o => ApplyAccent((Swatch)o!) };

        private static void ApplyAccent(Swatch swatch)
        {
            if (swatch is { AccentExemplarHue: not null })
            {
                ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));
            }
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
using System.Configuration;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using EpiDeckClient.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using NetEscapades.Extensions.Logging.RollingFile;
using System.IO;
using System;
using System.Windows.Media;
using EpiDeckClient.Configuration;
using EpiDeckClient.Framework.Extensions;
using EpiDeckClient.Services;
using EpiDeckClient.Services.Interfaces;
using Prism.Events;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;

namespace EpiDeckClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
#if DEBUG
                .AddJsonFile("appsettings.Development.json", true, true)
#else
            .AddJsonFile("appsettings.Production.json", true, true)
#endif
                .Build();


            var userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var logDirectory = Path.Combine(userDirectory, "EpiDeck", "Logs");
            Directory.CreateDirectory(logDirectory);

            var deviceConfiguration = configuration.GetSection("DeviceSettings").Get<DeviceSettingsConfiguration>();

            var serviceCollection = new ServiceCollection()
                .AddSingleton<IBinarySerializationService, BinarySerializationService>()
                .AddSingleton<IHIDDeviceFactory, HidDeviceFactory>()
                .AddSingleton<IRawHIDDeviceService, RawHIDDeviceService>()
                .AddSingleton<IEventAggregator, EventAggregator>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IProducerFactory, ProducerFactory>()
                .AddViewModels<ViewModelBase>()
                .AddProducers()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                    loggingBuilder.AddFile(options =>
                    {
                        options.FileSizeLimit = 1024 * 1024 * 250; // 250MB max
                        options.Periodicity = PeriodicityOptions.Daily;
                        options.RetainedFileCountLimit = null;
                        options.LogDirectory = logDirectory;
                    });
                    loggingBuilder.AddConsole();

                });

            if (deviceConfiguration != null)
            {
                serviceCollection.AddSingleton(deviceConfiguration);
            }

            Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());


        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#FF9E5B43"));
            paletteHelper.SetTheme(theme);

        }
    }
}

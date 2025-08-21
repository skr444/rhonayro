using System;
using System.IO;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using RhonAyro.Client.Desktop.Ui.Navigation;
using RhonAyro.Client.Desktop.Ui.ViewModels;
using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Runtime.Implementation;
using RhonAyro.Infrastructure.Storage.Api;
using RhonAyro.Infrastructure.Storage.File;

namespace RhonAyro.Client.Desktop.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string AppName = "RhonAyro";
        private readonly string storageDirectory;

        public App()
        {
            InitializeComponent();

            storageDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppName);
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }

            var lifecycleManager = new LifecycleManager(Current.Shutdown);

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<TabHostViewModel>()
                .AddSingleton<ILifecycleManager, LifecycleManager>(_ => lifecycleManager)
                .AddSingleton<IFileStorage, FileStorage>(_ => new FileStorage(storageDirectory, lifecycleManager))

                .AddTransient<PreparationViewModel>()
                .AddTransient<StartListViewModel>()
                .AddTransient<CompetitionViewModel>()
                .AddTransient<ScoreboardViewModel>()

                .AddTransient<MainWindow>()

                .BuildServiceProvider());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var window = Ioc.Default.GetService<MainWindow>()!;
            Current.MainWindow = window;
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Ioc.Default.GetService<ILifecycleManager>()?.Shutdown();
            base.OnExit(e);
        }
    }
}

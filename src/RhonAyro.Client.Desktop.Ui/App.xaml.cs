using System;
using System.IO;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

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
            if (Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .BuildServiceProvider());

            base.OnStartup(e);
        }
    }
}

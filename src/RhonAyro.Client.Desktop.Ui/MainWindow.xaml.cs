using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using RhonAyro.Client.Desktop.Ui.Navigation;
using RhonAyro.Client.Desktop.Ui.ViewModels;

namespace RhonAyro.Client.Desktop.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetService<TabHostViewModel>();

            Loaded += (_, __) =>
            {
                var nav = Ioc.Default.GetService<INavigationService>()!;
                nav.OpenOrActivate<PreparationViewModel>("Preparation", "prep");
            };
        }
    }
}

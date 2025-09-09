using System.ComponentModel;
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

            if (DataContext is BaseViewModel vm)
            {
                vm.CloseRequested += (_, _) => Close();
            }

            Closing += (object? _, CancelEventArgs e) =>
            {
                if (DataContext is BaseViewModel vm)
                {
                    e.Cancel = !vm.ProcessCloseRequest();
                }
            };

            Loaded += (_, __) =>
            {
                var nav = Ioc.Default.GetService<INavigationService>()!;
                nav.OpenOrActivate<ClubMemberManagementViewModel>("Members", "members");
                nav.OpenOrActivate<PreparationViewModel>("Preparation", "preparation");
                nav.OpenOrActivate<StartListViewModel>("Start List", "startlist");
                nav.OpenOrActivate<CompetitionViewModel>("Competition", "competition");
                nav.OpenOrActivate<ClubMemberManagementViewModel>("Members", "members");
            };
        }
    }
}

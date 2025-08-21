using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Client.Desktop.Ui.Navigation;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal partial class TabHostViewModel : ObservableObject
    {
        /// <summary>
        /// Gets the open tabs.
        /// </summary>
        public ObservableCollection<TabItemHost> Tabs { get; }

        private readonly INavigationService navigation;

        [ObservableProperty]
        private TabItemHost? selectedTab;

        public TabHostViewModel(INavigationService nav)
        {
            navigation = nav;
            Tabs = [];
        }

        [RelayCommand]
        private void CloseTab(TabItemHost? tab)
        {
            if (tab is null)
            {
                return;
            }

            Tabs.Remove(tab);

            if ((Tabs.Count > 0) && (SelectedTab is null))
            {
                SelectedTab = Tabs[^1]; // fallback selection
            }
        }

        // Convenience commands to open key tabs again (e.g., via menu/toolbar)
        [RelayCommand]
        private void OpenPreparation() => navigation.OpenOrActivate<PreparationViewModel>("Preparation", "prep");
        [RelayCommand]
        private void OpenStartList() => navigation.OpenOrActivate<StartListViewModel>("Start List", "startlist");
        [RelayCommand]
        private void OpenCompetition() => navigation.OpenOrActivate<CompetitionViewModel>("Competition", "competition");
        [RelayCommand]
        private void OpenScoreboard() => navigation.OpenOrActivate<ScoreboardViewModel>("Scoreboard", "scoreboard");
    }
}

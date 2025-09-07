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

        [ObservableProperty]
        private TabItemHost? selectedTab;

        public TabHostViewModel()
        {
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
    }
}

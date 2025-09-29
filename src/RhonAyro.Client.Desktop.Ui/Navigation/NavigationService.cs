using System;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Win32;

using RhonAyro.Client.Desktop.Ui.ViewModels;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    /// <inheritdoc/>
    internal sealed class NavigationService : INavigationService
    {
        /// <summary>
        /// Creates a new instance of <see cref="NavigationService"/>.
        /// </summary>
        public NavigationService()
        {
        }

        /// <inheritdoc/>
        public void OpenOrActivate<TVm>(string header, string? key = null, Func<TVm>? factory = null) where TVm : ObservableObject
        {
            var tabHost = Ioc.Default.GetService<TabHostViewModel>()!;

            // Try activate existing by key first
            if (!string.IsNullOrWhiteSpace(key))
            {
                var existing = tabHost.Tabs.FirstOrDefault(t => t.Key == key);
                if (existing is not null)
                {
                    tabHost.SelectedTab = existing;
                    return;
                }
            }

            // Or prevent duplicates by type if no key is provided (policy choice)
            if (string.IsNullOrWhiteSpace(key))
            {
                var existingByType = tabHost.Tabs.FirstOrDefault(t => t.ViewModel.GetType() == typeof(TVm));
                if (existingByType is not null)
                {
                    tabHost.SelectedTab = existingByType;
                    return;
                }
            }

            var vm = factory?.Invoke() ?? Ioc.Default.GetService<TVm>()!;
            var tab = new TabItemHost(header, vm, key);
            tabHost.Tabs.Add(tab);
            tabHost.SelectedTab = tab;
        }

        /// <inheritdoc/>
        public void CloseByKey(string key)
        {
            var tabHost = Ioc.Default.GetService<TabHostViewModel>()!;

            var tab = tabHost.Tabs.FirstOrDefault(t => t.Key == key);
            if (tab != null)
            {
                tabHost.Tabs.Remove(tab);
            }
        }

        /// <inheritdoc/>
        public void Close(object viewModel)
        {
            var tabHost = Ioc.Default.GetService<TabHostViewModel>()!;

            var tab = tabHost.Tabs.FirstOrDefault(t => ReferenceEquals(t.ViewModel, viewModel));
            if (tab != null)
            {
                tabHost.Tabs.Remove(tab);
            }
        }

        /// <inheritdoc/>
        public void OpenFileDialog()
        {
            var ofd = new OpenFileDialog
            {
                AddExtension = false,
                AddToRecent = false,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultDirectory = "",
                DefaultExt = "",
                DereferenceLinks = true,
                Filter = "",
                FilterIndex = 1,
                ForcePreviewPane = false,
                InitialDirectory = "",
                Multiselect = false,
                ReadOnlyChecked = false,
                RootDirectory = "",
                ShowHiddenItems = false,
                ShowReadOnly = false,
                Title = "",
                ValidateNames = false
            };
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    /// <summary>
    /// Represents a tab on the main view.
    /// </summary>
    internal sealed class TabItemHost
    {
        /// <summary>
        /// Gets the display title in the tab header.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// Gets the actual VM that DataTemplates will map to a View.
        /// </summary>
        public ObservableObject ViewModel { get; }

        /// <summary>
        /// Gets the optional uniqueness key (e.g., "Competition:Sprung:Session1").
        /// </summary>
        public string? Key { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TabItemHost"/>.
        /// </summary>
        /// <param name="header">Display title.</param>
        /// <param name="viewModel">The viewmodel for the view of this tab.</param>
        /// <param name="key">Uniqueness key.</param>
        public TabItemHost(string header, ObservableObject viewModel, string? key = null)
        {
            Header = header;
            ViewModel = viewModel;
            Key = key;
        }
    }
}

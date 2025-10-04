using System;

using CommunityToolkit.Mvvm.ComponentModel;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    /// <summary>
    /// Provides means to navigate between different UIs.
    /// </summary>
    internal interface INavigationService
    {
        /// <summary>
        /// Opens or activates a tab for the given ViewModel type.
        /// </summary>
        /// <typeparam name="TVm">Type of the requested viewmodel.</typeparam>
        /// <param name="header">Text on the tab.</param>
        /// <param name="key">Unique identifier.</param>
        /// <param name="factory">Viewmodel factory or <see langword="null"/> for DI resolution.</param>
        void OpenOrActivate<TVm>(string header, string? key = null, Func<TVm>? factory = null) where TVm : ObservableObject;

        /// <summary>
        /// Closes a view by viewmodel identifier.
        /// </summary>
        /// <param name="key">Unique identifier.</param>
        void CloseByKey(string key);

        /// <summary>
        /// Closes the associated with the <paramref name="viewModel"/>.
        /// </summary>
        /// <param name="viewModel">View to close.</param>
        void Close(object viewModel);

        /// <summary>
        /// Creates a new instance of <see cref="IOpenFileDialog"/>.
        /// </summary>
        /// <returns>A new ready to use dialog.</returns>
        IOpenFileDialog NewOfd();

        MessageBoxResult ShowMessageBox(string message, string? caption = null, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.Cancel, object? owner = null, MessageBoxOptions messageBoxOptions = MessageBoxOptions.None);
    }
}

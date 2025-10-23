using System;
using System.Linq;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

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
        public void OpenOrActivate<TVm>(string header, string? key = null, Func<TVm>? factory = null)
            where TVm : ObservableObject
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
        public IOpenFileDialog NewOfd()
        {
            return new MyOfd();
        }

        public MessageBoxResult ShowMessageBox(string message, string? caption = null, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.Cancel, object? owner = null, MessageBoxOptions messageBoxOptions = MessageBoxOptions.None)
        {
            System.Windows.MessageBoxResult dialogResult = System.Windows.MessageBoxResult.None;
            if (owner is Window window)
            {
                dialogResult = MessageBox.Show(window, message, caption, ConvertMessageBoxButton(messageBoxButton), ConvertMessageBoxImage(messageBoxImage), ConvertMessageBoxResult(defaultResult), ConvertMessageBoxOptions(messageBoxOptions));
            }
            else
            {
                dialogResult = MessageBox.Show(message, caption, ConvertMessageBoxButton(messageBoxButton), ConvertMessageBoxImage(messageBoxImage), ConvertMessageBoxResult(defaultResult), ConvertMessageBoxOptions(messageBoxOptions));
            }
            return ConvertMessageBoxResult(dialogResult);
        }

        private static System.Windows.MessageBoxButton ConvertMessageBoxButton(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    return System.Windows.MessageBoxButton.OK;

                case MessageBoxButton.OKCancel:
                    return System.Windows.MessageBoxButton.OKCancel;

                case MessageBoxButton.YesNo:
                    return System.Windows.MessageBoxButton.YesNo;

                case MessageBoxButton.YesNoCancel:
                    return System.Windows.MessageBoxButton.YesNoCancel;

                default:
                    throw new ArgumentException($"Unsupported {nameof(MessageBoxButton)} value '{button}({(int)button})'");
            }
        }

        private static System.Windows.MessageBoxImage ConvertMessageBoxImage(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.None:
                    return System.Windows.MessageBoxImage.None;

                case MessageBoxImage.Asterisk:
                    return System.Windows.MessageBoxImage.Asterisk;

                case MessageBoxImage.Error:
                    return System.Windows.MessageBoxImage.Error;

                case MessageBoxImage.Exclamation:
                    return System.Windows.MessageBoxImage.Exclamation;

                case MessageBoxImage.Hand:
                    return System.Windows.MessageBoxImage.Hand;

                case MessageBoxImage.Information:
                    return System.Windows.MessageBoxImage.Information;

                case MessageBoxImage.Stop:
                    return System.Windows.MessageBoxImage.Stop;

                case MessageBoxImage.Warning:
                    return System.Windows.MessageBoxImage.Warning;

                default:
                    throw new ArgumentException($"Unsupported {nameof(MessageBoxImage)} value '{image}({(int)image})'");
            }
        }

        private static System.Windows.MessageBoxResult ConvertMessageBoxResult(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.None:
                    return System.Windows.MessageBoxResult.None;

                case MessageBoxResult.OK:
                    return System.Windows.MessageBoxResult.OK;

                case MessageBoxResult.Cancel:
                    return System.Windows.MessageBoxResult.Cancel;

                case MessageBoxResult.Yes:
                    return System.Windows.MessageBoxResult.Yes;

                case MessageBoxResult.No:
                    return System.Windows.MessageBoxResult.No;

                default:
                    throw new ArgumentException($"Unsupported {nameof(MessageBoxResult)} value '{result}({(int)result})'");
            }
        }

        private static MessageBoxResult ConvertMessageBoxResult(System.Windows.MessageBoxResult result)
        {
            switch (result)
            {
                case System.Windows.MessageBoxResult.None:
                    return MessageBoxResult.None;

                case System.Windows.MessageBoxResult.OK:
                    return MessageBoxResult.OK;

                case System.Windows.MessageBoxResult.Cancel:
                    return MessageBoxResult.Cancel;

                case System.Windows.MessageBoxResult.Yes:
                    return MessageBoxResult.Yes;

                case System.Windows.MessageBoxResult.No:
                    return MessageBoxResult.No;

                default:
                    throw new ArgumentException($"Unsupported {nameof(System.Windows.MessageBoxResult)} value '{result}({(int)result})'");
            }
        }

        private static System.Windows.MessageBoxOptions ConvertMessageBoxOptions(MessageBoxOptions options)
        {
            switch (options)
            {
                case MessageBoxOptions.None:
                    return System.Windows.MessageBoxOptions.None;

                case MessageBoxOptions.DefaultDesktopOnly:
                    return System.Windows.MessageBoxOptions.DefaultDesktopOnly;

                case MessageBoxOptions.RightAlign:
                    return System.Windows.MessageBoxOptions.RightAlign;

                case MessageBoxOptions.RtlReading:
                    return System.Windows.MessageBoxOptions.RtlReading;

                case MessageBoxOptions.ServiceNotification:
                    return System.Windows.MessageBoxOptions.ServiceNotification;

                default:
                    throw new ArgumentException($"Unsupported {nameof(MessageBoxOptions)} value '{options}({(int)options})'");
            }
        }
    }
}

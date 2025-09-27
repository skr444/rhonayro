using System.Windows;
using System.Windows.Input;

namespace RhonAyro.Client.Desktop.Ui.Behaviors
{
    internal static class FrameworkElementBehaviors
    {
        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached(
                "LoadedCommand",
                typeof(ICommand),
                typeof(FrameworkElementBehaviors),
                new PropertyMetadata(null, OnLoadedCommandChanged));

        public static void SetLoadedCommand(DependencyObject d, ICommand value) =>
            d.SetValue(LoadedCommandProperty, value);

        public static ICommand? GetLoadedCommand(DependencyObject d) =>
            (ICommand?)d.GetValue(LoadedCommandProperty);

        private static void OnLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement fe)
            {
                return;
            }

            // Unhook previous
            fe.Loaded -= OnLoaded;

            if (e.NewValue is ICommand)
            {
                fe.Loaded += OnLoaded;
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement fe)
            {
                return;
            }

            var cmd = GetLoadedCommand(fe);
            if (cmd?.CanExecute(null) == true)
            {
                cmd.Execute(null);
            }

            // Optional: run only once per element
            fe.Loaded -= OnLoaded;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace RhonAyro.Client.Desktop.Ui.Behaviors
{
    internal static class DataGridBehaviors
    {
        public static readonly DependencyProperty RowEditEndingCommandProperty =
            DependencyProperty.RegisterAttached(
                "RowEditEndingCommand",
                typeof(ICommand),
                typeof(DataGridBehaviors),
                new PropertyMetadata(null, OnRowEditEndingCommandChanged));

        public static void SetRowEditEndingCommand(DependencyObject d, ICommand value) =>
            d.SetValue(RowEditEndingCommandProperty, value);

        public static ICommand GetRowEditEndingCommand(DependencyObject d) =>
            (ICommand)d.GetValue(RowEditEndingCommandProperty);

        private static void OnRowEditEndingCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid dg)
            {
                return;
            }

            if (e.NewValue is ICommand)
            {
                dg.RowEditEnding += DataGridRowEditEnding;
            }
            else
            {
                dg.RowEditEnding -= DataGridRowEditEnding;
            }
        }

        private static void DataGridRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
        {
            if (sender is not DataGrid dg)
            {
                return;
            }

            // Only act on commit (not cancel)
            if (e.EditAction != DataGridEditAction.Commit)
            {
                return;
            }

            var item = e.Row?.Item;

            // Defer execution until after the edit pipeline completes
            dg.Dispatcher.BeginInvoke(new Action(() =>
            {
                var cmd = GetRowEditEndingCommand(dg);
                if (cmd?.CanExecute(item) == true)
                    cmd.Execute(item);
            }), DispatcherPriority.Background);
        }
    }
}

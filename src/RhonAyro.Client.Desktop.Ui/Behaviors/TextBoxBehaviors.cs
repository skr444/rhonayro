using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RhonAyro.Client.Desktop.Ui.Behaviors
{
    internal static class TextBoxBehaviors
    {
        public static readonly DependencyProperty DigitsOnlyProperty =
            DependencyProperty.RegisterAttached(
                "DigitsOnly",
                typeof(bool),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(false, OnDigitsOnlyChanged));

        public static void SetDigitsOnly(DependencyObject element, bool value) =>
            element.SetValue(DigitsOnlyProperty, value);

        public static bool GetDigitsOnly(DependencyObject element) =>
            (bool)element.GetValue(DigitsOnlyProperty);

        private static void OnDigitsOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox tb) return;

            if ((bool)e.NewValue)
            {
                DataObject.AddPastingHandler(tb, OnPaste);
                tb.PreviewTextInput += OnPreviewTextInput;
                tb.PreviewKeyDown += OnPreviewKeyDown;
                tb.PreviewDragOver += OnPreviewDragOver;
                tb.Drop += OnDrop;

                // Optional: Disable IME to avoid non-digit compositions
                InputMethod.SetIsInputMethodEnabled(tb, false);
            }
            else
            {
                DataObject.RemovePastingHandler(tb, OnPaste);
                tb.PreviewTextInput -= OnPreviewTextInput;
                tb.PreviewKeyDown -= OnPreviewKeyDown;
                tb.PreviewDragOver -= OnPreviewDragOver;
                tb.Drop -= OnDrop;

                InputMethod.SetIsInputMethodEnabled(tb, true);
            }
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsAllDigits(e.Text);
        }

        private static void OnPreviewKeyDown(object? sender, KeyEventArgs e)
        {
            // Block space; allow editing/navigation keys
            if (e.Key == Key.Space)
                e.Handled = true;
            // All other keys (Backspace, Delete, Tab, arrows, Home/End) pass through
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            var text = e.SourceDataObject.GetData(DataFormats.Text) as string ?? string.Empty;
            if (!IsAllDigits(text))
                e.CancelCommand(); // silently ignore
        }

        private static void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            var text = e.Data.GetData(DataFormats.Text) as string ?? string.Empty;
            if (!IsAllDigits(text))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private static void OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Handled = true;
                return;
            }

            var text = e.Data.GetData(DataFormats.Text) as string ?? string.Empty;
            if (!IsAllDigits(text))
                e.Handled = true; // ignore drop
        }

        private static bool IsAllDigits(string s) => s.All(char.IsDigit);
    }
}

using System;
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
            if (d is not TextBox tb)
            {
                return;
            }

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
            {
                e.Handled = true;
            }
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
            {
                e.CancelCommand(); // silently ignore
            }
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
            {
                e.Handled = true; // ignore drop
            }
        }

        private static bool IsAllDigits(string s) => s.All(char.IsDigit);

        // --- New attached properties for numeric input (decimals/negatives) ---

        public static readonly DependencyProperty NumericInputProperty =
            DependencyProperty.RegisterAttached(
                "NumericInput",
                typeof(bool),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(false, OnNumericInputChanged));

        public static void SetNumericInput(DependencyObject element, bool value) =>
            element.SetValue(NumericInputProperty, value);

        public static bool GetNumericInput(DependencyObject element) =>
            (bool)element.GetValue(NumericInputProperty);

        public static readonly DependencyProperty AllowDecimalProperty =
            DependencyProperty.RegisterAttached(
                "AllowDecimal",
                typeof(bool),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(true)); // default: allow decimals

        public static void SetAllowDecimal(DependencyObject element, bool value) =>
            element.SetValue(AllowDecimalProperty, value);

        public static bool GetAllowDecimal(DependencyObject element) =>
            (bool)element.GetValue(AllowDecimalProperty);

        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.RegisterAttached(
                "AllowNegative",
                typeof(bool),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(false)); // default: no negatives

        public static void SetAllowNegative(DependencyObject element, bool value) =>
            element.SetValue(AllowNegativeProperty, value);

        public static bool GetAllowNegative(DependencyObject element) =>
            (bool)element.GetValue(AllowNegativeProperty);

        public static readonly DependencyProperty MaxDecimalPlacesProperty =
            DependencyProperty.RegisterAttached(
                "MaxDecimalPlaces",
                typeof(int),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(-1)); // -1 => no limit

        public static void SetMaxDecimalPlaces(DependencyObject element, int value) =>
            element.SetValue(MaxDecimalPlacesProperty, value);

        public static int GetMaxDecimalPlaces(DependencyObject element) =>
            (int)element.GetValue(MaxDecimalPlacesProperty);

        // Hook/unhook like DigitsOnly, but with numeric validation.
        private static void OnNumericInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox tb) return;

            if ((bool)e.NewValue)
            {
                DataObject.AddPastingHandler(tb, OnNumericPaste);
                tb.PreviewTextInput += OnNumericPreviewTextInput;
                tb.PreviewKeyDown += OnNumericPreviewKeyDown;
                tb.PreviewDragOver += OnNumericPreviewDragOver;
                tb.Drop += OnNumericDrop;

                // Optional: Disable IME to avoid composition of non-numeric
                InputMethod.SetIsInputMethodEnabled(tb, false);
            }
            else
            {
                DataObject.RemovePastingHandler(tb, OnNumericPaste);
                tb.PreviewTextInput -= OnNumericPreviewTextInput;
                tb.PreviewKeyDown -= OnNumericPreviewKeyDown;
                tb.PreviewDragOver -= OnNumericPreviewDragOver;
                tb.Drop -= OnNumericDrop;

                InputMethod.SetIsInputMethodEnabled(tb, true);
            }
        }

        private static void OnNumericPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb) return;
            var proposed = BuildProposedText(tb, e.Text);
            e.Handled = !IsValidNumeric(proposed, tb);
        }

        private static void OnNumericPreviewKeyDown(object? sender, KeyEventArgs e)
        {
            // Block space like in DigitsOnly; allow standard editing/navigation keys
            if (e.Key == Key.Space) e.Handled = true;
        }

        private static void OnNumericPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }
            var pasteText = e.SourceDataObject.GetData(DataFormats.Text) as string ?? string.Empty;
            var proposed = BuildProposedText(tb, pasteText);
            if (!IsValidNumeric(proposed, tb))
                e.CancelCommand();
        }

        private static void OnNumericPreviewDragOver(object sender, DragEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (!e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            var text = e.Data.GetData(DataFormats.Text) as string ?? string.Empty;
            var proposed = BuildProposedText(tb, text);
            if (!IsValidNumeric(proposed, tb))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private static void OnNumericDrop(object sender, DragEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (!e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Handled = true;
                return;
            }
            var text = e.Data.GetData(DataFormats.Text) as string ?? string.Empty;
            var proposed = BuildProposedText(tb, text);
            if (!IsValidNumeric(proposed, tb))
                e.Handled = true;
        }

        // --- helpers ---

        private static string BuildProposedText(TextBox tb, string incoming)
        {
            var start = tb.SelectionStart;
            var length = tb.SelectionLength;
            var current = tb.Text ?? string.Empty;

            // Replace the current selection with incoming
            var before = start > 0 ? current.Substring(0, start) : string.Empty;
            var after = (start + length) < current.Length ? current.Substring(start + length) : string.Empty;
            return before + incoming + after;
        }

        private static bool IsValidNumeric(string s, TextBox tb)
        {
            var allowDecimal = GetAllowDecimal(tb);
            var allowNegative = GetAllowNegative(tb);
            var maxDecimals = GetMaxDecimalPlaces(tb);

            const char dec = '.';     // enforce US-style decimal point
            const char sign = '-';

            if (string.IsNullOrWhiteSpace(s)) return true;

            s = s.Trim();

            // Quick scan
            foreach (var ch in s)
            {
                if (char.IsDigit(ch)) continue;
                if (allowDecimal && ch == dec) continue;
                if (allowNegative && ch == sign) continue;
                return false;
            }

            // Only one sign, and leading
            if (s.Count(c => c == sign) > 1) return false;
            if (s.Contains(sign) && !s.StartsWith(sign)) return false;

            // Only one decimal point if allowed
            if (!allowDecimal && s.Contains(dec)) return false;
            if (allowDecimal && s.Count(c => c == dec) > 1) return false;

            // Limit fractional digits
            var core = s.TrimStart(sign);
            if (allowDecimal && core.Contains(dec))
            {
                var parts = core.Split(dec);
                if (maxDecimals >= 0 && parts.Length == 2 && parts[1].Length > maxDecimals)
                    return false;
            }

            // Final strict parse with InvariantCulture
            var styles = System.Globalization.NumberStyles.AllowLeadingSign |
                         System.Globalization.NumberStyles.AllowDecimalPoint;

            return double.TryParse(s,
                styles,
                System.Globalization.CultureInfo.InvariantCulture,
                out _);
        }
    }
}

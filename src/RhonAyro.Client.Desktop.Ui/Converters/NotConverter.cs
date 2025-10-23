using System;
using System.Globalization;
using System.Windows.Data;

namespace RhonAyro.Client.Desktop.Ui.Converters
{
    internal sealed class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return !b;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return !b;
            }

            return Binding.DoNothing;
        }
    }
}

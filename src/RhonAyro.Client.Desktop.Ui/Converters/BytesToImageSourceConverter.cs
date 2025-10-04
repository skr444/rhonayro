using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RhonAyro.Client.Desktop.Ui.Converters
{
    internal class BytesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not byte[] bytes || bytes.Length == 0)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                using var stream = new MemoryStream(bytes);
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                //image.DecodePixelHeight = 512; // optional for performance boost
                //image.DecodePixelWidth = 512;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze(); // make cross-thread safe
                return image;
            }
            catch
            {
                return DependencyProperty.UnsetValue; // or a placeholder ImageSource from resources
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If you need ImageSource -> bytes (e.g., after edits), encode it:
            if (value is BitmapSource src)
            {
                var encoder = new PngBitmapEncoder(); // or JpegBitmapEncoder, etc.
                encoder.Frames.Add(BitmapFrame.Create(src));
                using var ms = new MemoryStream();
                encoder.Save(ms);
                return ms.ToArray();
            }
            return Binding.DoNothing;
        }
    }
}

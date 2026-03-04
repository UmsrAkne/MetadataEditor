using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MetadataEditor.Converters
{
    public class MetadataColorConverter : IValueConverter
    {
        private readonly static Brush[] Palette =
        {
            Brushes.Crimson,
            Brushes.OrangeRed,
            Brushes.DarkOrange,
            Brushes.Goldenrod,
            Brushes.ForestGreen,
            Brushes.Teal,
            Brushes.SteelBlue,
            Brushes.MediumSlateBlue,
            Brushes.MediumVioletRed,
            Brushes.DeepPink,
            Brushes.HotPink,
            Brushes.Orange,
            Brushes.DarkGoldenrod,
            Brushes.OliveDrab,
            Brushes.SeaGreen,
            Brushes.DodgerBlue,
            Brushes.CornflowerBlue,
            Brushes.BlueViolet,
            Brushes.Indigo,
            Brushes.DarkSeaGreen,
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string text || string.IsNullOrEmpty(text))
            {
                return Brushes.Gray;
            }

            var hash = text.GetHashCode();
            hash = Math.Abs(hash);

            var index = hash % Palette.Length;

            return Palette[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
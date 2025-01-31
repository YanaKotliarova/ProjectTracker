using System.Globalization;
using System.Windows.Data;

namespace ProjectTracker.MVVM.View.UIHelpers
{
    public class PriorityToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(Properties.Resources.LowPriority)) return "\xF0AE";
            else if (value.Equals(Properties.Resources.HighPriority)) return "\xF0AD";
            else return "\xE8CB";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

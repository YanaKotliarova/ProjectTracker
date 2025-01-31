using System.Globalization;
using System.Windows.Data;

namespace ProjectTracker.MVVM.View.UIHelpers
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(Properties.Resources.LowPriority)) return "Green";
            else if (value.Equals(Properties.Resources.HighPriority)) return "Red";
            else return "Orange";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

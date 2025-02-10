using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectTracker.MVVM.View.UIHelpers
{
    public class ProjectIdToNameConverter : IValueConverter
    {
        private readonly IWorkWithProjectService _workWithProject = App.GetServiceProvider().GetService<IWorkWithProjectService>()!;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return _workWithProject.GetProjectName((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

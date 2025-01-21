using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.MVVM.ViewModel;
using System.Windows.Controls;

namespace ProjectTracker.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for AddItemUserControl.xaml
    /// </summary>
    public partial class AddItemUserControl : UserControl
    {
        public AddItemUserControl()
        {
            InitializeComponent();
            DataContext = App.GetServiceProvider().GetService<AddItemUserControlViewModel>();
        }
    }
}

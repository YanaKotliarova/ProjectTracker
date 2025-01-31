using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.MVVM.ViewModel;
using System.Windows.Controls;

namespace ProjectTracker.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for IssuesBoardUserControl.xaml
    /// </summary>
    public partial class IssuesBoardUserControl : UserControl
    {
        public IssuesBoardUserControl()
        {
            InitializeComponent();
            DataContext = App.GetServiceProvider().GetService<IssueBoardUserControlViewModel>();
        }
    }
}

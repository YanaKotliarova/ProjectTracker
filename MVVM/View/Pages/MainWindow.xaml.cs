using MahApps.Metro.Controls;
using ProjectTracker.MVVM.View.UIHelpers.Interfaces;
using System.Windows.Controls;

namespace ProjectTracker.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IMainView
    {
        public Frame Frame
        {
            get { return MainFrame; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
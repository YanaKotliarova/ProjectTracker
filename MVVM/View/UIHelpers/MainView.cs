using ProjectTracker.MVVM.View.Pages;
using ProjectTracker.MVVM.View.UIHelpers.Interfaces;
using System.Windows.Controls;

namespace ProjectTracker.MVVM.View.UIHelpers
{
    public class MainView : IMainView
    {
        private readonly MainWindow _mainWindow;
        public MainView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public Frame Frame
        {
            get { return _mainWindow.Frame; }
        }
    }
}

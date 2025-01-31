using MahApps.Metro.Controls.Dialogs;

namespace ProjectTracker.MVVM.View.UI.Interfaces
{
    public interface IMetroDialog
    {
        Task CloseMessageWithProgressBar();
        Task<bool> ShowConfirmationMessage(object viewModel, string header, string message);
        Task ShowMessage(object viewModel, string header, string message);
        Task ShowMessageWithProgressBar(object viewModel, string header, string message);
    }
}
using MahApps.Metro.Controls.Dialogs;
using ProjectTracker.MVVM.View.UI.Interfaces;

namespace ProjectTracker.MVVM.View.UI
{
    public class MetroDialog : IMetroDialog
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        public MetroDialog(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        /// <summary>
        /// The method to show message in MetroMahapps dialog.
        /// </summary>
        /// <param name="header"> Header of dialog. </param>
        /// <param name="message"> Message of dialog. </param>
        /// <returns></returns>
        public async Task ShowMessage(object viewModel, string header, string message)
        {
            await _dialogCoordinator.ShowMessageAsync(viewModel, header, message);
        }

        /// <summary>
        /// The method to show message in MetroMahapps dialog with two buttons to confirm or deny actions.
        /// </summary>
        /// <param name="viewModel"> ViewModel for creating a bound. </param>
        /// <param name="header">  Header of dialog. </param>
        /// <param name="message"> Message of dialog. </param>
        /// <returns> Returns true if action is confirmed, otherwise false. </returns>
        public async Task<bool> ShowConfirmationMessage(object viewModel, string header, string message)
        {
            if (await _dialogCoordinator.ShowMessageAsync(viewModel, header, message, 
                MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
                return true;
            else return false;
        }

        ProgressDialogController Controller { get; set; }

        /// <summary>
        /// The method to show message with progress bar in MetroMahapps dialog.
        /// </summary>
        /// <param name="viewModel"> ViewModel for creating a bound. </param>
        /// <param name="header"> Header of dialog. </param>
        /// <param name="message"> Message of dialog. </param>
        /// <returns> It returns ProgressDialogController for CloseMessageWithProgressBar method. </returns>
        public async Task ShowMessageWithProgressBar(object viewModel, string header, string message)
        {
            Controller = await _dialogCoordinator.ShowProgressAsync(viewModel, header, message);
            Controller.SetIndeterminate();
        }

        /// <summary>
        /// The method to close MetroMahapps dialog with prodress bar.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public async Task CloseMessageWithProgressBar()
        {
            await Controller.CloseAsync();
        }
    }
}

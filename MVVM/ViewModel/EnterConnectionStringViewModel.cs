using MahApps.Metro.Controls.Dialogs;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class EnterConnectionStringViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly IMetroDialog _metroDialog;

        public EnterConnectionStringViewModel(INavigationService navigationService, IRepository repository, IMetroDialog metroDialog)
        {
            NavigationService = navigationService;
            _repository = repository;
            _metroDialog = metroDialog;
        }

        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                _navigationService = value;
                OnPropertyChanged(nameof(NavigationService));
            }
        }

        private string _newConnectionStringTextBox;
        public string NewConnectionStringTextBox
        {
            get { return _newConnectionStringTextBox; }
            set 
            { 
                _newConnectionStringTextBox = value; 
                OnPropertyChanged(nameof(NewConnectionStringTextBox));
            }
        }

        private bool _isButtonVisible;
        public bool IsButtonVisible
        {
            get { return _isButtonVisible; }
            set
            {
                _isButtonVisible = value;
                OnPropertyChanged(nameof(IsButtonVisible));
            }
        }

        private RelayCommand _retryDbConnectionCommand;
        public RelayCommand RetryDbConnectionCommand
        {
            get
            {
                return _retryDbConnectionCommand ??
                        (_retryDbConnectionCommand = new RelayCommand(async obj =>
                        {
                            try
                            {
                                await _metroDialog.ShowMessageWithProgressBar(this, Properties.Resources.PleaseWait, 
                                    Properties.Resources.ReconnectionToDatabase);

                                await Task.Run(async () => await _repository.InitializeDbAsync(NewConnectionStringTextBox));

                                await _metroDialog.CloseMessageWithProgressBar();
                                await _metroDialog.ShowMessage(this, Properties.Resources.Success,
                                    Properties.Resources.ConnectionCompletedSuccessfully);

                                NavigationService.NavigateTo<AutorizationPageViewModel>();
                            }
                            catch (Exception ex)
                            {
                                await _metroDialog.CloseMessageWithProgressBar();
                                await _metroDialog.ShowMessage(this, Properties.Resources.Failure,
                                    Properties.Resources.ConnectionCompletedUnsuccessfully);
                            }
                            
                        }));
            }
        }
    }
}

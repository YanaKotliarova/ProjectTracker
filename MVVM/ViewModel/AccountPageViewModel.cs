using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {
        private const int minPasswordLength = 8;

        private readonly IAccountService _account;
        private readonly IMetroDialog _metroDialog;
        public AccountPageViewModel(INavigationService navigationService, IAccountService account, IMetroDialog metroDialog)
        {
            NavigationService = navigationService;
            _account = account;
            _metroDialog = metroDialog;
        }

        private INavigationService _navigationService;
        /// <summary>
        /// A property for navigating between views.
        /// </summary>
        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                _navigationService = value;
                OnPropertyChanged(nameof(NavigationService));
            }
        }

        private string _loginTextBox;
        /// <summary>
        /// A property for binding a user's login and a TextBox for it.
        /// </summary>
        public string LoginTextBox
        {
            get { return _loginTextBox; }
            set
            {
                _loginTextBox = value;
                OnPropertyChanged(nameof(LoginTextBox));
            }
        }

        private string _roleTextBox;
        /// <summary>
        /// A property for binding a user's role and a TextBox for it.
        /// </summary>
        public string RoleTextBox
        {
            get { return _roleTextBox; }
            set
            {
                _roleTextBox = value;
                OnPropertyChanged(nameof(RoleTextBox));
            }
        }

        private string _newPasswordBox;
        /// <summary>
        /// A property for binding a user's new password and a PasswordBox for it.
        /// </summary>
        public string NewPasswordBox
        {
            get { return _newPasswordBox; }
            set
            {
                _newPasswordBox = value;
                OnPropertyChanged(nameof(NewPasswordBox));
            }
        }

        private string _repeatPasswordBox;
        /// <summary>
        /// A property for binding a repeated password and a PasswordBox for it.
        /// </summary>
        public string RepeatPasswordBox
        {
            get { return _repeatPasswordBox; }
            set
            {
                _repeatPasswordBox = value;
                OnPropertyChanged(nameof(RepeatPasswordBox));
            }
        }

        private bool _passwordsNotEqual;
        /// <summary>
        /// A property for binding a result of cheking entered passwords and an IsOpen property of certain Popup.
        /// </summary>
        public bool PasswordsNotEqual
        {
            get { return _passwordsNotEqual; }
            set
            {
                _passwordsNotEqual = value;
                OnPropertyChanged(nameof(PasswordsNotEqual));
            }
        }

        private bool _isLoginExists;
        /// <summary>
        /// A property for binding a result of cheking existence of login in database and an IsOpen property of certain Popup.
        /// </summary>
        public bool IsLoginExists
        {
            get { return _isLoginExists; }
            set
            {
                _isLoginExists = value;
                OnPropertyChanged(nameof(IsLoginExists));
            }
        }

        private bool _isPasswordLengthEnough;
        /// <summary>
        /// A property for binding a result of cheking the lenth of new password and an IsOpen property of certain Popup.
        /// </summary>
        public bool IsPasswordLengthEnough
        {
            get { return _isPasswordLengthEnough; }
            set
            {
                _isPasswordLengthEnough = value;
                OnPropertyChanged(nameof(IsPasswordLengthEnough));
            }
        }

        private bool _hasInfoNotChanged;
        /// <summary>
        /// A property for binding a result of cheking if user's personal information has changed
        /// and an IsOpen property of certain Popup.
        /// </summary>
        public bool HasInfoNotChanged
        {
            get { return _hasInfoNotChanged; }
            set
            {
                _hasInfoNotChanged = value;
                OnPropertyChanged(nameof(HasInfoNotChanged));
            }
        }

        private RelayCommand _loadAccountPageCommand;
        /// <summary>
        /// The command that is called when the page loads to update the controls.
        /// </summary>
        public RelayCommand LoadAccountPageCommand
        {
            get
            {
                return _loadAccountPageCommand ??
                    (_loadAccountPageCommand = new RelayCommand(obj =>
                    {
                        UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _saveNewPersonalInfoCommand;
        /// <summary>
        /// The command that is called when button to save updated personal information is clicked.
        /// </summary>
        public RelayCommand SaveNewPersonalInfoCommand
        {
            get
            {
                return _saveNewPersonalInfoCommand ??
                    (_saveNewPersonalInfoCommand = new RelayCommand(async obj =>
                    {
                        HasInfoNotChanged = !HasInfoChangedCheck();
                        if(!HasInfoNotChanged)
                        {
                            if (await _metroDialog.ShowConfirmationMessage(this, 
                                Properties.Resources.ConfirmInfoChanges, ""))
                            {
                                IsLoginExists = await _account.CheckIfLoginExistsAsync(LoginTextBox);

                                if (!IsLoginExists)
                                {
                                    await _account.UpdateUserPersonalInfoAsync(LoginTextBox, RoleTextBox);
                                    await _metroDialog.ShowMessage(this, Properties.Resources.Success, 
                                        Properties.Resources.PersonalInfoUpdated);
                                }
                            }
                            else
                            {
                                UpdatePageControls();
                            }
                        }                        
                    }));
            }
        }

        private RelayCommand _saveNewPasswordCommand;
        /// <summary>
        /// The command that is called when button to update password is clicked.
        /// </summary>
        public RelayCommand SaveNewPasswordCommand
        {
            get
            {
                return _saveNewPasswordCommand ??
                    (_saveNewPasswordCommand = new RelayCommand(async obj =>
                    {
                        if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmPasswordChanges, ""))
                        {
                            IsPasswordLengthEnough = NewPasswordBox.Length is not >= minPasswordLength;

                            if(!IsPasswordLengthEnough)
                            {
                                PasswordsNotEqual = !NewPasswordBox.Equals(RepeatPasswordBox);

                                if (!PasswordsNotEqual)
                                {
                                    await _account.UpdateUserPasswordAsync(NewPasswordBox);
                                    await _metroDialog.ShowMessage(this, Properties.Resources.Success, 
                                        Properties.Resources.PasswordUpdated);
                                }

                                UpdatePageControls();
                            }                            
                        }
                    }, x => !NewPasswordBox.IsNullOrEmpty() && !RepeatPasswordBox.IsNullOrEmpty()));
            }
        }

        private RelayCommand _logOutCommand;
        /// <summary>
        /// The command that is called when log out button is clicked.
        /// </summary>
        public RelayCommand LogOutCommand
        {
            get
            {
                return _logOutCommand ??
                    (_logOutCommand = new RelayCommand(async obj =>
                    {
                        if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmLogOut, ""))
                        {
                            NavigationService.NavigateTo<AutorizationPageViewModel>();
                            _account.ResetCurrentUser();
                        }
                    }));
            }
        }

        private RelayCommand _deleteAccountCommand;
        /// <summary>
        /// The command that is called when delete account button is clicked.
        /// </summary>
        public RelayCommand DeleteAccountCommand
        {
            get
            {
                return _deleteAccountCommand ??
                    (_deleteAccountCommand = new RelayCommand(async obj =>
                    {
                        if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmAccountDelete, Properties.Resources.ActionIrreversible))
                        {
                            await _account.DeleteAccountAsync();
                            NavigationService.NavigateTo<AutorizationPageViewModel>();
                        }                            
                    }));
            }
        }

        /// <summary>
        /// The method for updating page controls.
        /// </summary>
        private void UpdatePageControls()
        {
            LoginTextBox = _account.CustomPrincipal.Identity.Login;
            RoleTextBox = _account.CustomPrincipal.Identity.Role;
            NewPasswordBox = "";
            RepeatPasswordBox = "";
        }

        /// <summary>
        /// The method for checking if user personal information has been changed.
        /// </summary>
        /// <returns></returns>
        private bool HasInfoChangedCheck()
        {
            return !_account.CustomPrincipal.Identity.Login.Equals(LoginTextBox) && !LoginTextBox.IsNullOrEmpty() ||
                !_account.CustomPrincipal.Identity.Role.Equals(RoleTextBox);
        }
    }
}

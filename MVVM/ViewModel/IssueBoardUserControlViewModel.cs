using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class IssueBoardUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithIssueService _workWithIssue;
        private readonly IWorkWithProjectService _workWithProject;
        private readonly IMetroDialog _metroDialog;
        public IssueBoardUserControlViewModel(INavigationService navigationService, 
            IWorkWithIssueService workWithIssue, IWorkWithProjectService workWithProject, IMetroDialog metroDialog)
        {
            NavigationService = navigationService;
            _workWithIssue = workWithIssue;
            _workWithProject = workWithProject;
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

        private ObservableCollection<Issue> _toDoIssuesList;
        public ObservableCollection<Issue> ToDoIssuesList
        {
            get { return _toDoIssuesList; }
            set
            {
                _toDoIssuesList = value;
                OnPropertyChanged(nameof(ToDoIssuesList));
            }
        }

        private ObservableCollection<Issue> _inProgressIssuesList;
        public ObservableCollection<Issue> InProgressIssuesList
        {
            get { return _inProgressIssuesList; }
            set
            {
                _inProgressIssuesList = value;
                OnPropertyChanged(nameof(InProgressIssuesList));
            }
        }

        private ObservableCollection<Issue> _reviewIssuesList;
        public ObservableCollection<Issue> ReviewIssuesList
        {
            get { return _reviewIssuesList; }
            set
            {
                _reviewIssuesList = value;
                OnPropertyChanged(nameof(ReviewIssuesList));
            }
        }

        private ObservableCollection<Issue> _doneIssuesList;
        public ObservableCollection<Issue> DoneIssuesList
        {
            get { return _doneIssuesList; }
            set
            {
                _doneIssuesList = value;
                OnPropertyChanged(nameof(DoneIssuesList));
            }
        }

        private Issue _selectedIssue;
        public Issue SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                OnPropertyChanged(nameof(SelectedIssue));
            }
        }

        private RelayCommand _loadUserControlCommand;
        public RelayCommand LoadUserControlCommand
        {
            get
            {
                return _loadUserControlCommand ??
                    (_loadUserControlCommand = new RelayCommand(obj =>
                    {
                        UpdateIssuesCollections();
                    }));
            }
        }

        private RelayCommand _doubleIssueClickCommand;
        public RelayCommand DoubleIssueClickCommand
        {
            get
            {
                return _doubleIssueClickCommand ??
                    (_doubleIssueClickCommand = new RelayCommand(obj =>
                    {
                        _workWithIssue.SelectedIssue = SelectedIssue;
                        NavigationService.NavigateTo<IssuePageViewModel>();
                    }));
            }
        }

        private RelayCommand _deleteIssueCommand;
        public RelayCommand DeleteIssueCommand
        {
            get
            {
                return _deleteIssueCommand ??
                    (_deleteIssueCommand = new RelayCommand(async obj =>
                    {
                        if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmIssueDelete, Properties.Resources.ActionIrreversible))
                        {
                            _workWithIssue.SelectedIssue = SelectedIssue;
                            await _workWithIssue.DeleteIssueAsync();
                            UpdateIssuesCollections();
                        }                            
                    }));
            }
        }

        private void UpdateIssuesCollections()
        {
            List<Issue> tempToDoIssuesList = new List<Issue>();
            List<Issue> tempInProgressIssuesList = new List<Issue>();
            List<Issue> tempReviewIssuesList = new List<Issue>();
            List<Issue> tempDoneIssuesList = new List<Issue>();

            foreach (var p in _workWithProject.GetUserProjectsList())
            {
                tempToDoIssuesList.AddRange(_workWithIssue.GetIssuesList(p.Id, Properties.Resources.ToDoStatus));
                tempInProgressIssuesList.AddRange(_workWithIssue.GetIssuesList(p.Id, Properties.Resources.InProgressStatus));
                tempReviewIssuesList.AddRange(_workWithIssue.GetIssuesList(p.Id, Properties.Resources.ReviewStatus));
                tempDoneIssuesList.AddRange(_workWithIssue.GetIssuesList(p.Id, Properties.Resources.DoneStatus));
            }

            ToDoIssuesList = _workWithIssue.CreateCollection(tempToDoIssuesList);
            InProgressIssuesList = _workWithIssue.CreateCollection(tempInProgressIssuesList);
            ReviewIssuesList = _workWithIssue.CreateCollection(tempReviewIssuesList);
            DoneIssuesList = _workWithIssue.CreateCollection(tempDoneIssuesList);
        }
    }
}

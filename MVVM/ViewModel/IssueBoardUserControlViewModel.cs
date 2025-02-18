using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.ServiceHelpers.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class IssueBoardUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithIssueService _workWithIssue;
        private readonly IWorkWithProjectService _workWithProject;
        private readonly IMetroDialog _metroDialog;
        private readonly ICollectionHelper _collectionHelper;

        public IssueBoardUserControlViewModel(INavigationService navigationService,
            IWorkWithIssueService workWithIssue, IWorkWithProjectService workWithProject,
            IMetroDialog metroDialog, ICollectionHelper collectionHelper)
        {
            NavigationService = navigationService;
            _workWithIssue = workWithIssue;
            _workWithProject = workWithProject;
            _metroDialog = metroDialog;
            _collectionHelper = collectionHelper;

            WindowName = Properties.Resources.IssuesLabel;
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

        private ObservableCollection<Issue> _toDoIssuesList;
        /// <summary>
        /// An observable collection of issues with "to do" status, to put them into ListBox.
        /// </summary>
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
        /// <summary>
        /// An observable collection of issues with "in progress" status, to put them into ListBox.
        /// </summary>
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
        /// <summary>
        /// An observable collection of issues with "review" status, to put them into ListBox.
        /// </summary>
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
        /// <summary>
        /// An observable collection of issues with "done" status, to put them into ListBox.
        /// </summary>
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
        /// <summary>
        /// A property for getting the selected issue.
        /// </summary>
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
        /// <summary>
        /// The command that is called when the user control loads to update the controls.
        /// </summary>
        public RelayCommand LoadUserControlCommand
        {
            get
            {
                return _loadUserControlCommand ??
                    (_loadUserControlCommand = new RelayCommand(async obj =>
                    {
                        await UpdateIssuesCollections();
                    }));
            }
        }

        private RelayCommand _doubleIssueClickCommand;
        /// <summary>
        /// The command that is binded with DoubleMouseClick event of ListBox item
        /// and when update MenuItem of issue in ListBox is clicked.
        /// </summary>
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
        /// <summary>
        /// The command that is called when delete MenuItem of issue in ListBox is clicked.
        /// </summary>
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
                            await UpdateIssuesCollections();
                        }
                    }));
            }
        }

        /// <summary>
        /// The method for updating collections of issues.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateIssuesCollections()
        {
            List<Issue> tempToDoIssuesList = new List<Issue>();
            List<Issue> tempInProgressIssuesList = new List<Issue>();
            List<Issue> tempReviewIssuesList = new List<Issue>();
            List<Issue> tempDoneIssuesList = new List<Issue>();

            foreach (Project p in await _workWithProject.GetUserProjectsListAsync())
            {
                tempToDoIssuesList.AddRange(await _workWithIssue.GetIssuesByStatusAsync(p.Id, Properties.Resources.ToDoStatus));
                tempInProgressIssuesList.AddRange(await _workWithIssue.GetIssuesByStatusAsync(p.Id, Properties.Resources.InProgressStatus));
                tempReviewIssuesList.AddRange(await _workWithIssue.GetIssuesByStatusAsync(p.Id, Properties.Resources.ReviewStatus));
                tempDoneIssuesList.AddRange(await _workWithIssue.GetIssuesByStatusAsync(p.Id, Properties.Resources.DoneStatus));
            }

            ToDoIssuesList = _collectionHelper.CreateCollection(tempToDoIssuesList);
            InProgressIssuesList = _collectionHelper.CreateCollection(tempInProgressIssuesList);
            ReviewIssuesList = _collectionHelper.CreateCollection(tempReviewIssuesList);
            DoneIssuesList = _collectionHelper.CreateCollection(tempDoneIssuesList);
        }
    }
}

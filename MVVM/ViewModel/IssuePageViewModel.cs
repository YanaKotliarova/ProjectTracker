using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.ServiceHelpers.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class IssuePageViewModel : ViewModelBase
    {
        private readonly IWorkWithIssueService _workWithIssue;
        private readonly IWorkWithProjectService _workWithProject;
        private readonly IMetroDialog _metroDialog;
        private readonly ICollectionHelper _collectionHelper;

        public IssuePageViewModel(IWorkWithIssueService workWithIssue, IWorkWithProjectService workWithProject, 
            IMetroDialog metroDialog, ICollectionHelper collectionHelper)
        {
            _workWithIssue = workWithIssue;
            _workWithProject = workWithProject;
            _metroDialog = metroDialog;
            _collectionHelper = collectionHelper;
        }


        private ObservableCollection<Project> _projectsList;
        /// <summary>
        /// An observable collection of projects for creating selection.
        /// </summary>
        public ObservableCollection<Project> ProjectsList
        {
            get { return _projectsList; }
            set
            {
                _projectsList = value;
                OnPropertyChanged(nameof(ProjectsList));
            }
        }

        private Project _selectedProject;
        /// <summary>
        /// A property for getting the selected project.
        /// </summary>
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private ObservableCollection<string> _statusList;
        /// <summary>
        /// An observable collection of statuses for creating selection.
        /// </summary>
        public ObservableCollection<string> StatusList
        {
            get { return _statusList; }
            set
            {
                _statusList = value;
                OnPropertyChanged(nameof(StatusList));
            }
        }

        private string _selectedStatus;
        /// <summary>
        /// A property for getting the selected status.
        /// </summary>
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        private ObservableCollection<string> _availableStatusList;
        /// <summary>
        /// An observable collection of statuses, to list them for updating issue information.
        /// </summary>
        public ObservableCollection<string> AvailableStatusList
        {
            get { return _availableStatusList; }
            set
            {
                _availableStatusList = value;
                OnPropertyChanged(nameof(AvailableStatusList));
            }
        }

        private string _currentIssueStatus;
        /// <summary>
        /// A property for getting the selected status for updating.
        /// </summary>
        public string CurrentIssueStatus
        {
            get { return _currentIssueStatus; }
            set
            {
                _currentIssueStatus = value;
                OnPropertyChanged(nameof(CurrentIssueStatus));
            }
        }

        private ObservableCollection<string> _priorityList;
        /// <summary>
        /// An observable collection of priorities for creating selection.
        /// </summary>
        public ObservableCollection<string> PriorityList
        {
            get { return _priorityList; }
            set
            {
                _priorityList = value;
                OnPropertyChanged(nameof(PriorityList));
            }
        }

        private string _selectedPriority;
        /// <summary>
        /// A property for getting the selected priority.
        /// </summary>
        public string SelectedPriority
        {
            get { return _selectedPriority; }
            set
            {
                _selectedPriority = value;
                OnPropertyChanged(nameof(SelectedPriority));
            }
        }

        private ObservableCollection<string> _availablePriorityList;
        /// <summary>
        /// An observable collection of priorities, to list them for updating issue information.
        /// </summary>
        public ObservableCollection<string> AvailablePriorityList
        {
            get { return _availablePriorityList; }
            set
            {
                _availablePriorityList = value;
                OnPropertyChanged(nameof(AvailablePriorityList));
            }
        }

        private string _currentIssuePriority;
        /// <summary>
        /// A property for getting the selected priority for change.
        /// </summary>
        public string CurrentIssuePriority
        {
            get { return _currentIssuePriority; }
            set
            {
                _currentIssuePriority = value;
                OnPropertyChanged(nameof(CurrentIssuePriority));
            }
        }

        private ObservableCollection<Issue> _issuesList;
        /// <summary>
        /// An observable collection of issues, to list them for viewing.
        /// </summary>
        public ObservableCollection<Issue> IssuesList
        {
            get { return _issuesList; }
            set
            {
                _issuesList = value;
                OnPropertyChanged(nameof(IssuesList));
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

        private string _issueNameTextBox;
        /// <summary>
        /// A property for binding a issue name and a TextBox for it.
        /// </summary>
        public string IssueNameTextBox
        {
            get { return _issueNameTextBox; }
            set
            {
                _issueNameTextBox = value;
                OnPropertyChanged(nameof(IssueNameTextBox));
            }
        }

        private string _descriptionTextBox;
        /// <summary>
        /// A property for binding a issue description and a TextBox for it.
        /// </summary>
        public string DescriptionTextBox
        {
            get { return _descriptionTextBox; }
            set
            {
                _descriptionTextBox = value;
                OnPropertyChanged(nameof(DescriptionTextBox));
            }
        }

        private string _commentTextBox;
        /// <summary>
        /// A property for binding a issue comment and a TextBox for it.
        /// </summary>
        public string CommentTextBox
        {
            get { return _commentTextBox; }
            set
            {
                _commentTextBox = value;
                OnPropertyChanged(nameof(CommentTextBox));
            }
        }

        private ObservableCollection<string> _labelsList;
        /// <summary>
        /// An observable collection of selected issue labels.
        /// </summary>
        public ObservableCollection<string> LabelsList
        {
            get { return _labelsList; }
            set
            {
                _labelsList = value;
                OnPropertyChanged(nameof(LabelsList));
            }
        }

        private string _addLabelTextBox;
        /// <summary>
        /// A property for binding a created issue label and a TextBox for it.
        /// </summary>
        public string AddLabelTextBox
        {
            get { return _addLabelTextBox; }
            set
            {
                _addLabelTextBox = value;
                OnPropertyChanged(nameof(AddLabelTextBox));
            }
        }

        private string _selectedLabel;
        /// <summary>
        /// A property for getting the selected label.
        /// </summary>
        public string SelectedLabel
        {
            get { return _selectedLabel; }
            set
            {
                _selectedLabel = value;
                OnPropertyChanged(nameof(SelectedLabel));
            }
        }

        private string _updatedLabelTextBox;
        /// <summary>
        /// A property for binding an updated issue label and a TextBox for it.
        /// </summary>
        public string UpdatedLabelTextBox
        {
            get { return _updatedLabelTextBox; }
            set
            {
                _updatedLabelTextBox = value;
                OnPropertyChanged(nameof(UpdatedLabelTextBox));
            }
        }

        private bool _isLabelUpdated;
        /// <summary>
        /// A property for binding a value if label is updated and an IsOpen property of certain Popup.
        /// </summary>
        public bool IsLabelUpdated
        {
            get { return _isLabelUpdated; }
            set
            {
                _isLabelUpdated = value;
                OnPropertyChanged(nameof(IsLabelUpdated));
            }
        }

        private bool _isThereSameIssueName;
        /// <summary>
        /// A property for binding a result of cheking if entered new issue name is exists in its project 
        /// in database and an IsOpen property of certain Popup.
        /// </summary>
        public bool IsThereSameIssueName
        {
            get { return _isThereSameIssueName; }
            set
            {
                _isThereSameIssueName = value;
                OnPropertyChanged(nameof(IsThereSameIssueName));
            }
        }

        private bool _hasInfoNotChanged;
        /// <summary>
        /// A property for binding a result of cheking if issue information has changed
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

        private RelayCommand _loadIssuePageCommand;
        /// <summary>
        /// The command that is called when the page loads to update the controls.
        /// </summary>
        public RelayCommand LoadIssuePageCommand
        {
            get
            {
                return _loadIssuePageCommand ??
                    (_loadIssuePageCommand = new RelayCommand(async obj =>
                    {
                        await UpdateProjectsList();
                        CreateStatusList();
                        CreatePriorityList();
                        CreateAvailableStatusList();
                        CreateAvailablePriorityList();
                        await UpdateIssuesList();

                        UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _selectionChangedCommand;
        /// <summary>
        /// The command that is binded with SelectionChanged event of ListBox of issues.
        /// </summary>
        public RelayCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ??
                    (_selectionChangedCommand = new RelayCommand(obj =>
                    {
                        _workWithIssue.SelectedIssue = SelectedIssue;
                        UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _listSelectionChangedCommand;
        /// <summary>
        /// The command that is binded with SelectionChanged event of ComboBox with projects, 
        /// statuses or priorities for creating selection.
        /// </summary>
        public RelayCommand ListSelectionChangedCommand
        {
            get
            {
                return _listSelectionChangedCommand ??
                    (_listSelectionChangedCommand = new RelayCommand(async obj =>
                    {
                        await UpdateIssuesList();
                    }));
            }
        }

        private RelayCommand _updateIssueCommand;
        /// <summary>
        /// The command whitch is called when update issue button is clicked.
        /// </summary>
        public RelayCommand UpdateIssueCommand
        {
            get
            {
                return _updateIssueCommand ??
                    (_updateIssueCommand = new RelayCommand(async obj =>
                    {
                        HasInfoNotChanged = !HasInfoChangedCheck();
                        if(!HasInfoNotChanged)
                        {
                            if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmIssueChanges, ""))
                            {
                                IsThereSameIssueName = await _workWithIssue.ChechIssueNameAsync(SelectedIssue.ProjectId, IssueNameTextBox);
                                if (!IsThereSameIssueName)
                                {
                                    SelectedIssue.Name = IssueNameTextBox;
                                    SelectedIssue.Description = DescriptionTextBox;
                                    SelectedIssue.Comment = CommentTextBox;
                                    SelectedIssue.Status = CurrentIssueStatus;
                                    SelectedIssue.Priority = CurrentIssuePriority;

                                    _workWithIssue.SelectedIssue = SelectedIssue;
                                    await _workWithIssue.UpdateIssueInfoAsync();

                                    await UpdateIssuesList();

                                    await _metroDialog.ShowMessage(this, Properties.Resources.Success,
                                        Properties.Resources.IssueUpdated);
                                }
                            }
                        }                                  
                    }, x => SelectedIssue != null ));
            }
        }

        private RelayCommand _addLabelCommand;
        /// <summary>
        /// The command whitch is called when add label button is clicked.
        /// </summary>
        public RelayCommand AddLabelCommand
        {
            get
            {
                return _addLabelCommand ??
                    (_addLabelCommand = new RelayCommand(async obj =>
                    {
                        if (!AddLabelTextBox.IsNullOrEmpty())
                        {
                            SelectedIssue.Labels.Add(AddLabelTextBox);
                            await _workWithIssue.UpdateIssueInfoAsync();
                            AddLabelTextBox = "";
                            UpdatePageControls();
                        }
                    }));
            }
        }

        private RelayCommand _updateLabelCommand;
        /// <summary>
        /// The command whitch is called when update label button on update panel is clicked.
        /// </summary>
        public RelayCommand UpdateLabelCommand
        {
            get
            {
                return _updateLabelCommand ??
                    (_updateLabelCommand = new RelayCommand(async obj =>
                    {
                        SelectedIssue.Labels[SelectedIssue.Labels.FindIndex(l => l == SelectedLabel)] = UpdatedLabelTextBox;
                        await _workWithIssue.UpdateIssueInfoAsync();
                        UpdatePageControls();
                        IsLabelUpdated = false;
                    }));
            }
        }

        private RelayCommand _openUpdatePanelCommand;
        /// <summary>
        /// The command whitch is called when update label button is clicked.
        /// </summary>
        public RelayCommand OpenUpdatePanelCommand
        {
            get
            {
                return _openUpdatePanelCommand ??
                    (_openUpdatePanelCommand = new RelayCommand(obj =>
                    {
                        UpdatedLabelTextBox = SelectedLabel;
                        IsLabelUpdated = true;
                    }));
            }
        }

        private RelayCommand _closeUpdatePanelCommand;
        /// <summary>
        /// The command whitch is called when close update label panel button and 
        /// update label button on update label panel is clicked.
        /// </summary>
        public RelayCommand CloseUpdatePanelCommand
        {
            get
            {
                return _closeUpdatePanelCommand ??
                    (_closeUpdatePanelCommand = new RelayCommand(obj =>
                    {
                        IsLabelUpdated = false;
                    }));
            }
        }

        private RelayCommand _deleteLabelCommand;
        /// <summary>
        /// The command that is called when delete label MenuItem is clicked.
        /// </summary>
        public RelayCommand DeleteLabelCommand
        {
            get
            {
                return _deleteLabelCommand ??
                    (_deleteLabelCommand = new RelayCommand(async obj =>
                    {
                        SelectedIssue.Labels.Remove(SelectedLabel);
                        await _workWithIssue.UpdateIssueInfoAsync();
                        UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _deleteIssueCommand;
        /// <summary>
        /// The command that is called when delete MenuItem of issue in ListBox or delete button is clicked.
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
                            await _workWithIssue.DeleteIssueAsync();
                            int index = IssuesList.IndexOf(SelectedIssue);
                            int count = IssuesList.Count - 1;
                            if (count > 0 && index != count)
                            {
                                SelectedIssue = IssuesList[index + 1];
                            }
                            else if (count > 0 && index == count)
                            {
                                SelectedIssue = IssuesList[index - 1];
                            }
                            else SelectedIssue = null;
                            await UpdateIssuesList();
                        }                                
                    }, x => SelectedIssue != null));
            }
        }

        /// <summary>
        /// The method for updating page controls.
        /// </summary>
        private void UpdatePageControls()
        {
            if (SelectedIssue != null)
            {
                IssueNameTextBox = SelectedIssue.Name;
                DescriptionTextBox = SelectedIssue.Description;
                CommentTextBox = SelectedIssue.Comment;
                CurrentIssueStatus = SelectedIssue.Status;
                CurrentIssuePriority = SelectedIssue.Priority;
                LabelsList = new ObservableCollection<string>(SelectedIssue.Labels);
            }
            else
            {
                IssueNameTextBox = "";
                DescriptionTextBox = "";
                CommentTextBox = "";
                CurrentIssueStatus = default;
                CurrentIssuePriority = default;
                LabelsList = default;
            }
        }

        /// <summary>
        /// The method for updating visible issue collection.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateIssuesList()
        {
            List<Issue> issues = await _workWithIssue.GetAllUserIssuesAsync();

            if (!SelectedProject.Name.Equals(Properties.Resources.All))
            {
                List<Issue> tempIssueList = new List<Issue>();
                foreach (Issue i in issues)
                {
                    if(SelectedProject.Name.Equals(await _workWithProject.GetProjectNameAsync(i.ProjectId)))
                        tempIssueList.Add(i);
                }
                issues = tempIssueList;
            }

            if (!SelectedStatus.Equals(Properties.Resources.All))
                issues = issues.FindAll(i => i.Status.Equals(SelectedStatus));

            if (!SelectedPriority.Equals(Properties.Resources.All))
                issues = issues.FindAll(i => i.Priority.Equals(SelectedPriority));

            IssuesList = _collectionHelper.CreateCollection(issues);
            SelectedIssue = _workWithIssue.SelectedIssue;
        }

        /// <summary>
        /// The method for updating collection of projects for selections.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateProjectsList()
        {
            ProjectsList = _collectionHelper.CreateCollection<Project>(await _workWithProject.GetUserProjectsListAsync());

            Project allProject = new Project();
            allProject.Name = Properties.Resources.All;
            ProjectsList.Insert(0, allProject);
            SelectedProject = allProject;
        }

        /// <summary>
        /// The method for creating collection of statuses for selections.
        /// </summary>
        private void CreateStatusList()
        {
            StatusList = [Properties.Resources.All, Properties.Resources.ToDoStatus, Properties.Resources.InProgressStatus,
                Properties.Resources.ReviewStatus, Properties.Resources.DoneStatus];
            SelectedStatus = Properties.Resources.All;
        }

        /// <summary>
        /// The method for creating collection of priorities for selections.
        /// </summary>
        private void CreatePriorityList()
        {
            PriorityList = [Properties.Resources.All, Properties.Resources.LowPriority, 
                Properties.Resources.MediumPriority, Properties.Resources.HighPriority];
            SelectedPriority = Properties.Resources.All;
        }

        /// <summary>
        /// The method for creating collection of statuses for updating.
        /// </summary>
        private void CreateAvailableStatusList()
        {
            AvailableStatusList = [Properties.Resources.ToDoStatus, Properties.Resources.InProgressStatus,
                Properties.Resources.ReviewStatus, Properties.Resources.DoneStatus];
        }

        /// <summary>
        /// The method for creating collection of priorities for updating.
        /// </summary>
        private void CreateAvailablePriorityList()
        {
            AvailablePriorityList = [Properties.Resources.LowPriority,
                Properties.Resources.MediumPriority, Properties.Resources.HighPriority];
        }

        /// <summary>
        /// The method for checking if issue information has been changed.
        /// </summary>
        /// <returns></returns>
        private bool HasInfoChangedCheck()
        {
            return !_workWithIssue.SelectedIssue.Name.Equals(IssueNameTextBox) && !IssueNameTextBox.IsNullOrEmpty() ||
                !_workWithIssue.SelectedIssue.Description.Equals(DescriptionTextBox) ||
                !_workWithIssue.SelectedIssue.Comment.Equals(CommentTextBox) ||
                !_workWithIssue.SelectedIssue.Status.Equals(CurrentIssueStatus) ||
                !_workWithIssue.SelectedIssue.Priority.Equals(CurrentIssuePriority);
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class IssuePageViewModel : ViewModelBase
    {
        private readonly IWorkWithIssue _workWithIssue;
        private readonly IWorkWithProject _workWithProject;
        private readonly IMetroDialog _metroDialog;

        public IssuePageViewModel(IWorkWithIssue workWithIssue, IWorkWithProject workWithProject, IMetroDialog metroDialog)
        {
            _workWithIssue = workWithIssue;
            _workWithProject = workWithProject;
            _metroDialog = metroDialog;
        }


        private ObservableCollection<Project> _projectsList;
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
        public string SelectedLabel
        {
            get { return _selectedLabel; }
            set
            {
                _selectedLabel = value;
                OnPropertyChanged(nameof(SelectedLabel));
            }
        }

        private string _updatedLabel;
        public string UpdatedLabel
        {
            get { return _updatedLabel; }
            set
            {
                _updatedLabel = value;
                OnPropertyChanged(nameof(UpdatedLabel));
            }
        }

        private bool _isLabelUpdated;
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
        public RelayCommand LoadIssuePageCommand
        {
            get
            {
                return _loadIssuePageCommand ??
                    (_loadIssuePageCommand = new RelayCommand(obj =>
                    {
                        UpdateProjectsList();
                        CreateStatusList();
                        CreatePriorityList();
                        CreateAvailableStatusList();
                        CreateAvailablePriorityList();
                        UpdateIssuesList();

                        UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _selectionChangedCommand;
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
        public RelayCommand ListSelectionChangedCommand
        {
            get
            {
                return _listSelectionChangedCommand ??
                    (_listSelectionChangedCommand = new RelayCommand(obj =>
                    {
                        UpdateIssuesList();
                    }));
            }
        }

        private RelayCommand _updateIssueCommand;
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
                            "Are you sure you want to update your issue?", ""))
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
                                    await _workWithIssue.UpdateIssueInfo();

                                    UpdateIssuesList();

                                    await _metroDialog.ShowMessage(this, "Success", "Your issue has been updated");
                                }
                            }
                        }                                  
                    }, x => SelectedIssue != null ));
            }
        }

        private RelayCommand _addLabelCommand;
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
                            await _workWithIssue.UpdateIssueInfo();
                            AddLabelTextBox = "";
                            UpdatePageControls();
                        }
                    }));
            }
        }

        private RelayCommand _updateLabelCommand;
        public RelayCommand UpdateLabelCommand
        {
            get
            {
                return _updateLabelCommand ??
                    (_updateLabelCommand = new RelayCommand(async obj =>
                    {
                        SelectedIssue.Labels[SelectedIssue.Labels.FindIndex(l => l == SelectedLabel)] = UpdatedLabel;
                        await _workWithIssue.UpdateIssueInfo();
                        UpdatePageControls();
                        IsLabelUpdated = false;
                    }));
            }
        }

        private RelayCommand _openUpdatePanelCommand;
        public RelayCommand OpenUpdatePanelCommand
        {
            get
            {
                return _openUpdatePanelCommand ??
                    (_openUpdatePanelCommand = new RelayCommand(obj =>
                    {
                        UpdatedLabel = SelectedLabel;
                        IsLabelUpdated = true;
                    }));
            }
        }

        private RelayCommand _closeUpdatePanelCommand;
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
        public RelayCommand DeleteLabelCommand
        {
            get
            {
                return _deleteLabelCommand ??
                    (_deleteLabelCommand = new RelayCommand(async obj =>
                    {
                        SelectedIssue.Labels.Remove(SelectedLabel);
                        await _workWithIssue.UpdateIssueInfo();
                        UpdatePageControls();
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
                            "Are you sure you want to DELETE your issue?", "This action is irreversible"))
                        {
                            await _workWithIssue.DeleteIssue();
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
                            UpdateIssuesList();
                        }                                
                    }, x => SelectedIssue != null));
            }
        }
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
        private void UpdateIssuesList()
        {
            List<Issue> issues = _workWithIssue.GetAllUserIssues();

            if (!SelectedProject.Name.Equals("All"))
                issues = issues.FindAll(i => _workWithProject.GetProjectName(i.ProjectId).Equals(SelectedProject.Name));

            if (!SelectedStatus.Equals("All"))
                issues = issues.FindAll(i => i.Status.Equals(SelectedStatus));

            if (!SelectedPriority.Equals("All"))
                issues = issues.FindAll(i => i.Priority.Equals(SelectedPriority));

            IssuesList = _workWithIssue.CreateCollection(issues);
            SelectedIssue = _workWithIssue.SelectedIssue;
        }

        private void UpdateProjectsList()
        {
            ProjectsList = _workWithProject.CreateCollection();
            Project allProjects = new Project();
            allProjects.Name = "All";
            ProjectsList.Insert(0, allProjects);
            SelectedProject = allProjects;
        }

        private void CreateStatusList()
        {
            StatusList = ["All", "To Do", "In Progress", "Review", "Done"];
            SelectedStatus = "All";
        }

        private void CreatePriorityList()
        {
            PriorityList = ["All", "Low", "Medium", "High"];
            SelectedPriority = "All";
        }

        private void CreateAvailableStatusList()
        {
            AvailableStatusList = ["To Do", "In Progress", "Review", "Done"];
        }

        private void CreateAvailablePriorityList()
        {
            AvailablePriorityList = ["Low", "Medium", "High"];
        }

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

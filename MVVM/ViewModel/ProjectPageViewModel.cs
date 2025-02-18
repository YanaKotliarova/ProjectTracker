using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.ServiceHelpers.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class ProjectPageViewModel : ViewModelBase
    {
        private readonly IWorkWithProjectService _workWithProject;
        private readonly IWorkWithIssueService _workWithIssue;
        private readonly IMetroDialog _metroDialog;
        private readonly ICollectionHelper _collectionHelper;

        public ProjectPageViewModel(INavigationService navigationService, IWorkWithProjectService workWithProject, 
            IWorkWithIssueService workWithIssue, IMetroDialog metroDialog, ICollectionHelper collectionHelper)
        {
            NavigationService = navigationService;
            _workWithProject = workWithProject;
            _workWithIssue = workWithIssue;
            _metroDialog = metroDialog;
            _collectionHelper = collectionHelper;
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

        private ObservableCollection<Project> _projectsList;
        /// <summary>
        /// An observable collection of projects.
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

        private ObservableCollection<Issue> _issuesList;
        /// <summary>
        /// An observable collection of issues of project.
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

        private ObservableCollection<string> _labelsList;
        /// <summary>
        /// An observable collection of labels of project.
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

        private string _projectNameTextBox;
        /// <summary>
        /// A property for binding a project name and a TextBox for it.
        /// </summary>
        public string ProjectNameTextBox
        {
            get { return _projectNameTextBox; }
            set
            {
                _projectNameTextBox = value;
                OnPropertyChanged(nameof(ProjectNameTextBox));
            }
        }

        private string _descriptionTextBox;
        /// <summary>
        /// A property for binding a project description and a TextBox for it.
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

        private bool _isThereSameProjectName;
        /// <summary>
        /// A property for binding a result of cheking if entered new project name is exists
        /// in database and an IsOpen property of certain Popup.
        /// </summary>
        public bool IsThereSameProjectName
        {
            get { return _isThereSameProjectName; }
            set
            {
                _isThereSameProjectName = value;
                OnPropertyChanged(nameof(IsThereSameProjectName));
            }
        }

        private bool _hasInfoNotChanged;
        /// <summary>
        /// A property for binding a result of cheking if project information has changed
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

        private RelayCommand _loadProjectPageCommand;
        /// <summary>
        /// The command that is called when the page loads to update the controls.
        /// </summary>
        public RelayCommand LoadProjectPageCommand
        {
            get
            {
                return _loadProjectPageCommand ??
                    (_loadProjectPageCommand = new RelayCommand(async obj =>
                    {
                        await UpdateProjectCollection();
                        SelectedProject = _workWithProject.SelectedProject!;
                        await UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _selectionChangedCommand;
        /// <summary>
        /// The command that is binded with SelectionChanged event of ListBox of projects.
        /// </summary>
        public RelayCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ??
                    (_selectionChangedCommand = new RelayCommand(async obj =>
                    {
                        _workWithProject.SelectedProject = SelectedProject;
                        await UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _updateProjectCommand;
        /// <summary>
        /// The command whitch is called when update project button is clicked.
        /// </summary>
        public RelayCommand UpdateProjectCommand
        {
            get
            {
                return _updateProjectCommand ??
                    (_updateProjectCommand = new RelayCommand(async obj =>
                    {
                        HasInfoNotChanged = !HasInfoChangedCheck();
                        if (!HasInfoNotChanged)
                        {
                            if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmProjectChanges, ""))
                            {
                                IsThereSameProjectName = await _workWithProject.CheckProjectNameAsync(ProjectNameTextBox);
                                if (!IsThereSameProjectName)
                                {
                                    SelectedProject.Name = ProjectNameTextBox;
                                    SelectedProject.Description = DescriptionTextBox;

                                    _workWithProject.SelectedProject = SelectedProject;
                                    await _workWithProject.UpdateProjectInfoAsync();

                                    await UpdateProjectCollection();

                                    await _metroDialog.ShowMessage(this, Properties.Resources.Success, 
                                        Properties.Resources.ProjectUpdated);
                                }
                            }
                            else await UpdatePageControls();
                        }                            
                    }, x => SelectedProject != null));
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
                            SelectedProject.Labels.Add(AddLabelTextBox);
                            await _workWithProject.UpdateProjectInfoAsync();
                            AddLabelTextBox = "";
                            await UpdatePageControls();
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
                        SelectedProject.Labels[SelectedProject.Labels.FindIndex(l => l == SelectedLabel)] = UpdatedLabelTextBox;
                        await _workWithProject.UpdateProjectInfoAsync();
                        await UpdatePageControls();
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
                        SelectedProject.Labels.Remove(SelectedLabel);
                        await _workWithProject.UpdateProjectInfoAsync();
                        await UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _doubleIssueClickCommand;
        /// <summary>
        /// The command that is called when DoubleMouseClick event of issue is called.
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

        private RelayCommand _deleteProjectCommand;
        /// <summary>
        /// The command that is called when delete MenuItem of project in ListBox or delete button is clicked.
        /// </summary>
        public RelayCommand DeleteProjectCommand
        {
            get
            {
                return _deleteProjectCommand ??
                    (_deleteProjectCommand = new RelayCommand(async obj =>
                    {
                        if (await _metroDialog.ShowConfirmationMessage(this,
                            Properties.Resources.ConfirmProjectDelete, Properties.Resources.ActionIrreversible))
                        {
                            await _workWithProject.DeleteProjectAsync();
                            int index = ProjectsList.IndexOf(SelectedProject);
                            int count = ProjectsList.Count - 1;
                            if (count > 0 && index != count)
                            {
                                SelectedProject = ProjectsList[index + 1];
                            }
                            else if (count > 0 && index == count)
                            {
                                SelectedProject = ProjectsList[index - 1];
                            }
                            else SelectedProject = null;
                            await UpdateProjectCollection();
                        }                                
                    }, x => SelectedProject != null));
            }
        }

        /// <summary>
        /// The method for updating page controls.
        /// </summary>
        /// <returns></returns>
        private async Task UpdatePageControls()
        {
            if (SelectedProject != null)
            {
                ProjectNameTextBox = SelectedProject.Name;
                DescriptionTextBox = SelectedProject.Description;
                LabelsList = new ObservableCollection<string>(SelectedProject.Labels);
                await UpdateIssueCollection();
            }
            else
            {
                ProjectNameTextBox = "";
                DescriptionTextBox = "";
                LabelsList = default;
                IssuesList = default;
            }
        }

        /// <summary>
        /// The method for update collection of issues.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateIssueCollection()
        {
            List<Issue> issues = new List<Issue>();
            //IssuesList = _collectionHelper.CreateCollection(await _workWithIssue.GetProjectIssuesListAsync());
            IssuesList = _collectionHelper.CreateCollection(ProjectsList.First(p => p.Id == SelectedProject.Id).Issues);
        }

        /// <summary>
        /// The method for update collection of projects.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateProjectCollection()
        {
            ProjectsList = _collectionHelper.CreateCollection<Project>(await _workWithProject.GetUserProjectsListAsync());
        }

        /// <summary>
        /// The method for checking if project information has been changed.
        /// </summary>
        /// <returns></returns>
        private bool HasInfoChangedCheck()
        {
            return !_workWithProject.SelectedProject.Name.Equals(ProjectNameTextBox) && !ProjectNameTextBox.IsNullOrEmpty() ||
                !_workWithProject.SelectedProject.Description.Equals(DescriptionTextBox);
        }
    }
}

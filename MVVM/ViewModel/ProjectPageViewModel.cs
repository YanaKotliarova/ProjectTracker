﻿using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class ProjectPageViewModel : ViewModelBase
    {
        private readonly IWorkWithProjectService _workWithProject;
        private readonly IWorkWithIssueService _workWithIssue;
        private readonly IMetroDialog _metroDialog;

        public ProjectPageViewModel(INavigationService navigationService, IWorkWithProjectService workWithProject, 
            IWorkWithIssueService workWithIssue, IMetroDialog metroDialog)
        {
            NavigationService = navigationService;
            _workWithProject = workWithProject;
            _workWithIssue = workWithIssue;
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

        private string _projectNameTextBox;
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

        private bool _isThereSameProjectName;
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
        public RelayCommand LoadProjectPageCommand
        {
            get
            {
                return _loadProjectPageCommand ??
                    (_loadProjectPageCommand = new RelayCommand(obj =>
                    {
                        UpdateProjectCollection();
                        SelectedProject = _workWithProject.SelectedProject!;
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
                        _workWithProject.SelectedProject = SelectedProject;
                        UpdatePageControls();
                    }));
            }
        }

        private RelayCommand _updateProjectCommand;
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

                                    UpdateProjectCollection();

                                    await _metroDialog.ShowMessage(this, Properties.Resources.Success, Properties.Resources.ProjectUpdated);
                                }
                            }
                            else UpdatePageControls();
                        }                            
                    }, x => SelectedProject != null));
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
                            SelectedProject.Labels.Add(AddLabelTextBox);
                            await _workWithProject.UpdateProjectInfoAsync();
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
                        SelectedProject.Labels[SelectedProject.Labels.FindIndex(l => l == SelectedLabel)] = UpdatedLabel;
                        await _workWithProject.UpdateProjectInfoAsync();
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
                        SelectedProject.Labels.Remove(SelectedLabel);
                        await _workWithProject.UpdateProjectInfoAsync();
                        UpdatePageControls();
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

        private RelayCommand _deleteProjectCommand;
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
                            UpdateProjectCollection();
                        }                                
                    }, x => SelectedProject != null));
            }
        }

        private void UpdatePageControls()
        {
            if (SelectedProject != null)
            {
                ProjectNameTextBox = SelectedProject.Name;
                DescriptionTextBox = SelectedProject.Description;
                LabelsList = new ObservableCollection<string>(SelectedProject.Labels);
                UpdateIssueCollection();
            }
            else
            {
                ProjectNameTextBox = "";
                DescriptionTextBox = "";
                LabelsList = default;
                IssuesList = default;
            }
        }

        private void UpdateIssueCollection()
        {
            IssuesList = _workWithIssue.CreateCollection(_workWithIssue.GetProjectIssuesList());
        }
        private void UpdateProjectCollection()
        {
            ProjectsList = _workWithProject.CreateCollection();
        }

        private bool HasInfoChangedCheck()
        {
            return !_workWithProject.SelectedProject.Name.Equals(ProjectNameTextBox) && !ProjectNameTextBox.IsNullOrEmpty() ||
                !_workWithProject.SelectedProject.Description.Equals(DescriptionTextBox);
        }
    }
}

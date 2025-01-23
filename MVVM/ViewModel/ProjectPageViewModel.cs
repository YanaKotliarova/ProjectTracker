using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class ProjectPageViewModel : ViewModelBase
    {
        private readonly IWorkWithProject _workWithProject;
        private readonly IWorkWithIssue _workWithIssue;

        public ProjectPageViewModel(IWorkWithProject workWithProject, IWorkWithIssue workWithIssue)
        {
            _workWithProject = workWithProject;
            _workWithIssue = workWithIssue;
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
                        SelectedProject.Name = ProjectNameTextBox;
                        SelectedProject.Description = DescriptionTextBox;

                        _workWithProject.SelectedProject = SelectedProject;
                        await _workWithProject.UpdateProjectInfo();

                        UpdateProjectCollection();
                    }));
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
                        if(!AddLabelTextBox.IsNullOrEmpty())
                        {
                            SelectedProject.Labels.Add(AddLabelTextBox);
                            await _workWithProject.UpdateProjectInfo();
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
                        await _workWithProject.UpdateProjectInfo();
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
                        await _workWithProject.UpdateProjectInfo();
                        UpdatePageControls();
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
                        await _workWithProject.DeleteProject();
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
                    }));
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
                // сделать кнопки неактивными!
            }
        }

        private void UpdateIssueCollection()
        {
            IssuesList = _workWithIssue.CreateCollection();
        }
        private void UpdateProjectCollection()
        {
            ProjectsList = _workWithProject.CreateCollection();
        }
    }
}

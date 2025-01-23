using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AddItemUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithProject _workWithProject;
        private readonly IWorkWithIssue _workWithIssue;

        public AddItemUserControlViewModel(IWorkWithProject workWithProject, IWorkWithIssue workWithIssue)
        {
            _workWithProject = workWithProject;
            _workWithIssue = workWithIssue;
        }

        private string? _nameOfItemTextBox;
        public string? NameOfItemTextBox
        {
            get { return _nameOfItemTextBox; }
            set
            {
                _nameOfItemTextBox = value;
                OnPropertyChanged(nameof(NameOfItemTextBox));
            }
        }

        private string? _descriptionTextBox;
        public string? DescriptionTextBox
        {
            get { return _descriptionTextBox; }
            set
            {
                _descriptionTextBox = value;
                OnPropertyChanged(nameof(DescriptionTextBox));
            }
        }

        private bool _isItProject;
        public bool IsItProject
        {
            get { return _isItProject; }
            set
            {
                _isItProject = value;
                OnPropertyChanged(nameof(IsItProject));
            }
        }

        private bool _isItIssue;
        public bool IsItIssue
        {
            get { return _isItIssue; }
            set
            {
                _isItIssue = value;
                OnPropertyChanged(nameof(IsItIssue));
            }
        }

        private bool _isThereNoItemName;
        public bool IsThereNoItemName
        {
            get { return _isThereNoItemName; }
            set
            {
                _isThereNoItemName = value;
                OnPropertyChanged(nameof(IsThereNoItemName));
            }
        }

        private bool _isThereNoSelectedProject;
        public bool IsThereNoSelectedProject
        {
            get { return _isThereNoSelectedProject; }
            set
            {
                _isThereNoSelectedProject = value;
                OnPropertyChanged(nameof(IsThereNoSelectedProject));
            }
        }

        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
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

        private RelayCommand _createItemCommand;
        public RelayCommand CreateItemCommand
        {
            get
            {
                return _createItemCommand ??
                    (_createItemCommand = new RelayCommand(async obj =>
                    {
                        if (NameOfItemTextBox != null)
                        {
                            IsThereNoItemName = false;
                            if (IsItProject)
                            {
                                await _workWithProject.CreateProjectAsync(NameOfItemTextBox!, DescriptionTextBox);
                                CleanUserControlControls();
                            }
                            else if (IsItIssue)
                            {
                                if (SelectedProject != null)
                                {
                                    IsThereNoSelectedProject = false;
                                    _workWithProject.SelectedProject = SelectedProject;
                                    await _workWithIssue.CreateIssueAsync(NameOfItemTextBox!, DescriptionTextBox);
                                    CleanUserControlControls();
                                }
                                else IsThereNoSelectedProject = true;
                            }
                        }
                        else IsThereNoItemName = true;
                        
                    }));
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
                        CleanUserControlControls();
                    }));
            }
        }

        private void CleanUserControlControls()
        {
            IsItProject = true;
            NameOfItemTextBox = null;
            DescriptionTextBox = null;
            IsThereNoItemName = false;
            IsThereNoSelectedProject = false;
            UpdateListOfUserProjectsNames();
        }

        private void UpdateListOfUserProjectsNames()
        {
            //Projects.Clear();
            Projects = _workWithProject.CreateCollection();
        }
    }
}

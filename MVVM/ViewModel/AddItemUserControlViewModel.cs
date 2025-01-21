using ProjectTracker.MVVM.Core;
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

        private bool _isCreationUnsuccessful;
        public bool IsCreationUnsuccessful
        {
            get { return _isCreationUnsuccessful; }
            set
            {
                _isCreationUnsuccessful = value;
                OnPropertyChanged(nameof(IsCreationUnsuccessful));
            }
        }

        private ObservableCollection<string> _projects = new ObservableCollection<string>();
        public ObservableCollection<string> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        private string _selectedProject;
        public string SelectedProject
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
                    (_createItemCommand = new RelayCommand(obj =>
                    {
                        if (NameOfItemTextBox != null)
                        {
                            IsCreationUnsuccessful = false;
                            if (IsItProject)
                            {
                                _workWithProject.CreateProjectAsync(NameOfItemTextBox!, DescriptionTextBox);
                                CleanUserControlControls();
                            }
                            else if (IsItIssue)
                            {
                                _workWithIssue.CreateIssueAsync(NameOfItemTextBox!, DescriptionTextBox, SelectedProject);
                                CleanUserControlControls();
                            }
                        }
                        else IsCreationUnsuccessful = true;
                        
                    }));
            }
        }

        private RelayCommand _loadUserControlCommand;
        public RelayCommand LoadUserControlCommand
        {
            get
            {
                return _loadUserControlCommand ??
                    (_loadUserControlCommand = new RelayCommand(async obj =>
                    {
                        await CleanUserControlControls();
                    }));
            }
        }

        private async Task CleanUserControlControls()
        {
            IsItProject = true;
            NameOfItemTextBox = null;
            DescriptionTextBox = null;
            IsCreationUnsuccessful = false;
            await UpdateListOfUserProjectsNames();
        }

        private async Task UpdateListOfUserProjectsNames()
        {
            Projects.Clear();
            Projects = _workWithProject.CreateCollection(_workWithProject.GetUserProjectsNames());
        }
    }
}

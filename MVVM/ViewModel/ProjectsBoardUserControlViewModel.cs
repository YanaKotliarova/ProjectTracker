using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class ProjectsBoardUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithProject _workWithProject;

        public ProjectsBoardUserControlViewModel(INavigationService navigationService, IWorkWithProject workWithProject)
        {
            NavigationService = navigationService;
            _workWithProject = workWithProject;
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

        private List<string> _labelsList;
        public List<string> LabelsList
        {
            get { return _labelsList; }
            set
            {
                _labelsList = value; 
                OnPropertyChanged(nameof(LabelsList));
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
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
                        UpdateListOfUserProjects();
                    }));
            }
        }

        private RelayCommand _doubleProjectClickCommand;
        public RelayCommand DoubleProjectClickCommand
        {
            get
            {
                return _doubleProjectClickCommand ??
                    (_doubleProjectClickCommand = new RelayCommand(obj =>
                    {
                        _workWithProject.SelectedProject = SelectedProject;
                        NavigationService.NavigateTo<ProjectPageViewModel>();
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
                        _workWithProject.SelectedProject = SelectedProject;
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
                        UpdateListOfUserProjects();
                    }));
            }
        }

        private void UpdateListOfUserProjects()
        {
            ProjectsList = _workWithProject.CreateCollection();
        }
    }
}

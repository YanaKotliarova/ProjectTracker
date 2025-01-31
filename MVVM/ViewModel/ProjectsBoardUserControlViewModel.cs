using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class ProjectsBoardUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithProject _workWithProject;
        private readonly IMetroDialog _metroDialog;

        public ProjectsBoardUserControlViewModel(INavigationService navigationService, IWorkWithProject workWithProject, IMetroDialog metroDialog)
        {
            NavigationService = navigationService;
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
                        if (await _metroDialog.ShowConfirmationMessage(this,
                            "Are you sure you want to DELETE your project?", "This action is irreversible"))
                        {
                            _workWithProject.SelectedProject = SelectedProject;
                            await _workWithProject.DeleteProject();
                            UpdateListOfUserProjects();
                        }                            
                    }));
            }
        }

        private void UpdateListOfUserProjects()
        {
            ProjectsList = _workWithProject.CreateCollection();
        }
    }
}

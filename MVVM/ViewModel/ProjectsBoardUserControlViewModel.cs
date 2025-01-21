using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class ProjectsBoardUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithProject _workWithProject;

        public ProjectsBoardUserControlViewModel(IWorkWithProject workWithProject)
        {
            _workWithProject = workWithProject;
        }

        private ObservableCollection<Project> _projectsList = new ObservableCollection<Project>();
        public ObservableCollection<Project> ProjectsList
        {
            get { return _projectsList; }
            set
            {
                _projectsList = value;
                OnPropertyChanged(nameof(ProjectsList));
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
                    (_loadUserControlCommand = new RelayCommand(async obj =>
                    {
                        await UpdateListOfUserProjects();
                    }));
            }
        }

        private async Task UpdateListOfUserProjects()
        {
            ProjectsList.Clear();
            ProjectsList = _workWithProject.CreateCollection(_workWithProject.GetUserProjects());
        }
    }
}

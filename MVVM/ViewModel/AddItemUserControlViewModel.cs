using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.ServiceHelpers.Interfaces;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AddItemUserControlViewModel : ViewModelBase
    {
        private readonly IWorkWithProjectService _workWithProject;
        private readonly IWorkWithIssueService _workWithIssue;
        private readonly ICollectionHelper _collectionHelper;

        public AddItemUserControlViewModel(IWorkWithProjectService workWithProject, IWorkWithIssueService workWithIssue, 
            ICollectionHelper collectionHelper)
        {
            _workWithProject = workWithProject;
            _workWithIssue = workWithIssue;
            _collectionHelper = collectionHelper;

            WindowName = Properties.Resources.AddItemLabel;
        }

        private string? _nameOfItemTextBox;
        /// <summary>
        /// A property for binding a created item name and a TextBox for it.
        /// </summary>
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
        /// <summary>
        /// A property for binding a created item description and a TextBox for it.
        /// </summary>
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
        /// <summary>
        /// A property for binding with RadioButton for checking if chosen type of created item is project.
        /// </summary>
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
        /// <summary>
        /// A property for binding with RadioButton for checking if chosen type of created item is issue.
        /// </summary>
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
        /// <summary>
        /// A property for binding a result of cheking if created item name has been entered
        /// and an IsOpen property of certain Popup.
        /// </summary>
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
        /// <summary>
        /// A property for binding a result of cheking if a project was selected when creating the issue 
        /// and an IsOpen property of certain Popup.
        /// </summary>
        public bool IsThereNoSelectedProject
        {
            get { return _isThereNoSelectedProject; }
            set
            {
                _isThereNoSelectedProject = value;
                OnPropertyChanged(nameof(IsThereNoSelectedProject));
            }
        }

        private bool _isThereSameProjectName;
        /// <summary>
        /// A property for binding a result of cheking if entered created project name is exists in database 
        /// and an IsOpen property of certain Popup.
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

        private bool _isThereSameIssueName;
        /// <summary>
        /// A property for binding a result of cheking if entered created issue name is exists in choosen project 
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

        private ObservableCollection<Project> _projects;
        /// <summary>
        /// An observable collection of projects, to list them when creating an issue.
        /// </summary>
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

        private RelayCommand _createItemCommand;
        /// <summary>
        /// The command that is called when button create new item is clicked.
        /// </summary>
        public RelayCommand CreateItemCommand
        {
            get
            {
                return _createItemCommand ??
                    (_createItemCommand = new RelayCommand(async obj =>
                    {
                        if (NameOfItemTextBox != null)
                        {
                            if (IsItProject)
                            {
                                IsThereSameProjectName = await _workWithProject.CheckProjectNameAsync(NameOfItemTextBox);
                                if(!IsThereSameProjectName)
                                {
                                    await _workWithProject.CreateProjectAsync(NameOfItemTextBox!, DescriptionTextBox);
                                    await CleanUserControlControls();
                                }                                
                            }
                            else if (IsItIssue)
                            {
                                if (SelectedProject != null)
                                {
                                    IsThereSameIssueName = await _workWithIssue.ChechIssueNameAsync(SelectedProject.Id, NameOfItemTextBox);
                                    if(!IsThereSameIssueName)
                                    {
                                        IsThereNoSelectedProject = false;
                                        _workWithProject.SelectedProject = SelectedProject;
                                        await _workWithIssue.CreateIssueAsync(NameOfItemTextBox!, DescriptionTextBox);
                                        await CleanUserControlControls();
                                    }                                    
                                }
                                else IsThereNoSelectedProject = true;
                            }
                        }
                        else IsThereNoItemName = true;
                        
                    }));
            }
        }

        private RelayCommand _loadUserControlCommand;
        /// <summary>
        /// The command that is called when the user control loads to update the controls.
        /// </summary>
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

        /// <summary>
        /// The method for cleaning user control controls.
        /// </summary>
        /// <returns></returns>
        private async Task CleanUserControlControls()
        {
            IsItProject = true;
            NameOfItemTextBox = null;
            DescriptionTextBox = null;
            IsThereNoItemName = false;
            IsThereNoSelectedProject = false;
            await UpdateListOfUserProjectsNames();
        }

        /// <summary>
        /// The method for updating collection of projects.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateListOfUserProjectsNames()
        {
            Projects = _collectionHelper.CreateCollection<Project>(await _workWithProject.GetUserProjectsListAsync());
        }
    }
}

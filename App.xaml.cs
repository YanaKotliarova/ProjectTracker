using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.Data;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.Pages;
using ProjectTracker.MVVM.View.UI;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.MVVM.View.UIHelpers;
using ProjectTracker.MVVM.View.UIHelpers.Interfaces;
using ProjectTracker.MVVM.ViewModel;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Navigation;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.WorkWithItems;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Windows;

namespace ProjectTracker
{
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<AutorizationPageViewModel>();
            services.AddSingleton<RegistrationPageViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<AccountPageViewModel>();
            services.AddSingleton<ProjectPageViewModel>();
            services.AddSingleton<IssuePageViewModel>();
            services.AddSingleton<EnterConnectionStringViewModel>();

            services.AddSingleton<HomeUserControlViewModel>();
            services.AddSingleton<ProjectsBoardUserControlViewModel>();
            services.AddSingleton<IssueBoardUserControlViewModel>();
            services.AddSingleton<AddItemUserControlViewModel>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IAutorizationService, AutorizationService>();
            services.AddSingleton<IRegistrationService, RegistrationService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IWorkWithProjectService, WorkWithProjectService>();
            services.AddSingleton<IWorkWithIssueService, WorkWithIssueService>();

            services.AddSingleton<IDialogCoordinator, DialogCoordinator>();
            services.AddSingleton<IMetroDialog, MetroDialog>();

            services.AddSingleton<IMainView, MainView>();

            services.AddSingleton<IConnectionStringValidation, ConnectionStringValidation>();

            services.AddScoped<IRepository, DataBase>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();

            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => 
            viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Window mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        public static IServiceProvider GetServiceProvider() 
        { 
            return _serviceProvider; 
        }
    }

}

using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.Data;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.Pages;
using ProjectTracker.MVVM.View.UI;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.MVVM.ViewModel;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.Authentication.CurrentUser;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Localization;
using ProjectTracker.Services.Navigation;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.ServiceHelpers;
using ProjectTracker.Services.ServiceHelpers.Interfaces;
using ProjectTracker.Services.WorkWithItems;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Configuration;
using System.Windows;

namespace ProjectTracker
{
    public partial class App : Application
    {
        private const string DefaultConnection = "DefaultConnection";

        private static IServiceProvider _serviceProvider;
        public App()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;

            IServiceCollection services = new ServiceCollection();

            services.AddScoped<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });

            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<AutorizationPageViewModel>();
            services.AddScoped<RegistrationPageViewModel>();
            services.AddScoped<HomePageViewModel>();
            services.AddScoped<AccountPageViewModel>();
            services.AddScoped<ProjectPageViewModel>();
            services.AddScoped<IssuePageViewModel>();
            services.AddScoped<EnterConnectionStringViewModel>();

            services.AddScoped<HomeUserControlViewModel>();
            services.AddScoped<ProjectsBoardUserControlViewModel>();
            services.AddScoped<IssueBoardUserControlViewModel>();
            services.AddScoped<AddItemUserControlViewModel>();

            services.AddScoped<INavigationService, NavigationService>();

            services.AddScoped<IAutorizationService, AutorizationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWorkWithProjectService, WorkWithProjectService>();
            services.AddScoped<IWorkWithIssueService, WorkWithIssueService>();

            services.AddScoped<ICollectionHelper, CollectionHelper>();

            services.AddScoped<IDialogCoordinator, DialogCoordinator>();
            services.AddScoped<IMetroDialog, MetroDialog>();

            services.AddScoped<IConnectionStringValidation, ConnectionStringValidation>();

            services.AddDbContext<ApplicationContext>();

            services.AddScoped<IRepository, DataBase>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();

            services.AddScoped<Func<Type, ViewModelBase>>(serviceProvider =>
            viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

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

using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.Data;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.Pages;
using ProjectTracker.MVVM.ViewModel;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Navigation;
using ProjectTracker.Services.Navigation.Interfaces;
using System.Windows;

namespace ProjectTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
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

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IAutorization, Autorization>();
            services.AddSingleton<IRegistration, Registration>();

            services.AddSingleton<IRepository, DataBase>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IProjectRepository, ProjectRepository>();
            services.AddSingleton<IIssueRepository, IssueRepository>();

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
    }

}

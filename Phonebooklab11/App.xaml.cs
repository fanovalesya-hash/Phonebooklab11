using Phonebooklab11.Services;
using Phonebooklab11.View;
using Phonebooklab11.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;


namespace Phonebooklab11
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<ContactEditViewModel>();

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>(sp => {
                var window = new MainWindow();
                window.DataContext = sp.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            var sp = services.BuildServiceProvider();
            sp.GetRequiredService<MainWindow>().Show();
        }

    }
}

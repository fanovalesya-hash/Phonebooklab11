using Phonebooklab11.Services;
using Phonebooklab11.View;
using Phonebooklab11.ViewModels;
using Microsoft.EntityFrameworkCore;
using Phonebooklab11.Models;
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

            services.AddDbContext<PhoneBookDbNasenkinaOeContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PhoneBookDB_NasenkinaOE;Integrated Security=True;TrustServerCertificate=True"));

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>(sp =>
            {
                var window = new MainWindow();
                window.DataContext = sp.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();

            var sp = services.BuildServiceProvider();
            sp.GetRequiredService<MainWindow>().Show();
        }
    }
}


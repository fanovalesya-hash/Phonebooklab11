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

            // 1. Сервисы (Singleton)
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ✅ 2. РЕГИСТРАЦИЯ БАЗЫ ДАННЫХ (DbContext)
            // Замени PhoneBookDB_NasenkinaOEContext на точное имя твоего класса контекста
            // И вставь свою строку подключения из метода OnConfiguring
            services.AddDbContext<PhoneBookDbNasenkinaOeContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PhoneBookDB_NasenkinaOE;Integrated Security=True;TrustServerCertificate=True"));

            // 3. Оболочка (Singleton)
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>(sp =>
            {
                var window = new MainWindow();
                window.DataContext = sp.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            // 4. Экраны (Transient)
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();

            var sp = services.BuildServiceProvider();
            sp.GetRequiredService<MainWindow>().Show();
        }
    }
}


using Phonebooklab11.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Phonebooklab11.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public INavigationService NavigationService => _navigationService;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            ShowContactsCommand = new RelayCommand(() => _navigationService.NavigateTo<ContactsListViewModel>());
            ShowAboutCommand = new RelayCommand(() => _navigationService.NavigateTo<AboutViewModel>());

            _navigationService.NavigateTo<ContactsListViewModel>();
        }

        public ICommand ShowContactsCommand { get; }
        public ICommand ShowAboutCommand { get; }
    }
}

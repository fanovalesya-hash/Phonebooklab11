using Phonebooklab11.Models;
using Phonebooklab11.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Phonebooklab11.ViewModels
{
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigation;
        private readonly IDialogService _dialogService;
        private readonly PhoneBookDbNasenkinaOeContext _context;
        private Contact? _contact;
        private bool _isNewContact;

        public ContactEditViewModel(
            INavigationService navigation,
            IDialogService dialogService,
            PhoneBookDbNasenkinaOeContext context)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dialogService = dialogService;

            SaveCommand = new RelayCommand(SaveContact);
            CancelCommand = new RelayCommand(CancelEdit);
        }

        public string EditName
        {
            get => _contact?.Name ?? string.Empty;
            set
            {
                if (_contact != null && _contact.Name != value)
                {
                    _contact.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EditPhone
        {
            get => _contact?.Phone ?? string.Empty;
            set
            {
                if (_contact != null && _contact.Phone != value)
                {
                    _contact.Phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is Contact contact)
            {
                _contact = contact;
                _isNewContact = false;
                OnPropertyChanged(nameof(EditName));
                OnPropertyChanged(nameof(EditPhone));
            }
            else
            {
                _contact = new Contact();
                _isNewContact = true;
                OnPropertyChanged(nameof(EditName));
                OnPropertyChanged(nameof(EditPhone));
            }
        }

        private void SaveContact()
        {
            try
            {
                if (_contact == null)
                {
                    _navigation.NavigateTo<ContactsListViewModel>();
                    return;
                }

                if (_isNewContact)
                {
                    _context.Contacts.Add(_contact);
                }
                else
                {
                    _context.Contacts.Update(_contact);
                }

                _context.SaveChanges();
                _dialogService.ShowInfo("Контакт сохранён!");
                _navigation.NavigateTo<ContactsListViewModel>();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void CancelEdit()
        {
            _navigation.NavigateTo<ContactsListViewModel>();
        }
    }
}

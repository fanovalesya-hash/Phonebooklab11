using Phonebooklab11.Models;
using Phonebooklab11.Services;
using System;
using System.Collections.Generic;
using System.Text;
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

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
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

        public ContactEditViewModel(
            INavigationService navigation,
            IDialogService dialogService,
            PhoneBookDbNasenkinaOeContext context)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            SaveCommand = new RelayCommand(SaveContact);
            CancelCommand = new RelayCommand(CancelEdit);
        }

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
                _contact = null;
                _isNewContact = true;
                Name = string.Empty;
                Phone = string.Empty;
            }
        }

        private void SaveContact()
        {
            try
            {
                if (_isNewContact)
                {
                    if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
                    {
                        _dialogService.ShowWarning("Заполните все поля!");
                        return;
                    }

                    if (_context.Contacts.Any(c => c.Phone == Phone))
                    {
                        _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                        return;
                    }

                    var newContact = new Contact
                    {
                        Name = Name,
                        Phone = Phone
                    };

                    _context.Contacts.Add(newContact);
                    _context.SaveChanges();

                    _dialogService.ShowInfo("Контакт успешно создан!");
                }
                else
                {
                    _context.SaveChanges();

                    _dialogService.ShowInfo("Изменения сохранены!");
                }

                _navigation.NavigateTo<ContactsListViewModel>();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void CancelEdit()
        {
            _navigation.NavigateTo<ContactsListViewModel>();
        }
    }
}

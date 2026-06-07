using Phonebooklab11.Models;
using Phonebooklab11.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Phonebooklab11.ViewModels
{
    public class ContactsListViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigation;


        // Коллекция контактов
        public ObservableCollection<Contact> Contacts { get; }
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

        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }


        public ICommand EditContactCommand { get; }
        private void EditContact()
        {
            if (SelectedContact != null)
            {
                _navigation.NavigateTo<ContactEditViewModel>(SelectedContact);
            }
        }
        private bool CanEditContact() => SelectedContact != null;

        // Команды
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ContactsListViewModel(IDialogService dialogService, INavigationService navigation)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            Contacts = new ObservableCollection<Contact>();
            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand<Contact?>(DeleteContact, CanDeleteContact);
            EditContactCommand = new RelayCommand(EditContact, CanEditContact);
        }
        private void AddContact()
        {
            if (Contacts.Any(c => c.Phone == _phone))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                return;
            }
            try
            {
                var newContact = new Contact(Name, Phone);
                Contacts.Add(newContact);
                Name = string.Empty;
                Phone = string.Empty;
                _dialogService.ShowInfo("Контакт успешно добавлен!");
            }
            catch
            {
                _dialogService.ShowError("Ошибка при добавлении контакта (проверьте формат номера).");
            }
        }
        private bool CanAddContact()
        {
            return Contact.Validate(Name, Phone);
        }
        private void DeleteContact(Contact? contact)
        {
            if (contact == null) return;
            bool result = _dialogService.ShowConfirmation(
                $"Удалить контакт {contact.Name}?",
                "Удаление");
            if (result)
            {
                Contacts.Remove(contact);
                SelectedContact = null;
            }
        }
        private bool CanDeleteContact(Contact? contact)
        {
            return contact != null;
        }
    }
}

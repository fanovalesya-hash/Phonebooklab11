using Microsoft.EntityFrameworkCore;
using Phonebooklab11.Models;
using Phonebooklab11.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Phonebooklab11.ViewModels
{
    public class ContactsListViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigation;
        private readonly PhoneBookDbNasenkinaOeContext _context;

        private readonly List<Contact> _allContacts;
        public ObservableCollection<Contact> Contacts { get; }

        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (Set(ref _searchText, value))
                {
                    FilterContacts();
                }
            }
        }

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

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditContactCommand { get; }

        public ContactsListViewModel(
            IDialogService dialogService,
            INavigationService navigation,
            PhoneBookDbNasenkinaOeContext context)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _allContacts = _context.Contacts.ToList();
            Contacts = new ObservableCollection<Contact>(_allContacts);

            AddCommand = new RelayCommand(AddContact);
            DeleteCommand = new RelayCommand<Contact?>(DeleteContact, CanDeleteContact);
            EditContactCommand = new RelayCommand(EditContact, CanEditContact);
        }

        private void FilterContacts()
        {
            Contacts.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allContacts
                : _allContacts.Where(c =>
                    c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var contact in filtered)
            {
                Contacts.Add(contact);
            }
        }

        private void AddContact()
        {
            _navigation.NavigateTo<ContactEditViewModel>(null);
        }

        private void EditContact()
        {
            if (SelectedContact != null)
            {
                _navigation.NavigateTo<ContactEditViewModel>(SelectedContact);
            }
        }
        private bool CanEditContact() => SelectedContact != null;

        private void DeleteContact(Contact? contact)
        {
            if (contact == null) return;

            bool result = _dialogService.ShowConfirmation(
                $"Удалить контакт {contact.Name}?",
                "Удаление");

            if (result)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges(); // DELETE

                _allContacts.Remove(contact);
                Contacts.Remove(contact);
                SelectedContact = null;
            }
        }
        private bool CanDeleteContact(Contact? contact) => contact != null;
    }
}

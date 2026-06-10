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
        private readonly PhoneBookDbNasenkinaOeContext _context; // ← Добавь это поле



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
        public ContactsListViewModel(
                IDialogService dialogService,
                INavigationService navigation,
                PhoneBookDbNasenkinaOeContext context) // ← Добавь этот параметр!
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            Contacts = new ObservableCollection<Contact>(_context.Contacts.ToList());

            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand<Contact?>(DeleteContact, CanDeleteContact);
            EditContactCommand = new RelayCommand(EditContact, CanEditContact);
        }
        private void AddContact()
        {
            // Проверка на дубликат
            if (_context.Contacts.Any(c => c.Phone == Phone))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                return;
            }

            try
            {
                // Создаем контакт через объектную инициализацию (не через конструктор!)
                var newContact = new Contact
                {
                    Name = Name,
                    Phone = Phone
                };

                _context.Contacts.Add(newContact);
                _context.SaveChanges(); // Сохраняем в БД!

                // Обновляем ObservableCollection
                Contacts.Add(newContact);

                Name = string.Empty;
                Phone = string.Empty;
                _dialogService.ShowInfo("Контакт успешно добавлен!");
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при добавлении контакта: {ex.Message}");
            }
        }

        private bool CanAddContact()
        {
            // Простая проверка на пустоту (вместо Contact.Validate)
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }
        private void DeleteContact(Contact? contact)
        {
            if (contact == null) return;

            bool result = _dialogService.ShowConfirmation(
                $"Удалить контакт {contact.Name}?",
                "Удаление");

            if (result)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges(); // Сохраняем изменения в БД!

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

using Phonebooklab11.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phonebooklab11.Models
{
    public class Contact : ObservableObject
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        public Contact(string name, string phone)
        {
            if (!Validate(name, phone))
            {
                throw new ArgumentException("Некорректные данные контакта");
            }
            _name = name;
            _phone = phone;
        }
        public string Name
        {
            get => _name;
            set { Set(ref _name, value); }
        }
        public string Phone
        {
            get => _phone;
            set { Set(ref _phone, value); }
        }
        public static bool Validate(string name, string phone)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            string cleanPhone = phone.Replace(" ", "").Replace("-", "")
                                     .Replace("(", "").Replace(")", "")
                                     .Replace("+", "");
            string justDigits = new string(cleanPhone.Where(char.IsDigit).ToArray());
            if (phone.Trim().StartsWith("+7"))
            {
                return justDigits.Length == 11;
            }
            return justDigits.Length == 10;
        }
    }
}

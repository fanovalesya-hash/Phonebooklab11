using System;
using System.Collections.Generic;
using System.Text;

namespace Phonebooklab11.ViewModels
{
    public class AboutViewModel
    {
        public string AppName => "Телефонная книга MVVM";
        public string Version => "Версия 2.0 (Навигация + DI)";
        public string Description => "Пример реализации Shell-архитектуры и ViewModel-First навигации в WPF.";
    }
}

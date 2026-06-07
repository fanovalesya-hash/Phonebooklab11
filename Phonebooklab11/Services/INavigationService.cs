using System;
using System.Collections.Generic;
using System.Text;

namespace Phonebooklab11.Services
{
    public interface INavigationService
    {
        object? CurrentViewModel { get; }
        void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class;
    }
}

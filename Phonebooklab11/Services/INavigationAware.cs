using System;
using System.Collections.Generic;
using System.Text;

namespace Phonebooklab11.Services
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object? parameter);
    }
}

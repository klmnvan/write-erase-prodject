using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WriteErase.ViewModels
{
    partial class AuthVM : ViewModelBase
    {
        [ObservableProperty]
        private string _userLogin = ""; 
        [ObservableProperty]
        private string _userPassword = "";

        public AuthVM()
        {
            
        }

        [RelayCommand]
        public void GoListProduct()
        {
            MainWindowViewModel.Instance.CurrentPage = new ListProductPage();
        }
    }
}

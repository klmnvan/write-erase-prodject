using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using System.Linq;

namespace WriteErase.ViewModels
{
    partial class AuthVM : ViewModelBase
    {
        [ObservableProperty]
        private string _userLogin = "loginDEjrm2018"; 
        [ObservableProperty]
        private string _userPassword = "Cpb+Im";

        public AuthVM()
        {
            
        }

        [RelayCommand]
        public void GoListProduct()
        {
            MainWindowViewModel.Instance.CurrentPage = new ListProductPage(new Models.Order());
        }

        [RelayCommand]
        public async void TryAuth()
        {
            if(UserLogin != "" && UserPassword != "")
            {

                if (MainWindowViewModel.Context.Users.Any(it => it.Login == UserLogin))
                {
                    var user = MainWindowViewModel.Context.Users.First(it => it.Login == UserLogin);
                    if (user.Password == UserPassword)
                    {
                        MainWindowViewModel.Instance.CurrentPage = new ListProductPage(user);
                    }
                    else MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пароль неверный!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
                }
                else MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Пользователь с логином {UserLogin} в системе не зарегистрирован!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            }
            else MessageBoxManager.GetMessageBoxStandard("Ошибка", "Не все поля заполнены!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }
    }
}

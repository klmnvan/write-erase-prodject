using CommunityToolkit.Mvvm.Input;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class OrderManagementVM : ViewModelBase
    {
        private User _currentUser = new();

        public OrderManagementVM(User u)
        {
            _currentUser = u;
        }

        [RelayCommand]
        public void GoBack()
        {
            MainWindowViewModel.Instance.CurrentPage = new ListProductPage(_currentUser);
        }
    }
}

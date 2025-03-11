using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public static TradeContext Context = new TradeContext();
        public static MainWindowViewModel Instance;
        [ObservableProperty]
        private UserControl _currentPage;

        public MainWindowViewModel()
        {
            Instance = this;
            CurrentPage = new AuthPage();
        }

    }
}

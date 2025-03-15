using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class OrderManagementVM : ViewModelBase
    {
        private User _currentUser = new();
        [ObservableProperty]
        private List<Order> _orders = new();
        [ObservableProperty]
        private List<string> _listSortedType = new()
        {
            "Без сортировки",
            "По возрастанию стоимости",
            "По убыванию стоимости",
        };
        private int _selectedSortedType = 0;
        [ObservableProperty]
        private List<string> _listFilterType = new()
        {
            "Все диапазоны",
            "0-9,99%",
            "10-14,99%",
            "15% и более",
        };
        private int _selectedFilterType = 0;
        public int SelectedSortedType
        {
            get => _selectedSortedType;
            set
            {
                SetProperty(ref _selectedSortedType, value);
                Filter();
            }
        }
        public int SelectedFilterType
        {
            get => _selectedFilterType;
            set
            {
                SetProperty(ref _selectedFilterType, value);
                Filter();
            }
        }

        public OrderManagementVM(User u)
        {
            _currentUser = u;
            InitData();
        }

        void Filter()
        {
            InitData();
            if (_selectedFilterType != 0)
            {
                if (_selectedFilterType == 1)
                    Orders = Orders
                        .Where(it => it.AllDiscount >= 0 && it.AllDiscount < 10)
                        .ToList();

                if (_selectedFilterType == 2)
                    Orders = Orders
                        .Where(it => it.AllDiscount >= 10 && it.AllDiscount < 14)
                        .ToList();

                if (_selectedFilterType == 3)
                    Orders = Orders
                        .Where(it => it.AllDiscount >= 15)
                        .ToList();

            }
            if (_selectedSortedType != 0)
            {
                if (_selectedSortedType == 1) Orders = Orders.OrderBy(it => it.AllCost).ToList();

                if (_selectedSortedType == 2) Orders = Orders.OrderByDescending(it => it.AllCost).ToList();

            }
        }

        private void InitData()
        {
            Orders = MainWindowViewModel.Context.Orders
                .Include(it => it.OrderProducts)
                .ThenInclude(it => it.ProductArticleNumberNavigation)
                .Include(it => it.IdUserNavigation)
                .Include(it => it.StatusNavigation)
                .Where(it => it.Status != null)
                .ToList();
        }


        [RelayCommand]
        public void GoBack()
        {
            MainWindowViewModel.Instance.CurrentPage = new ListProductPage(_currentUser);
        }

        [RelayCommand]
        public void EditOrder(Order o)
        {
            MainWindowViewModel.Instance.CurrentPage = new UpdateOrderPage(o, _currentUser);
        }
    }
}

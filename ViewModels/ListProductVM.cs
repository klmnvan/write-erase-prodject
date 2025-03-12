using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using System.Collections.Generic;
using System.Linq;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class ListProductVM : ViewModelBase
    {
        private List<Product> _products = new();
        [ObservableProperty]
        private List<Product> _productsPreview = new();
        [ObservableProperty]
        private User _currentUser = new();
        [ObservableProperty]
        private string _messageCountProduct = "";
        private string _search = "";
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

        public ListProductVM()
        {
            InitData();
        }

        public ListProductVM(User currentUser)
        {
            InitData();
            CurrentUser = currentUser;
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                Filter();
            }
        }

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

        void Filter()
        {
            ProductsPreview = _products;
            if (_selectedFilterType != 0)
            {
                if (_selectedFilterType == 1)
                    ProductsPreview = ProductsPreview
                        .Where(it => it.CurrentDiscount >= 0 && it.CurrentDiscount < 10)
                        .ToList();
                
                if (_selectedFilterType == 2)
                    ProductsPreview = ProductsPreview
                        .Where(it => it.CurrentDiscount >= 10 && it.CurrentDiscount < 14)
                        .ToList();
                
                if (_selectedFilterType == 3)
                    ProductsPreview = ProductsPreview
                        .Where(it => it.CurrentDiscount >= 15)
                        .ToList();
                
            }
            if (_selectedSortedType != 0)
            {
                if(_selectedSortedType == 1) ProductsPreview = ProductsPreview.OrderBy(it => it.Cost).ToList();
                
                if (_selectedSortedType == 2) ProductsPreview = ProductsPreview.OrderByDescending(it => it.Cost).ToList();
                
            }
            if(Search != "")
            {
                ProductsPreview = ProductsPreview.Where(it => it.Name.ToLower().Contains(_search.ToLower())).ToList();
            }
            MessageCountProduct = $"{ProductsPreview.Count} из {_products.Count}";
        }

        public void InitData()
        {
            ProductsPreview = MainWindowViewModel.Context.Products
                .Include(it => it.SupplierNavigation)
                .Include(it => it.ManufacturerNavigation)
                .Include(it => it.UnitNavigation)
                .Include(it => it.CategoryNavigation)
                .ToList();
            _products = ProductsPreview;
            MessageCountProduct = $"{ProductsPreview.Count} из {ProductsPreview.Count}";
        }

        [RelayCommand]
        public async void Test(Product p)
        {
            MessageBoxManager.GetMessageBoxStandard("тест", "тест", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }
    }
}

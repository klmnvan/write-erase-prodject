using Avalonia.Controls;
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
        [ObservableProperty]
        private bool _buttonIsVisible = false;
        [ObservableProperty]
        private bool _helloTextIsVisible = false;
        [ObservableProperty]
        private bool _buttonManageIsVisible = false;
        private Order _order = new();
        public ListProductVM(Order order)
        {
            _order = order;
            InitData();
            if(_order.OrderProducts.Count > 0) ButtonIsVisible = true;
        }

        public ListProductVM(User currentUser)
        {
            InitData();
            CurrentUser = currentUser;
            HelloTextIsVisible = true;
            //Проверяем, есть ли у текущего пользователя незавершённые заказы
            if (!MainWindowViewModel.Context.Orders.Any(it => it.Status == 1 && it.IdUser == CurrentUser.Id))
            {
                //если нет, создаём
                MainWindowViewModel.Context.Orders.Add(new() { Status = 1, IdUser = CurrentUser.Id });
                MainWindowViewModel.Context.SaveChanges();
            }
            var order = MainWindowViewModel.Context.Orders.Include(it => it.OrderProducts).First(it => it.Status == 1 && it.IdUser == CurrentUser.Id);
            if (order.OrderProducts.Count > 0) ButtonIsVisible = true;
            if(currentUser.RoleId == 2 || currentUser.RoleId == 3)
            {
                ButtonManageIsVisible = true;
            }
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
        public async void AddToOrder(Product p)
        {
            //Проверка на гостя
            if(CurrentUser.Id != 0)
            {
                //получаем заказ пользователя
                var order = MainWindowViewModel.Context.Orders.Include(it => it.OrderProducts).First(it => it.Status == 1 && it.IdUser == CurrentUser.Id);
                //получаем все товары из заказа
                var orderProducts = order.OrderProducts.ToList();
                //Если этот товар есть, увеличиваем на единицу количество
                if(orderProducts.Any(it => it.ProductArticleNumber == p.ArticleNumber))
                {
                    var productInOrder = orderProducts.First(it => it.ProductArticleNumber == p.ArticleNumber);
                    productInOrder.Count++;
                }
                //Если товара нет, добавляем его с количеством 1
                else
                {
                    MainWindowViewModel.Context.OrderProducts.Add(new() { ProductArticleNumber = p.ArticleNumber, OrderId = order.Id, Count = 1 });
                }
                MainWindowViewModel.Context.SaveChanges();
                MessageBoxManager.GetMessageBoxStandard("Сообщение", $"К заказу был добавлен товар {p.Name} в количестве одной единицы", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            }
            //у гостя работаем с объектом, которого ещё нет в БД
            else
            {
                //Если этот товар есть, увеличиваем на единицу количество
                if (_order.OrderProducts.Any(it => it.ProductArticleNumber == p.ArticleNumber))
                {
                    _order.OrderProducts.First(it => it.ProductArticleNumberNavigation == p).Count++;
                }
                //Если товара нет, добавляем его с количеством 1
                else
                {
                    _order.OrderProducts.Add(new() { ProductArticleNumberNavigation = p, Order = _order, Count = 1 });
                }
            }
            ButtonIsVisible = true;
        }

        [RelayCommand]
        public async void GoToOrder()
        {
            if(CurrentUser.Id != 0)
            {
                var order = MainWindowViewModel.Context.Orders
                    .Include(it => it.OrderProducts)
                    .Include(it => it.IdUserNavigation)
                    .Include(it => it.PickUpNavigation)
                    .Include(it => it.StatusNavigation)
                    .First(it => it.Status == 1 && it.IdUser == CurrentUser.Id);
                MainWindowViewModel.Instance.CurrentPage = new CreateOrderPage(order);
            }
            else
            {
                MainWindowViewModel.Instance.CurrentPage = new CreateOrderPage(_order);
            }
        }

        [RelayCommand]
        public void GoToManageOrder()
        {
            MainWindowViewModel.Instance.CurrentPage = new OrderManagementPage(_currentUser);
        }
    }
}

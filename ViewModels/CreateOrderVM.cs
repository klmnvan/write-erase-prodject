using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class CreateOrderVM : ViewModelBase
    {
        [ObservableProperty]
        private List<OrderProduct> _productsInOrder = new();
        [ObservableProperty]
        private User _currentUser = new();
        private Order _currentOrder = new();
        [ObservableProperty]
        private List<PickUpPoint> _pickUpPoints = new();
        [ObservableProperty]
        private PickUpPoint _selectedPoint = new();
        [ObservableProperty]
        private double _fullCost = 0;
        [ObservableProperty]
        private double _fullDiscount = 0;

        public CreateOrderVM(Order order)
        {
            var user = order.IdUserNavigation;
            _currentOrder = order;
            if (user != null)
            {
                CurrentUser = user;
                InitData();
            }
            else ProductsInOrder = order.OrderProducts.ToList();
            PickUpPoints = MainWindowViewModel.Context.PickUpPoints.ToList();
            SelectedPoint = PickUpPoints.FirstOrDefault();
            UpdateCost();
        }

        private void InitData()
        {
            ProductsInOrder = MainWindowViewModel.Context.OrderProducts
                .Include(it => it.ProductArticleNumberNavigation)
                .ThenInclude(it => it.SupplierNavigation)
                .Include(it => it.ProductArticleNumberNavigation)
                .ThenInclude(it => it.ManufacturerNavigation)
                .Include(it => it.ProductArticleNumberNavigation)
                .ThenInclude(it => it.CategoryNavigation)
                .Include(it => it.ProductArticleNumberNavigation)
                .ThenInclude(it => it.UnitNavigation)
                .Where(it => it.OrderId == _currentOrder.Id)
                .OrderBy(it => it.ProductArticleNumberNavigation.Name)
                .ToList();
        }

        private void UpdateCost()
        {
            _fullCost = 0;
            FullDiscount = 0;
            double cost = 0;
            ProductsInOrder.ForEach(it =>
            {
                FullCost += (double)(it.Count * ((it.ProductArticleNumberNavigation.Cost / 100) * (100 - it.ProductArticleNumberNavigation.CurrentDiscount)));
                cost += (double)(it.Count * it.ProductArticleNumberNavigation.Cost);
            });
            if(cost > FullCost) FullDiscount = (1.0 - (FullCost / cost)) * 100;
        }

        [RelayCommand]
        public void AddUnit(OrderProduct op)
        {
            if (CurrentUser.Id != 0)
            {
                op.Count++;
                MainWindowViewModel.Context.SaveChanges();
                InitData();
            }
            else
            {
                string art = op.ProductArticleNumberNavigation.ArticleNumber.ToString();
                _currentOrder.OrderProducts.First(it => it.ProductArticleNumberNavigation.ArticleNumber == art).Count++;
                ProductsInOrder = _currentOrder.OrderProducts.ToList();
            }
            UpdateCost();
        }

        [RelayCommand]
        public void DeleteUnit(OrderProduct op)
        {
            if (CurrentUser.Id != 0)
            {
                if (op.Count == 1) MainWindowViewModel.Context.OrderProducts.Remove(op);
                else op.Count--;
                MainWindowViewModel.Context.SaveChanges();
                InitData();
            }
            else
            {
                if (op.Count == 1) _currentOrder.OrderProducts.Remove(op);
                else 
                {
                    string art = op.ProductArticleNumberNavigation.ArticleNumber.ToString();
                    _currentOrder.OrderProducts.First(it => it.ProductArticleNumberNavigation.ArticleNumber == art).Count--;
                }
                ProductsInOrder = _currentOrder.OrderProducts.ToList();
            }
            UpdateCost();
        }

        [RelayCommand]
        public void DeleteProduct(OrderProduct op)
        {
            if (CurrentUser.Id != 0)
            {
                MainWindowViewModel.Context.OrderProducts.Remove(op);
                MainWindowViewModel.Context.SaveChanges();
                InitData();
            }
            else
            {
                _currentOrder.OrderProducts.Remove(op);
                ProductsInOrder = _currentOrder.OrderProducts.ToList();
            }
            UpdateCost();
        }

        [RelayCommand]
        public void GoBack()
        {
            if(_currentUser.Id == 0) MainWindowViewModel.Instance.CurrentPage = new ListProductPage(_currentOrder);
            else
            {
                MainWindowViewModel.Instance.CurrentPage = new ListProductPage(_currentUser);
            }
        }

        [RelayCommand]
        public void CreateOrder()
        {
            _currentOrder.OrderProducts = ProductsInOrder;
            _currentOrder.DateOrder = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _currentOrder.PickUpNavigation = SelectedPoint;
            _currentOrder.Code = MainWindowViewModel.Context.Orders.Select(it => it.Code).Max() + 1;
            _currentOrder.Status = 2;
            if (_currentUser.Id != 0) _currentOrder.IdUserNavigation = CurrentUser;
            else MainWindowViewModel.Context.Orders.Add(_currentOrder);
            MainWindowViewModel.Context.SaveChanges();
            if(_currentUser.Id == 0) MainWindowViewModel.Instance.CurrentPage = new ListProductPage(new Order());
            else MainWindowViewModel.Instance.CurrentPage = new ListProductPage(CurrentUser);
        }
    }
}

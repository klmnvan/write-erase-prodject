using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class UpdateOrderVM : ViewModelBase
    {
        private User _user = new();
        [ObservableProperty]
        private Order _order = new();
        [ObservableProperty]
        private List<Status> _listStatus= new();
        [ObservableProperty]
        private Status _selectedStatus = new();
        DateTime _serviceDateTime = DateTime.Now;
        public DateTime ServiceDateTime
        {
            get => _serviceDateTime;
            set
            {
                SetProperty(ref _serviceDateTime, value);
                Order.DateDelivery = new DateOnly(_serviceDateTime.Year, _serviceDateTime.Month, _serviceDateTime.Day);
            }
        }
        public UpdateOrderVM(Order o, User u)
        {
            _user = u;
            _order = o;
            ListStatus = MainWindowViewModel.Context.Statuses.ToList();
            SelectedStatus = o.StatusNavigation;
            if(o.DateDelivery != null)
            {
                string dateString = o.DateDelivery.ToString(); // Предположим, что это строка
                DateOnly dateOnly = DateOnly.Parse(dateString); // Преобразование строки в DateOnly
                ServiceDateTime = dateOnly.ToDateTime(TimeOnly.MinValue); // Преобразование в DateTime
            }
            else ServiceDateTime = DateTime.Now;
        }

        [RelayCommand]
        public void Save()
        {
            Order.StatusNavigation = SelectedStatus;
            MainWindowViewModel.Context.SaveChanges();
            MainWindowViewModel.Instance.CurrentPage = new OrderManagementPage(_user);
        }

        [RelayCommand]
        public void GoBack()
        {
            MainWindowViewModel.Instance.CurrentPage = new OrderManagementPage(_user);
        }
    }
}

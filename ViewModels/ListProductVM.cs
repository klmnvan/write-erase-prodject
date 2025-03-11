using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class ListProductVM : ViewModelBase
    {
        [ObservableProperty]
        private List<Product> _products = new();
        public ListProductVM()
        {
            Products = MainWindowViewModel.Context.Products
                .Include(it => it.SupplierNavigation)
                .Include(it => it.ManufacturerNavigation)
                .Include(it => it.UnitNavigation)
                .Include(it => it.CategoryNavigation)
                .ToList();
        }
    }
}

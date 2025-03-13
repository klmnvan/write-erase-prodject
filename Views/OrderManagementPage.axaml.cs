using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.Models;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class OrderManagementPage : UserControl
{
    public OrderManagementPage(User u)
    {
        DataContext = new OrderManagementVM(u);
        InitializeComponent();
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.Models;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class CreateOrderPage : UserControl
{
    public CreateOrderPage(Order order)
    {
        DataContext = new CreateOrderVM(order);
        InitializeComponent();
    }
}
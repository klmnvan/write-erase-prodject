using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.Models;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class ListProductPage : UserControl
{
    public ListProductPage(Order order)
    {
        DataContext = new ListProductVM(order);
        InitializeComponent();
    }
    public ListProductPage(User user)
    {
        DataContext = new ListProductVM(user);
        InitializeComponent();
    }
}
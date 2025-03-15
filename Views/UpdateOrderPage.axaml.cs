using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.Models;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class UpdateOrderPage : UserControl
{
    public UpdateOrderPage(Order o, User u)
    {
        DataContext = new UpdateOrderVM(o, u);
        InitializeComponent();
    }
}
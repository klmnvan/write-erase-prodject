using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class CreateOrderPage : UserControl
{
    public CreateOrderPage()
    {
        DataContext = new CreateOrderVM();
        InitializeComponent();
    }
}
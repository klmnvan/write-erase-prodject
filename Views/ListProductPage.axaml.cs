using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class ListProductPage : UserControl
{
    public ListProductPage()
    {
        DataContext = new ListProductVM();
        InitializeComponent();
    }
}
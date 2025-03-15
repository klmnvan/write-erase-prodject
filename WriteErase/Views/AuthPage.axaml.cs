using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErase.ViewModels;

namespace WriteErase;

public partial class AuthPage : UserControl
{
    public AuthPage()
    {
        DataContext = new AuthVM();
        InitializeComponent();
    }

}
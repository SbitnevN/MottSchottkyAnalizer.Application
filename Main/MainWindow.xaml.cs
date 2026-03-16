using MottSchottkyAnalizer.DI.Registration;
using System.Windows;

namespace MottSchottkyAnalizer.Application.Main;

[View<MainWindow>]
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
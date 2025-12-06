using MottSchottkyAnalizer.DI.Registration;
using System.Windows;

namespace MottSchottkyAnalizer.Application;

[View<MainWindow>]
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottSchottkyAnalizer.Application.Main;
using MottSchottkyAnalizer.Core.User;
using MottSchottkyAnalizer.DI.Injection;
using System.Windows;

namespace MottSchottkyAnalizer.Application;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += (_, ex) => HandleException(ex.Exception);

        AppDomain.CurrentDomain.UnhandledException += (_, ev) =>
        {
            if (ev.ExceptionObject is Exception ex)
                HandleException(ex);
        };

        _host = HostFactory.Create();
        MainWindow window = _host.Services.GetRequiredService<MainWindow>();

        window.ShowDialog();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        _host?.Dispose();
    }

    private void HandleException(Exception ex)
    {
        if (ex is UserException)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            // MessageBox.Show("Произошла ошибка приложения. Свяжитесь с поддержкой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}

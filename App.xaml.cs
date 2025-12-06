using MottSchottkyAnalizer.DI.Injection;
using System.Windows;

namespace MottSchottkyAnalizer.Application;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IAppHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = HostFactory.Create();
        MainWindow window = _host.AppProvider.GetRequiredService<MainWindow>();
        window.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        _host?.Dispose();
    }
}

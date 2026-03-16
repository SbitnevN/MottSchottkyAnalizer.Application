using ElinsData.Data;
using MottSchottkyAnalizer.Controls.Controls.Drawer;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;
using MottSchottkyAnalizer.Infrastructure.Dialogs;
using MottSchottkyAnalizer.Infrastructure.Plot;
using OxyPlot;

namespace MottSchottkyAnalizer.Application.Main;

[ViewModel<MainWindowViewModel>]
public class MainWindowViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IPlotFactory _plotFactory;

    public ElinsRecord ExperimentData
    {
        get => field;
        set => Set(ref field, value);
    } = new ElinsRecord();

    public double StartPotential
    {
        get => field;
        set => Set(ref field, value);
    }

    public double EndPotential
    {
        get => field;
        set => Set(ref field, value);
    }

    public double StepPotential
    {
        get => field;
        set => Set(ref field, value);
    }

    public double Frequency
    {
        get => field;
        set => Set(ref field, value);
    }

    public DrawerViewModel MenuViewModel { get; set; }

    public DrawerViewModel HistoryViewModel { get; set; }

    public DrawerOverlayViewModel OverlayViewModel { get; set; }

    public PotentialViewModel PotentialViewModel { get; set; }

    public PlotModel BodePlotModel { get; set; }

    public PlotModel SchottkyPlotModel { get; set; }

    public IRelayCommand DialogShow { get; set; }

    public MainWindowViewModel(IDialogService dialogService, IPlotFactory plotFactory, DrawerViewModel menuViewModel, DrawerViewModel historyViewModel, DrawerOverlayViewModel overlayViewModel, PotentialViewModel potentialViewModel)
    {
        _dialogService = dialogService;
        _plotFactory = plotFactory;

        MenuViewModel = menuViewModel;
        HistoryViewModel = historyViewModel;
        OverlayViewModel = overlayViewModel;

        BodePlotModel = _plotFactory.CreateBode();
        SchottkyPlotModel = _plotFactory.CreateSchottky();

        MenuViewModel.SetDrawerOverlay(overlayViewModel);
        HistoryViewModel.SetDrawerOverlay(overlayViewModel);

        PotentialViewModel = potentialViewModel;

        DialogShow = new RelayCommand(Dialog);
    }

    private void Dialog()
    {
    }
}
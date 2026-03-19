using ElinsData.Data;
using MottSchottkyAnalizer.Controls.Controls.Drawer;
using MottSchottkyAnalizer.Core.Extensions;
using MottSchottkyAnalizer.Core.User;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;
using MottSchottkyAnalizer.Infrastructure.Dialogs;
using MottSchottkyAnalizer.Infrastructure.Plot;
using MottSchottkyAnalizer.Math;
using OxyPlot;
using System.Collections.ObjectModel;
using Point = MottSchottkyAnalizer.Math.DataPoint;

namespace MottSchottkyAnalizer.Application.Main;

public enum ApproximationType
{
    Мнк,
}

[ViewModel<MainWindowViewModel>]
public class MainWindowViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IPlotFactory _plotFactory;

    private readonly ICollection<Point> _schottkyPoints = [];
    private LinearFitResult _linearFit = new LinearFitResult();

    public ElinsRecord ExperimentData
    {
        get => field;
        set => Set(ref field, value);
    } = new ElinsRecord();

    public double Frequency
    {
        get => field;
        set
        {
            Set(ref field, value);
            OnSetFrequency(value);
        }
    }

    public double? StartPotential
    {
        get => field;
        set => Set(ref field, value);
    }

    public double? EndPotential
    {
        get => field;
        set => Set(ref field, value);
    }

    public ApproximationType ApproximationType
    {
        get => field;
        set => Set(ref field, value);
    }

    public Array ApproximationTypes => Enum.GetValues<ApproximationType>();

    public ObservableCollection<PotentialRow> Potentials { get; } = new ObservableCollection<PotentialRow>();

    public ObservableCollection<double> Frequencies { get; } = new ObservableCollection<double>();

    public ObservableCollection<double> StartPotentials { get; } = new ObservableCollection<double>();

    public ObservableCollection<double> EndPotentials { get; } = new ObservableCollection<double>();

    public DrawerViewModel MenuViewModel { get; set; }

    public DrawerViewModel HistoryViewModel { get; set; }

    public DrawerOverlayViewModel OverlayViewModel { get; set; }

    public PotentialViewModel PotentialViewModel { get; }

    public BodeFileViewModel BodeFileViewModel { get; }

    public PlotModel BodePlotModel { get; set; }

    public PlotModel BodeTestPlotModel { get; set; }

    public PlotModel SchottkyPlotModel { get; set; }

    public IRelayCommand DialogShow { get; set; }
    public IRelayCommand ApproximationApply { get; set; }

    public MainWindowViewModel(
        IDialogService dialogService,
        IPlotFactory plotFactory,
        DrawerViewModel menuViewModel,
        DrawerViewModel historyViewModel,
        DrawerOverlayViewModel overlayViewModel,
        PotentialViewModel potentialViewModel,
        BodeFileViewModel bodeFileViewModel)
    {
        _dialogService = dialogService;
        _plotFactory = plotFactory;

        MenuViewModel = menuViewModel;
        HistoryViewModel = historyViewModel;
        OverlayViewModel = overlayViewModel;

        BodePlotModel = _plotFactory.CreateBode();
        SchottkyPlotModel = _plotFactory.CreateSchottky();
        BodeTestPlotModel = _plotFactory.CreateBode();

        MenuViewModel.SetDrawerOverlay(overlayViewModel);
        HistoryViewModel.SetDrawerOverlay(overlayViewModel);

        PotentialViewModel = potentialViewModel;
        BodeFileViewModel = bodeFileViewModel;

        DialogShow = new RelayCommand(Dialog);
        ApproximationApply = new RelayCommand(OnApproximationApply);

        BodeFileViewModel.OnOpened += DataLoaded;
        PotentialViewModel.OnApply += FillPotentials;
        PotentialViewModel.OnApply += FillPotentialRange;
    }

    private void Dialog()
    {
    }

    private void OnApproximationApply()
    {
        if (StartPotential == null || EndPotential == null)
            throw new UserException("Диапазон потенциалов не задан!");

        _linearFit = Approximation.FitLine(GetPointInRange(StartPotential.Value, EndPotential.Value));
        SchottkyPlotModel.AppendLine(_linearFit.Slope, _linearFit.Intercept, StartPotential.Value, EndPotential.Value);
    }

    private void DataLoaded(ElinsRecord data)
    {
        ExperimentData = data;
        BodePlotModel.UpdateBode(data.ImpedancePoints);
        BodeTestPlotModel.UpdateBodeTest(data.ImpedancePoints);

        Potentials.Clear();
        Potentials.AddRange(data.StepPotentials
            .Select(p => new PotentialRow(p, () => SchottkyPlotModel.UpdateSchottky(FillSchootkyPoints(Frequency)))));

        Frequencies.Clear();
        Frequencies.AddRange(data.ImpedancePoints.Select(i => i.Frequency).Distinct());

        Frequency = Frequencies.FirstOrDefault();

        PotentialViewModel.StepCount = data.Steps;
    }

    private void FillPotentials(double[] potentials)
    {
        if (potentials.Length == 0)
            return;

        foreach ((PotentialRow potential, int index) in Potentials.Select((p, i) => (p, i)))
        {
            potential.Potential = potentials[index];
        }

        SchottkyPlotModel.UpdateSchottky(FillSchootkyPoints(Frequency));
    }

    private void FillPotentialRange(double[] potentials)
    {
        EndPotentials.Clear();
        StartPotentials.Clear();

        StartPotentials.AddRange(potentials);
        EndPotentials.AddRange(potentials);
    }

    private void OnSetFrequency(double frequency)
    {
        SchottkyPlotModel.UpdateSchottky(FillSchootkyPoints(frequency));
    }

    private IEnumerable<Point> FillSchootkyPoints(double frequency)
    {
        _schottkyPoints.Clear();
        _schottkyPoints.AddRange(ExperimentData.ImpedancePoints
            .Where(p => p.Frequency == frequency)
            .Select(p => new Point(p.PotentialStep.Potential, 1d / p.Capacitance)));

        return _schottkyPoints;
    }

    private IEnumerable<Point> GetPointInRange(double start, double stop)
    {
        if (stop - start < 0)
            return _schottkyPoints.Where(i => i.X <= start && i.X >= stop);

        return _schottkyPoints.Where(i => i.X >= start && i.X <= stop);
    }
}
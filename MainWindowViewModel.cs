using ElinsData.Data;
using ElinsData.Extensions;
using MottSchottkyAnalizer.Controls.CustomMenu;
using MottSchottkyAnalizer.Controls.ListEditor;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;
using MottSchottkyAnalizer.Infrastructure.Dialogs;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.IO;

namespace MottSchottkyAnalizer.Application;

[ViewModel<MainWindowViewModel>]
public class MainWindowViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    public ElinsRecord ExperimentData
    {
        get => field;
        set => Set(ref field, value);
    } = new ElinsRecord();

    public PlotModel PlotModel
    {
        get => field;
        set => Set(ref field, value);
    } = new PlotModel();

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

    public MenuViewModel MenuViewModel { get; set; }

    public IRelayCommand DialogShow { get; set; }

    public MainWindowViewModel(IDialogService dialogService, MenuViewModel menuViewModel)
    {
        InitializePlot();

        _dialogService = dialogService;
        MenuViewModel = menuViewModel;

        DialogShow = new RelayCommand(Dialog);

        MenuViewModel.DataImported += HandleDataImported;
        MenuViewModel.DataExported += HandleDataExported;
    }

    private void Dialog()
    {
        ListEditorParameters parameters = new ListEditorParameters()
        {
            Title = "Редактор списка",
            Items = ExperimentData.ImpedancePoints.Cast<object>().ToList(),
        };

        if (_dialogService.Show<ListEditorView>(parameters))
            CalculatePlot();
    }

    private void InitializePlot()
    {
        PlotModel.Title = "Координаты Боде";

        PlotModel.Axes.Add(new LogarithmicAxis()
        {
            Position = AxisPosition.Bottom,
            Title = "Частота, Гц",
            Base = 10
        });

        PlotModel.Axes.Add(new LinearAxis()
        {
            Position = AxisPosition.Left,
            Title = "C, Ф/м2"
        });
    }

    private void CalculatePlot()
    {
        IEnumerable<DataPoint> point = ExperimentData.ImpedancePoints
            .OrderBy(p => p.Frequency)
            .Select(p => new DataPoint(p.Frequency, p.Capacitance));

        PlotModel.Series.Clear();

        LineSeries series = new LineSeries
        {
            MarkerType = MarkerType.Circle,
            MarkerSize = 2,
            MarkerFill = OxyColors.Red,
        };
        series.Points.AddRange(point);

        PlotModel.Series.Add(series);
        PlotModel.ResetAllAxes();
        PlotModel.InvalidatePlot(true);

        UpdateStepPotential();
    }

    private void UpdateStepPotential()
    {
        if (StartPotential != 0 && EndPotential != 0 && ExperimentData?.Steps > 0 && StepPotential == 0)
            StepPotential = Math.Abs(EndPotential - StartPotential) / ExperimentData.Steps;
    }

    private void HandleDataImported(MenuViewModel menu, DataImportedEventArgs args)
    {
        ExperimentData = args.Data;
        CalculatePlot();
    }

    private void HandleDataExported(MenuViewModel menu, DataExportedEventArgs args)
    {
        string path = args.Path;
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(ExperimentData.ToCsv());
        }
    }
}
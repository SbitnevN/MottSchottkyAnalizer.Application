using CommunityToolkit.Mvvm.ComponentModel;
using ElinsDataParser.Data;
using MottSchottkyAnalizer.Application.Controls.FileControls.FileExporter;
using MottSchottkyAnalizer.Application.Controls.FileControls.FileImporter;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MottSchottkyAnalizer.Application
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private ElinsData _experimentData = new ElinsData();

        [ObservableProperty]
        private FileExporterViewModel<ElinsData> _experimentExportViewModel;

        [ObservableProperty]
        private FileImporterViewModel _experimentImportViewModel;

        [ObservableProperty]
        private PlotModel _plotModel = new PlotModel();

        [ObservableProperty]
        private double _startPotential = 0;

        [ObservableProperty]
        private double _endPotential = 0;

        [ObservableProperty]
        private double _stepPotential = 0;

        [ObservableProperty]
        private double _frequency = 0;

        public MainWindowViewModel()
        {
            ExperimentExportViewModel = new FileExporterViewModel<ElinsData>();
            ExperimentImportViewModel = new FileImporterViewModel();

            ExperimentImportViewModel.OnFileImport += CalculatePlot;

            InitializePlot();
        }

        private void InitializePlot()
        {
            PlotModel.Title = "Координаты Боде";

            PlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Title = "Частота, Гц"
            });

            PlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = "C, Ф/м2"
            });
        }

        private async void CalculatePlot(string filePath)
        {
            _experimentData = await Parser.ParseAsync(filePath);

            IEnumerable<DataPoint> point = _experimentData.ImpedancePoints
                .OrderBy(p => p.Frequency)
                .Select(p => new DataPoint(p.Frequency, p.Capacitance));

            PlotModel.Series.Clear();

            LineSeries series = new LineSeries();
            series.Points.AddRange(point);

            PlotModel.Series.Add(series);
            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);

            UpdateStepPotential();
        }

        private void UpdateStepPotential()
        {
            if (StartPotential != 0 && EndPotential != 0 && _experimentData?.Steps?.Count > 0 && StepPotential == 0)
                StepPotential = Math.Abs(EndPotential - StartPotential) / _experimentData.Steps.Count;
        }

        partial void OnStartPotentialChanged(double value)
        {
            UpdateStepPotential();
        }

        partial void OnEndPotentialChanged(double value)
        {
            UpdateStepPotential();
        }

        partial void OnFrequencyChanged(double value)
        {

        }
    }
}
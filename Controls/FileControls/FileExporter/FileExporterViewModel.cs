using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace MottSchottkyAnalizer.Application.Controls.FileControls.FileExporter
{
    public partial class FileExporterViewModel<T> : ObservableObject where T : class
    {
        [ObservableProperty]
        private string _filePath = string.Empty;

        private T? _data = null;

        public event Action<string>? OnFileExport;

        public void Initialize(T data)
        {
            _data = data;
        }

        [RelayCommand]
        private void ExportFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt",
                FileName = "MSAnalyze.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;

                OnFileExport?.Invoke(FilePath);
            }
        }
    }
}

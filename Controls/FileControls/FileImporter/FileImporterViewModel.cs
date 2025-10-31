using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MottSchottkyAnalizer.Application.Controls.FileControls.FileImporter.Localization;
using System.Windows;

namespace MottSchottkyAnalizer.Application.Controls.FileControls.FileImporter
{
    public partial class FileImporterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _filePath = string.Empty;

        public event Action<string>? OnFileImport;

        [RelayCommand]
        private void ImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt|Elins Data Format (*.edf)|*.edf",
                FileName = "MSAnalyze.txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;

                OnFileImport?.Invoke(FilePath);
                MessageBox.Show(LocalizedStrings.SuccessfulImport);
            }
        }
    }
}

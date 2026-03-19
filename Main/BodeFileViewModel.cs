using ElinsData.Data;
using ElinsData.Reader;
using MottSchottkyAnalizer.Core.Extensions;
using MottSchottkyAnalizer.Core.User;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;
using MottSchottkyAnalizer.Infrastructure.Files;
using System.IO;

namespace MottSchottkyAnalizer.Application.Main;

[ViewModel<BodeFileViewModel>]
public class BodeFileViewModel
{
    private readonly IFileService _fileService;
    private ElinsRecord? _elinsRecord;

    public event Action<ElinsRecord>? OnOpened;
    public IRelayCommand Open { get; set; }
    public IRelayCommand Save { get; set; }

    public BodeFileViewModel(IFileService fileService)
    {
        _fileService = fileService;

        Open = new RelayCommand(Opening);
        Save = new RelayCommand(Saving);
    }

    public void Opening()
    {
        string path = _fileService.SelectFile(Filters.ExperimentalFilter);
        if (path.IsEmpty())
            return;

        _elinsRecord = ElinsFactory.Create(path);
        OnOpened?.Invoke(_elinsRecord);
    }

    public void Saving()
    {
        if (_elinsRecord == null)
            throw new UserException("Для начала откройте файл!");

        string path = _fileService.CreateFile(_elinsRecord.Name, Filters.ExperimentalFilter);
        if (path.IsEmpty())
            return;

        File.WriteAllText(path, _elinsRecord.ToCsv());
    }
}

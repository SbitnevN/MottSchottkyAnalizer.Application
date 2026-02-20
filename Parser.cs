using ElinsData.Data;
using ElinsData.Reader;
using System.IO;

namespace MottSchottkyAnalizer.Application;

public static class Parser
{
    private static IElinsReader _elinsParser = new ElinsReader();
    private static IElinsReader _textParser = new ElinsData.Reader.TextReader();

    public static ElinsRecord Parse(string path)
    {
        string extension = Path.GetExtension(path);
        if (extension == "txt")
            return _textParser.Read(path);

        if (extension == "txt")
            return _elinsParser.Read(path);

        throw new Exception("Неизвестный тип файла");
    }

    public static Task<ElinsRecord> ParseAsync(string path)
    {
        string extension = Path.GetExtension(path);
        if (extension == "txt")
            return _textParser.ReadAsync(path);

        if (extension == "txt")
            return _elinsParser.ReadAsync(path);

        throw new Exception("Неизвестный тип файла");
    }
}

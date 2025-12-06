using ElinsDataParser;
using ElinsDataParser.Data;
using ElinsDataParser.Elins;
using ElinsDataParser.Text;
using System.IO;

namespace MottSchottkyAnalizer.Application;

public static class Parser
{
    private static IElinsParser _elinsParser = new ElinsParser();
    private static IElinsParser _textParser = new TextParser();

    public static ElinsData Parse(string path)
    {
        string extension = Path.GetExtension(path);
        if (extension == "txt")
            return _textParser.Parse(path);

        if (extension == "txt")
            return _elinsParser.Parse(path);

        throw new Exception("Неизвестный тип файла");
    }

    public static Task<ElinsData> ParseAsync(string path)
    {
        string extension = Path.GetExtension(path);
        if (extension == "txt")
            return _textParser.ParseAsync(path);

        if (extension == "txt")
            return _elinsParser.ParseAsync(path);

        throw new Exception("Неизвестный тип файла");
    }
}

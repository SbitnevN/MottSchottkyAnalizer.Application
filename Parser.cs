using ElinsDataParser;
using ElinsDataParser.Data;
using ElinsDataParser.Elins;
using ElinsDataParser.Text;

namespace MottSchottkyAnalizer.Application
{
    public static class Parser
    {
        private static IElinsParser _elinsParser = new ElinsParser();
        private static IElinsParser _textParser = new TextParser();

        public static ElinsData Parse(string path)
        {
            if (path.Contains(".txt"))
                return _textParser.Parse(path);

            if (path.Contains(".edf"))
                return _elinsParser.Parse(path);

            throw new Exception("Неизвестный тип файла");
        }

        public static Task<ElinsData> ParseAsync(string path)
        {
            if (path.Contains(".txt"))
                return _textParser.ParseAsync(path);

            if (path.Contains(".edf"))
                return _elinsParser.ParseAsync(path);

            throw new Exception("Неизвестный тип файла");
        }
    }
}

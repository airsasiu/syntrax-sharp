using SyntraxCore.Units;

namespace SyntraxConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var styleConfig = new StyleConfig();
            var inputArgs = new InputArguments(args);
            string scriptText = File.ReadAllText(inputArgs.Input);

            string result = GenerateSVG(inputArgs.Title, styleConfig, scriptText);
        }

        private static string GenerateSVG(string title, StyleConfig styleConfig, string scriptText)
        {
            IUnit root;
            string titleInSpecFile;
        }
    }
}

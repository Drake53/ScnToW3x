using System.IO;

namespace ScenarioConverter
{
    public class Program
    {
        private const string InputFolder = @"";
        private const string OutputFolder = @"";

        private static void Main()
        {
            foreach (var scenario in Directory.EnumerateFiles(InputFolder, "*.SCN", SearchOption.TopDirectoryOnly))
            {
                var inputFileInfo = new FileInfo(scenario);
                var outputMapFolder = Path.Combine(OutputFolder, inputFileInfo.Name.Substring(0, inputFileInfo.Name.Length - inputFileInfo.Extension.Length));

                MapConverter.Convert(scenario, outputMapFolder);
            }
        }
    }
}
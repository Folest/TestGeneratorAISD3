using System;
using System.IO;
using System.Text;

namespace TestGenerator.CLI
{
    class TestGenerator
    {
        public static void GenerateTests(TestSettings currentSettings, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var sw = new StreamWriter(fs))
                {
                    var rand = new Random();
                    sw.WriteLine($"{currentSettings.CommandCount}");
                    for (var i = 0; i < currentSettings.CommandCount; i++)
                    {
                        var commandInt = rand.Next(currentSettings.GetSumOfChances());
                        var sumOfPreviousChances = 0;

                        foreach (var option in currentSettings.AvailableOptionsTuples)
                        {
                            if (commandInt < currentSettings.GetChance(option.Key) + sumOfPreviousChances)
                            {
                                var appendedLineBuilder = new StringBuilder();
                                appendedLineBuilder.Append(option.Key);
                                for (var j = 0; j < option.Value; j++)
                                {
                                    appendedLineBuilder.Append($" {(j == 0 ? rand.Next() % 1000: rand.Next())}");
                                }
                                sw.WriteLine(appendedLineBuilder.ToString());
                                break;
                            }
                            else
                            {
                                sumOfPreviousChances += currentSettings.GetChance(option.Key);
                            }
                        }
                    }
                    fs.Flush();
                }
                TestMenuConsolePages.PrintGreen("Succeeded making test.txt");
                Console.ReadKey();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace TestGenerator.CLI
{
    [Serializable]
    internal class TestSettings
    {
        public readonly char[] AvailableOptions = {'a', 'i', 'e', 'p', 'm'};

        public readonly Dictionary<char, int> AvailableOptionsTuples = new Dictionary<char, int>
        {
            {'a', 2},
            {'i', 3},
            {'e', 1},
            {'p', 1},
            {'m', 2}
        };
        public int CommandCount { get; set; }
        [DataMember]
        private readonly Dictionary<char, int> _commandChances = new Dictionary<char, int>();
        private int _chanceSum;

        public TestSettings(int defaultOptionChance)
        {
            foreach (var option in AvailableOptions)
            {
                SetChance(option,defaultOptionChance);
            }
        }

        public void SetChance(char option, int chance)
        {
            if (AvailableOptions.Any(x => x == option))
            {
                if (_commandChances.TryGetValue(option, out var value))
                {
                    _chanceSum -= value;
                    _chanceSum += chance;
                    _commandChances[option] = chance;
                }
                else
                {
                    _commandChances.Add(option, chance);
                    _chanceSum += chance;

                }
            }
            else
            {
                throw new ArgumentException("There is no such command");
            }
        }

        public int GetChance(char option)
        {
            if (_commandChances.TryGetValue(option, out var value))
            {
                return value;
            }

            return 0;
        }

        public int GetSumOfChances()
        {
            return _chanceSum;
        }

        public static TestSettings Deserialize(string path)
        {
            object returned = null;
            if (File.Exists(path))
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var sw = new StreamReader(fileStream))
                    {
                        var ser = new DataContractSerializer(typeof(TestSettings));
                        try
                        {
                            returned = ser.ReadObject(fileStream);
                        }
                        catch (Exception)
                        {
                            TestMenuConsolePages.PrintRed("Settings file is corrupted, creating default settings file");
                            Console.ReadKey();
                            return (TestSettings)returned;
                        }
                        finally
                        {
                            sw.Close();
                        }
                    }
                }
                TestMenuConsolePages.PrintGreen("Configuration file succefuly loaded");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                return (TestSettings)returned;
            }
            else
            {

                var previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Couldn't open settings file, creating default");
                Console.ForegroundColor = previousColor;
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                return new TestSettings(1);
            }
        }

        public void Serialize(string path)
        {
            using (var fileStream = new FileStream(path,FileMode.OpenOrCreate,FileAccess.Write,FileShare.None))
            {
                using (var sw = new StreamWriter(fileStream))
                {
                    var ser = new DataContractSerializer(GetType());
                    ser.WriteObject(fileStream, this);
                    sw.Close();
                }
                fileStream.Close();
            }
        }
    }
}

using System;

namespace TestGenerator.CLI
{
    class TestService
    {
        private TestSettings _currentSettings;
        private readonly TestMenuConsolePages _menu = new TestMenuConsolePages();
        private readonly TestGenerator _generator = new TestGenerator();


        public void TestGeneratingProcedure()
        {
            _currentSettings = TestSettings.Deserialize("settings.xml") ?? new TestSettings(1);

            while (true)
            {
                Console.Clear();
                TestMenuConsolePages.ViewMainMenu();
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        _menu.ViewChanceMenu(_currentSettings);
                        Tuple<bool, char, int> change = TestMenuConsolePages.ChangeOption();
                        if (change.Item1)
                        {
                            _currentSettings.SetChance(change.Item2, change.Item3);
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            continue;
                        }
                    case "2":
                        _currentSettings.CommandCount = TestMenuConsolePages.AskForCommandAmount();
                        GenerateTests("test.txt");
                        break;
                    default:
                        _currentSettings.Serialize("settings.xml");
                        return;
                }
            }
        }


        private void GenerateTests(string path)
        {
            TestGenerator.GenerateTests(_currentSettings, path);
        }
    }
}
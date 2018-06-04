using System;
using System.Linq;

namespace TestGenerator.CLI
{
    class TestMenuConsolePages
    {
        public static void ViewMainMenu()
        {
            Console.WriteLine("Please select option: ");
            Console.WriteLine("1. Change test generation settings");
            Console.WriteLine("2. Generate test according to settings");
            PrintRed("Or press any other key. exit");

        }

        public static void PrintGreen(string message)
        {
            var previousConsoleForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = previousConsoleForeground;
        }

        public static void PrintRed(string message)
        {
            var previousConsoleForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = previousConsoleForeground;
        }

        public void ViewChanceMenu(TestSettings currentSettings)
        {
            Console.WriteLine("Avaible commands:");
            foreach (var option in currentSettings.AvailableOptions)
            {
                Console.WriteLine(
                    $"\'{option}\' command (current chance {currentSettings.GetChance(option)} pts which is " +
                    (currentSettings.GetSumOfChances() == 0 ? "0 %)":
                        $"{currentSettings.GetChance(option) * 100 / currentSettings.GetSumOfChances()} %)"));
            }
        }

      
        public static int AskForCommandAmount()
        {
            Console.WriteLine("Please the number of commands you want to generate");
            var input = Console.ReadLine();

            if (int.TryParse(input,out var result) && result > 0)
            {
                return result;
            }

            Console.WriteLine("Please enter a valid number");
            return AskForCommandAmount();
        }

        public static Tuple<bool, char, int> ChangeOption()
        {
            while (true)
            {
                PrintRed("Enter the command char to change it's chance value, or press enter to return to previous menu");
                var optionInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(optionInput))
                    return new Tuple<bool, char, int>(false, '0', 0);

                if (new[] {'a', 'e', 'p', 'i', 'm'}.Any(x => optionInput != null && x == optionInput[0]))
                {
                    if (optionInput == null) return new Tuple<bool, char, int>(false, '0', 0);
                    Console.WriteLine($"You're changing value for '{optionInput[0]}' option:");
                    var chanceInput = Console.ReadLine();

                    if (!int.TryParse(chanceInput, out var chanceValue))
                        return new Tuple<bool, char, int>(false, '0', 0);
                    PrintGreen($"Successfuly changed value for '{optionInput[0]}' to {chanceValue}");
                    Console.ReadKey();
                    return new Tuple<bool, char, int>(true, optionInput[0], chanceValue);
                }

                Console.WriteLine("Not a valid input for option");
                Console.ReadKey();
            }
        }
    }
}
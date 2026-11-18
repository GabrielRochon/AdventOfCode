using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public static class EfficienButtonPresser
{
    public static void CalculateFewestPressesForMachinesConfiguration(string fileName)
    {
        string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 10 - Factory/" + fileName;
        string[] lines = System.IO.File.ReadAllLines(filePath);

        foreach(string line in lines)
        {
            // Each line is a machine
            //[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}

            int finalDesiredLightsStateEndIndex = line.IndexOf(']');
            int lightsJoltageStartIndex = line.IndexOf('{');

            string finalDesiredLightsState = line.Substring(1, finalDesiredLightsStateEndIndex - 1);
            string availableButtonsString = line.Substring(finalDesiredLightsStateEndIndex + 2, lightsJoltageStartIndex - finalDesiredLightsStateEndIndex - 3);
            string lightsJoltage = line.Substring(lightsJoltageStartIndex + 1, line.Length - lightsJoltageStartIndex - 2);

            Console.WriteLine($"'{finalDesiredLightsState}'");
            Console.WriteLine($"'{availableButtonsString}'");
            Console.WriteLine($"'{lightsJoltage}'");
            Console.WriteLine("--------------------------");

            // Turn available buttons into a list of lists
            List<List<int>> availableButtons = new List<List<int>>();
            StringBuilder sb = new StringBuilder(availableButtonsString);
            while (sb.ToString().Contains('('))
            {
                Console.WriteLine(sb.ToString());

                int closingParenthesesIndex = sb.ToString().IndexOf(')');
                List<int> toggledLightsByButton = sb.ToString().Substring(1, closingParenthesesIndex - 1).Split(',').Select(x => Int32.Parse(x)).ToList();
                availableButtons.Add(toggledLightsByButton);

                // Remove that button from the string for the next iteration
                sb.Remove(0, Math.Min(sb.ToString().Length, closingParenthesesIndex + 2));
            }

            // A disabled light (.) means it's been pressed i % 2 == 0 times, while a toggled light (#), i % 2 == 1 times
            // Need fewest moves, so multiple scenarios
            // In terms of priority:
            // 1. There is a single button toggling all necessary buttons at once
            // 2. 

        }
    }
}
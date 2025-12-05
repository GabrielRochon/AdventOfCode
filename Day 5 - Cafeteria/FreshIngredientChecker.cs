/*

PART ONE

The Elves in the kitchen explain the situation: because of their complicated new inventory management system, they can't figure out which of their ingredients are fresh and which are spoiled. When you ask how it works, they give you a copy of their database (your puzzle input).

The database operates on ingredient IDs. It consists of a list of fresh ingredient ID ranges, a blank line, and a list of available ingredient IDs. For example:

3-5
10-14
16-20
12-18

1
5
8
11
17
32
The fresh ID ranges are inclusive: the range 3-5 means that ingredient IDs 3, 4, and 5 are all fresh. The ranges can also overlap; an ingredient ID is fresh if it is in any range.

The Elves are trying to determine which of the available ingredient IDs are fresh. In this example, this is done as follows:

Ingredient ID 1 is spoiled because it does not fall into any range.
Ingredient ID 5 is fresh because it falls into range 3-5.
Ingredient ID 8 is spoiled.
Ingredient ID 11 is fresh because it falls into range 10-14.
Ingredient ID 17 is fresh because it falls into range 16-20 as well as range 12-18.
Ingredient ID 32 is spoiled.
So, in this example, 3 of the available ingredient IDs are fresh.

Process the database file from the new inventory management system. How many of the available ingredient IDs are fresh?

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class FreshIngredientChecker
{
    public static void CheckFreshIngredients(string fileName)
    {
        string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 5 - Cafeteria/" + fileName;
        
        // Parsing logic: each line is a range until there's a blank line, then ingredients to check
        string[] lines = File.ReadAllLines(filepath);

        List<Tuple<double, double>> freshRanges = new List<Tuple<double, double>>();
        int freshAvailableIngredientsCount = 0;
        bool parsingFreshRanges = true;

        foreach(string line in lines)
        {
            // Divider between fresh ranges and ingredients to check
            if (line == "")
            {
                parsingFreshRanges = false;
                continue;
            }

            // Build fresh ingredient list
            if (parsingFreshRanges)
            {
                string[] parts = line.Split('-');
                freshRanges.Add(new Tuple<double, double>(double.Parse(parts[0]), double.Parse(parts[1])));
            }
            else
            // Check available ingredients
            {
                foreach(Tuple<double, double> range in freshRanges)
                {
                    if (double.Parse(line) >= range.Item1 && double.Parse(line) <= range.Item2)
                    {
                        freshAvailableIngredientsCount++;
                        break;
                    }
                }
            }
        }

        Console.WriteLine("[DAY 5] Fresh available ingredients count is: " + freshAvailableIngredientsCount);
    }
}
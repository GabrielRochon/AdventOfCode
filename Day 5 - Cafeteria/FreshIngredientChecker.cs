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

---

PART TWO

The Elves start bringing their spoiled inventory to the trash chute at the back of the kitchen.

So that they can stop bugging you when they get new inventory, the Elves would like to know all of the IDs that the fresh ingredient ID ranges consider to be fresh. An ingredient ID is still considered fresh if it is in any range.

Now, the second section of the database (the available ingredient IDs) is irrelevant. Here are the fresh ingredient ID ranges from the above example:

3-5
10-14
16-20
12-18
The ingredient IDs that these ranges consider to be fresh are 3, 4, 5, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, and 20. So, in this example, the fresh ingredient ID ranges consider a total of 14 ingredient IDs to be fresh.

Process the database file again. How many ingredient IDs are considered to be fresh according to the fresh ingredient ID ranges?
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class FreshIngredientChecker
{
    public static void GetFreshAvailableIngredients(string fileName)
    {
        string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 5 - Cafeteria/" + fileName;
        
        // Parsing logic: each line is a range until there's a blank line, then ingredients to check
        string[] lines = File.ReadAllLines(filepath);

        List<Tuple<long, long>> freshRanges = new List<Tuple<long, long>>();
        long freshAvailableIngredientsCount = 0, overallFreshIngredientsCount = 0;
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
                freshRanges.Add(new Tuple<long, long>(long.Parse(parts[0]), long.Parse(parts[1])));
            }
            else
            // Check available ingredients
            {
                long ingredientId = long.Parse(line);
                foreach(Tuple<long, long> range in freshRanges)
                {
                    if (ingredientId >= range.Item1 && ingredientId <= range.Item2)
                    {
                        freshAvailableIngredientsCount++;
                        break;
                    }
                }
            }
        }

        // Part two: Based on fresh ranges, how many ingredients are fresh?
        // Need to account for overlapping ranges
        freshRanges = MergeOverlappingRanges(freshRanges);

        // Once overlap is removed, we can sum up the ranges
        foreach(Tuple<long, long> range in freshRanges)
        {
            overallFreshIngredientsCount += range.Item2 - range.Item1 + 1;
        }

        Console.WriteLine("[DAY 5] Fresh available ingredients count is: " + freshAvailableIngredientsCount);
        Console.WriteLine("[DAY 5] Overall fresh ingredients count is: " + overallFreshIngredientsCount);
    }

    private static List<Tuple<long, long>> MergeOverlappingRanges(List<Tuple<long, long>> freshRanges)
    {
        // Sort the input first
        freshRanges.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        List<Tuple<long, long>> mergedRanges = new List<Tuple<long, long>>();
        mergedRanges.Add(freshRanges[0]);

        for(int i = 1; i < freshRanges.Count; i++)
        {
            Tuple<long, long> lastMerged = mergedRanges[mergedRanges.Count - 1];

            // Sorted in order of start value, so only need to check end value
            // Merge if overlapping, otherwise just add the candidate as the last "mergedRange"
            if (lastMerged.Item2 >= freshRanges[i].Item1)
            {
                mergedRanges[mergedRanges.Count - 1] = Tuple.Create(lastMerged.Item1, Math.Max(lastMerged.Item2, freshRanges[i].Item2));
            }
            else
            {
                mergedRanges.Add(freshRanges[i]);
            }
        }

        return mergedRanges;
    }
}

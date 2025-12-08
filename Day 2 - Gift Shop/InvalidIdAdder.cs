/*

PART ONE

As it turns out, one of the younger Elves was playing on a gift shop computer and managed to add a whole bunch of invalid product IDs to their gift shop database! Surely, it would be no trouble for you to identify the invalid product IDs for them, right?

They've even checked most of the product ID ranges already; they only have a few product ID ranges (your puzzle input) that you'll need to check. For example:

11-22,95-115,998-1012,1188511880-1188511890,222220-222224,
1698522-1698528,446443-446449,38593856-38593862,565653-565659,
824824821-824824827,2121212118-2121212124
(The ID ranges are wrapped here for legibility; in your input, they appear on a single long line.)

The ranges are separated by commas (,); each range gives its first ID and last ID separated by a dash (-).

Since the young Elf was just doing silly patterns, you can find the invalid IDs by looking for any ID which is made only of some sequence of digits repeated twice. So, 55 (5 twice), 6464 (64 twice), and 123123 (123 twice) would all be invalid IDs.

None of the numbers have leading zeroes; 0101 isn't an ID at all. (101 is a valid ID that you would ignore.)

Your job is to find all of the invalid IDs that appear in the given ranges. In the above example:

11-22 has two invalid IDs, 11 and 22.
95-115 has one invalid ID, 99.
998-1012 has one invalid ID, 1010.
1188511880-1188511890 has one invalid ID, 1188511885.
222220-222224 has one invalid ID, 222222.
1698522-1698528 contains no invalid IDs.
446443-446449 has one invalid ID, 446446.
38593856-38593862 has one invalid ID, 38593859.
The rest of the ranges contain no invalid IDs.
Adding up all the invalid IDs in this example produces 1227775554.

What do you get if you add up all of the invalid IDs?

---

PART TWO

The clerk quickly discovers that there are still invalid IDs in the ranges in your list. Maybe the young Elf was doing other silly patterns as well?

Now, an ID is invalid if it is made only of some sequence of digits repeated at least twice. So, 12341234 (1234 two times), 123123123 (123 three times), 1212121212 (12 five times), and 1111111 (1 seven times) are all invalid IDs.

From the same example as before:

11-22 still has two invalid IDs, 11 and 22.
95-115 now has two invalid IDs, 99 and 111.
998-1012 now has two invalid IDs, 999 and 1010.
1188511880-1188511890 still has one invalid ID, 1188511885.
222220-222224 still has one invalid ID, 222222.
1698522-1698528 still contains no invalid IDs.
446443-446449 still has one invalid ID, 446446.
38593856-38593862 still has one invalid ID, 38593859.
565653-565659 now has one invalid ID, 565656.
824824821-824824827 now has one invalid ID, 824824824.
2121212118-2121212124 now has one invalid ID, 2121212121.
Adding up all the invalid IDs in this example produces 4174379265.

What do you get if you add up all of the invalid IDs using these new rules?

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class InvalidAdder
{
    // Part one
    public static void AddSimpleInvalidIds(string fileName)
    {
       AddInvalidIds(2, fileName);
    }

    // Part two
    public static void AddComplexInvalidIds(string fileName)
    {
        AddInvalidIds(999999999, fileName);  // Arbitrary large pattern length
    }

    // Core function to add invalid IDs based on max pattern length
    public static void AddInvalidIds(int maxPatternLength, string fileName)
    {
        string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 2 - Gift Shop/" + fileName;
        long invalidIdsSum = 0;

        // Split the string into substrings with delimiter ',', then split each substring into start and end values with delimiter '-'
        string[] ranges = File.ReadAllText(filePath).Split(',');
        List<Tuple<long, long>> rangeList = new List<Tuple<long, long>>();

        foreach(string range in ranges)
        {
            long firstNumber = long.Parse(range.Split('-')[0]);
            long lastNumber = long.Parse(range.Split('-')[1]);
            rangeList.Add(new Tuple<long, long>(firstNumber, lastNumber));
        }

        // Go thru all possible numbers. If the number has even length of digits, check if it's made of some sequence of digits repeated twice
        foreach (Tuple<long, long> range in rangeList)
        {
            for (long i =  range.Item1; i <= range.Item2; i++)
            {
                string numberStr = i.ToString();
                
                // Check for all possible pattern lengths
                int localMaxPatternLength = Math.Min(numberStr.Length / 2, maxPatternLength);

                for (int patternLength = 1; patternLength <= localMaxPatternLength; patternLength++)
                {
                    if (numberStr.Length % patternLength == 0)
                    {
                        int repeatCount = numberStr.Length / patternLength;
                        string pattern = numberStr.Substring(0, patternLength);
                        string constructedStr = "";

                        for (int r = 0; r < repeatCount; r++)
                        {
                            constructedStr += pattern;
                        }

                        if (constructedStr == numberStr)
                        {
                            if (invalidIdsSum > long.MaxValue - i)
                            {
                                throw new OverflowException("The sum of invalid IDs exceeds the maximum value for a long integer.");
                            }
                            invalidIdsSum += i;
                            break;  // No need to check for other pattern lengths
                        }
                    }
                }
            }
        }
        
        Console.WriteLine($"[DAY 2] Added up, the invalid IDs sum is : {invalidIdsSum}");
    }
}
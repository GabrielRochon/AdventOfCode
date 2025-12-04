/*

PART ONE

There are batteries nearby that can supply emergency power to the escalator for just such an occasion. The batteries are each labeled with their joltage rating, a value from 1 to 9. You make a note of their joltage ratings (your puzzle input). For example:

987654321111111
811111111111119
234234234234278
818181911112111
The batteries are arranged into banks; each line of digits in your input corresponds to a single bank of batteries. Within each bank, you need to turn on exactly two batteries; the joltage that the bank produces is equal to the number formed by the digits on the batteries you've turned on. For example, if you have a bank like 12345 and you turn on batteries 2 and 4, the bank would produce 24 jolts. (You cannot rearrange batteries.)

You'll need to find the largest possible joltage each bank can produce. In the above example:

In 987654321111111, you can make the largest joltage possible, 98, by turning on the first two batteries.
In 811111111111119, you can make the largest joltage possible by turning on the batteries labeled 8 and 9, producing 89 jolts.
In 234234234234278, you can make 78 by turning on the last two batteries (marked 7 and 8).
In 818181911112111, the largest joltage you can produce is 92.
The total output joltage is the sum of the maximum joltage from each bank, so in this example, the total output joltage is 98 + 89 + 78 + 92 = 357.

There are many batteries in front of you. Find the maximum joltage possible from each bank; what is the total output joltage?

---

PART TWO

Now, you need to make the largest joltage by turning on exactly twelve batteries within each bank.

The joltage output for the bank is still the number formed by the digits of the batteries you've turned on; the only difference is that now there will be 12 digits in each bank's joltage output instead of two.

Consider again the example from before:

987654321111111
811111111111119
234234234234278
818181911112111
Now, the joltages are much larger:

In 987654321111111, the largest joltage can be found by turning on everything except some 1s at the end to produce 987654321111.
In the digit sequence 811111111111119, the largest joltage can be found by turning on everything except some 1s, producing 811111111119.
In 234234234234278, the largest joltage can be found by turning on everything except a 2 battery, a 3 battery, and another 2 battery near the start to produce 434234234278.
In 818181911112111, the joltage 888911112111 is produced by turning on everything except some 1s near the front.
The total output joltage is now much larger: 987654321111 + 811111111119 + 434234234278 + 888911112111 = 3121910778619.

What is the new total output joltage?

*/

using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class MaxJoltageAdder
{

    public static void SumSimpleMaxVoltageOfPowerBanks(string fileName, bool verboseMode)
    {
        SumMaxVoltageOfPowerBanks(2, fileName, verboseMode);
    }

        public static void SumComplexMaxVoltageOfPowerBanks(string fileName, bool verboseMode)
    {
        SumMaxVoltageOfPowerBanks(12, fileName, verboseMode);
    }

    // Generalized method to sum max voltage of power banks given number of batteries to enable
    public static void SumMaxVoltageOfPowerBanks(int batteriesToEnable, string fileName, bool verboseMode)
    {
        string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 3 - Lobby/" + fileName;

        string[] banks = File.ReadAllLines(filepath);
        double maxVoltageSum = 0;

        /* Manually, how I would do it:

            eg. 818181911112111

            Is there a 9? Yes. (Otherwise, is there an 8? etc.)
            Take everything after the 9, find the next highest number
            Next highest to the right is 2, so 92 (for part one)

            Edge case: if 9 is at the end, look for next highest before it

            For part two, use recursion?
        */

        foreach(string bank in banks)
        {
            double bankVoltage = RecursivelyGetLargestCombination(batteriesToEnable, bank, "");
            maxVoltageSum += bankVoltage;

            if (verboseMode)
            {
                Console.WriteLine("Bank: " + bank + " => Max Voltage: " + bankVoltage);
            }
        }

        Console.WriteLine($"[DAY 3] Sum of max joltage adapters with {batteriesToEnable} batteries to enable is: " + maxVoltageSum);
    }

    // Optimized greedy approach: greedily pick the largest digit at each position
    // This works because we want the leftmost digit to be as large as possible
    private static double RecursivelyGetLargestCombination(int batteriesToEnable, string bankSubstring, string currentCombination)
    {
        // Base case
        if (batteriesToEnable == 0)
        {
            return double.Parse(currentCombination);
        }

        // Greedy optimization: pick the largest digit we can while leaving enough batteries for remaining selections
        int remaining = bankSubstring.Length - batteriesToEnable;
        int indexOfLargestDigit = 0;
        for (int i = 0; i <= remaining; i++)
        {
            if (bankSubstring[i] > bankSubstring[indexOfLargestDigit])
            {
                indexOfLargestDigit = i;
            }
        }
        
        string nextCombination = currentCombination + bankSubstring[indexOfLargestDigit];
        return RecursivelyGetLargestCombination(batteriesToEnable - 1, bankSubstring.Substring(indexOfLargestDigit + 1), nextCombination);
    }
}

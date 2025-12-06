/*
PART ONE

As you try to find a way out, you are approached by a family of cephalopods! They're pretty sure they can get the door open, but it will take some time. While you wait, they're curious if you can help the youngest cephalopod with her math homework.

Cephalopod math doesn't look that different from normal math. The math worksheet (your puzzle input) consists of a list of problems; each problem has a group of numbers that need to either be either added (+) or multiplied (*) together.

However, the problems are arranged a little strangely; they seem to be presented next to each other in a very long horizontal list. For example:

123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  
Each problem's numbers are arranged vertically; at the bottom of the problem is the symbol for the operation that needs to be performed. Problems are separated by a full column of only spaces. The left/right alignment of numbers within each problem can be ignored.

So, this worksheet contains four problems:

123 * 45 * 6 = 33210
328 + 64 + 98 = 490
51 * 387 * 215 = 4243455
64 + 23 + 314 = 401
To check their work, cephalopod students are given the grand total of adding together all of the answers to the individual problems. In this worksheet, the grand total is 33210 + 490 + 4243455 + 401 = 4277556.

Of course, the actual worksheet is much wider. You'll need to make sure to unroll it completely so that you can read the problems clearly.

Solve the problems on the math worksheet. What is the grand total found by adding together all of the answers to the individual problems?

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public static class CephalopodMathSolver
{
    public static void SumAllProblems(string fileName)
    {
        string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 6 - Trash Compactor/" + fileName;

        long grandTotal = 0;

        string[] lines = File.ReadAllLines(filepath);
        
        // First, get the operations
        string operationsLine = lines[lines.Length - 1];
        char[] operations = operationsLine.Replace(" ", "").ToCharArray();
        List<long> problemsSums = new List<long>();

        // Then, get the problems by columns
        for(int i = 0; i < lines.Length - 1; i++)
        {
            long[] currentNumbers = lines[i]
                .Split(' ')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => Int64.Parse(s)).ToArray();

            for(int j = 0; j < operations.Length; j++)
            {
                if (i == 0)
                {
                    problemsSums.Add(currentNumbers[j]);
                }
                else
                {
                    // Perform the operation
                    problemsSums[j] = operations[j] == '+' 
                        ? problemsSums[j] + currentNumbers[j] 
                        : problemsSums[j] * currentNumbers[j];
                }
            }
        }

        // Calculate grand total
        foreach(long problemSum in problemsSums)
        {
            grandTotal += problemSum;
        }

        Console.WriteLine($"[DAY 6] Grand total of all cephalopod math problems is: {grandTotal}");
    }
}

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

---

PART TWO

The big cephalopods come back to check on how things are going. When they see that your grand total doesn't match the one expected by the worksheet, they realize they forgot to explain how to read cephalopod math.

Cephalopod math is written right-to-left in columns. Each number is given in its own column, with the most significant digit at the top and the least significant digit at the bottom. (Problems are still separated with a column consisting only of spaces, and the symbol at the bottom of the problem is still the operator to use.)

Here's the example worksheet again:

123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  
Reading the problems right-to-left one column at a time, the problems are now quite different:

The rightmost problem is 4 + 431 + 623 = 1058
The second problem from the right is 175 * 581 * 32 = 3253600
The third problem from the right is 8 + 248 + 369 = 625
Finally, the leftmost problem is 356 * 24 * 1 = 8544
Now, the grand total is 1058 + 3253600 + 625 + 8544 = 3263827.

Solve the problems on the math worksheet again. What is the grand total found by adding together all of the answers to the individual problems?

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
        string[] lines = File.ReadAllLines(filepath);
        
        string operationsLine = lines[lines.Length - 1];
        char[] operations = operationsLine.Replace(" ", "").ToCharArray();

        SumProblemsMethod1(lines, operations);   
        SumProblemsMethod2(lines, operations);   
    }

    /*
        Method 1 is performing the operation on the numbers of each column as they are presented.

        123 328  51 64 
        45 64  387 23 
        6 98  215 314
        *   +   *   +  

        = (123 * 45 * 6) + (328 + 64 + 98) + (51 * 387 * 215) + (64 + 23 + 314)
        = 33210 + 490 + 4243455 + 401 
        = 4277556
    */
    private static void SumProblemsMethod1(string[] lines, char[] operations)
    {
        List<long> problemsSums = new List<long>();
        long grandTotal = 0;

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

        Console.WriteLine($"[DAY 6] Grand total of all cephalopod math problems with method 1 is: {grandTotal}");
    }

    /*
        Method 2 is performing the operation on the numbers of each column, one column at a time, right to left

        123 328  51 64 
         45 64  387 23 
          6 98  215 314
        *   +   *   +  

        = (4 + 431 + 623) + (175 * 581 * 32) + (8 + 248 + 369) + (356 * 24 * 1)
        = 1058 + 3253600 + 625 + 8544
        = 3263827
    */
    private static void SumProblemsMethod2(string[] lines, char[] operations)
    {
        List<long> problemsSums = new List<long>();
        long grandTotal = 0;

        // From the last line, get the # of columns per problem
        string lastLine = lines[lines.Length - 1];
        List<int> columnsPerProblem = new List<int>();
        for(int i = 0; i < lastLine.Length; i++)
        {
            if (lastLine[i] != ' ')
            {
                // If not the first problem, remove 1 space (divider between problems) to last entry
                if (columnsPerProblem.Count > 0)
                {
                    columnsPerProblem[columnsPerProblem.Count - 1]--;
                }
                columnsPerProblem.Add(1);
            }
            else
            {
                columnsPerProblem[columnsPerProblem.Count - 1]++;
            }
        }

        // Go one column at a time, right to left
        int columnIndex = lastLine.Length - 1;
        int problemNumber = operations.Length;      // -1 for every new problem
        int columnsRemainingInCurrentProblem = 0;

        while (columnIndex >= 0)
        {
            // If we need a new problem, we can fetch it from columnsPerProblem
            if (columnsRemainingInCurrentProblem == 0)
            {
                columnsRemainingInCurrentProblem = columnsPerProblem[columnsPerProblem.Count - 1];
                columnsPerProblem.RemoveAt(columnsPerProblem.Count - 1);
                problemNumber--;

                if (operations[problemNumber] == '+')
                {
                    problemsSums.Add(0);    // Add 0 as starting point for addition
                }
                else
                {
                    problemsSums.Add(1);    // Add 1 as starting point for multiplication
                }
            }

            // Construct the number based on digits in this row
            string numberStr = "";
            for(int row = 0; row < lines.Length - 1; row++)
            {
                char currentChar = lines[row][columnIndex];
                if (currentChar != ' ' && currentChar != '*' && currentChar != '+')
                {
                    numberStr += currentChar;
                }
            }

            if (!string.IsNullOrEmpty(numberStr))
            {
                problemsSums[problemsSums.Count - 1] = operations[problemNumber] == '+' 
                    ? problemsSums[problemsSums.Count - 1] + Int64.Parse(numberStr) 
                    : problemsSums[problemsSums.Count - 1] * Int64.Parse(numberStr);
            }

            // Prepare for next iteration
            columnIndex--;
            columnsRemainingInCurrentProblem--;

            // If columnsRemainingInCurrentProblem is 0, skip the next column which is a divider
            if (columnsRemainingInCurrentProblem == 0)
            {
                columnIndex--;
            }
        }

        // Calculate grand total
        foreach(long problemSum in problemsSums)
        {
            grandTotal += problemSum;
        }

        Console.WriteLine($"[DAY 6] Grand total of all cephalopod math problems with method 2 is: {grandTotal}");
    }
}

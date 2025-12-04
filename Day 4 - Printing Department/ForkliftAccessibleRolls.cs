/*

PART ONE

The rolls of paper (@) are arranged on a large grid; the Elves even have a helpful diagram (your puzzle input) indicating where everything is located.

For example:

..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.
The forklifts can only access a roll of paper if there are fewer than four rolls of paper in the eight adjacent positions. If you can figure out which rolls of paper the forklifts can access, they'll spend less time looking and more time breaking down the wall to the cafeteria.

In this example, there are 13 rolls of paper that can be accessed by a forklift (marked with x):

..xx.xx@x.
x@@.@.@.@@
@@@@@.x.@@
@.@@@@..@.
x@.@@@@.@x
.@@@@@@@.@
.@.@.@.@@@
x.@@@.@@@@
.@@@@@@@@.
x.x.@@@.x.
Consider your complete diagram of the paper roll locations. How many rolls of paper can be accessed by a forklift?

---

PART TWO

Once a roll of paper can be accessed by a forklift, it can be removed. Once a roll of paper is removed, the forklifts might be able to access more rolls of paper, which they might also be able to remove. How many total rolls of paper could the Elves remove if they keep repeating this process?

...

Stop once no more rolls of paper are accessible by a forklift. In this example, a total of 43 rolls of paper can be removed.

Start with your original diagram. How many rolls of paper in total can be removed by the Elves and their forklifts?

*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

public static class ForkliftAccessibleRolls
{
    public static int[][] directions = new int[][] { 
        new int[] {-1, -1}, new int[] {-1, 0}, new int[] {-1, 1},
        new int[] {0, -1},                     new int[] {0, 1},
        new int[] {1, -1},  new int[] {1, 0},  new int[] {1, 1}
    };

    public static void GetForkliftAccessibleRolls(string fileName)
    {
        string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 4 - Printing Department/" + fileName;

        // Typical BFS problem.
        // Since the input is provided in a text file, we can build a 2D array to represent the grid.
        char[][] grid = File.ReadAllLines(filepath).Select(line => line.ToCharArray()).ToArray();

        int accessibleRollsOnFirstPass = 0, accessibleRollsAfterAllPasses = 0, passNumber = 1;
        bool allAccessibleRollsRemoved = false;
        while (!allAccessibleRollsRemoved)
        {
            int accessibleRollCount = 0;
            List<int[]> rollsToRemove = new List<int[]>();

            for(int i = 0; i < grid.Length; i++)
            {
                for(int j = 0; j < grid[0].Length; j++)
                {
                    if (grid[i][j] != '@')  // We only need to consider cells which are rolls of paper
                    {
                        continue;
                    }

                    if (GetAdjacentRollsCount(grid, i, j) < 4)
                    {
                        accessibleRollCount++;
                        rollsToRemove.Add(new int[] { i, j });
                    }
                }
            }

            // Add results of this pass
            if (passNumber == 1)
            {
                accessibleRollsOnFirstPass = accessibleRollCount;
            }
            accessibleRollsAfterAllPasses += accessibleRollCount;
            passNumber++;

            // Break of prepare for next pass
            if (accessibleRollCount == 0)
            {
                allAccessibleRollsRemoved = true;
            }
            else
            {
                foreach (int[] roll in rollsToRemove)
                {
                    grid[roll[0]][roll[1]] = '.';   // Mark as removed
                }
            }
        }

        Console.WriteLine("[DAY 4] Forklift accessible rolls (1st pass): " + accessibleRollsOnFirstPass);
        Console.WriteLine("[DAY 4] Forklift accessible rolls (all passes): " + accessibleRollsAfterAllPasses);
    }

    public static int GetAdjacentRollsCount(char[][] grid, int row, int col)
    {
        int adjacentRollCount = 0;
        foreach (int[] direction in directions)
        {
            int newRow = row + direction[0];
            int newCol = col + direction[1];

            // Ensure this is a valid grid
            if (newRow >= 0 && newRow <= grid.Length - 1 && newCol >= 0 && newCol <= grid[0].Length - 1)
            {
                if (grid[newRow][newCol] == '@')
                {
                    adjacentRollCount++;
                }
            }
        }
        return adjacentRollCount;
    }
}
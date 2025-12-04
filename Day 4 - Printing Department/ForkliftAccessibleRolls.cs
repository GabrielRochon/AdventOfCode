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

*/

using System;
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

        int accessibleRollCount = 0;
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
                }
            }
        }
        Console.WriteLine("[DAY 4] Forklift accessible rolls: " + accessibleRollCount);
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
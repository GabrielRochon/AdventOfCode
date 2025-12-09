/*

PART ONE

The movie theater has a big tile floor with an interesting pattern. Elves here are redecorating the theater by switching out some of the square tiles in the big grid they form. Some of the tiles are red; the Elves would like to find the largest rectangle that uses red tiles for two of its opposite corners. They even have a list of where the red tiles are located in the grid (your puzzle input).

For example:

7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3
Showing red tiles as # and other tiles as ., the above arrangement of red tiles would look like this:

..............
.......#...#..
..............
..#....#......
..............
..#......#....
..............
.........#.#..
..............
You can choose any two red tiles as the opposite corners of your rectangle; your goal is to find the largest rectangle possible.

...

Ultimately, the largest rectangle you can make in this example has area 50. One way to do this is between 2,5 and 11,1:

..............
..OOOOOOOOOO..
..OOOOOOOOOO..
..OOOOOOOOOO..
..OOOOOOOOOO..
..OOOOOOOOOO..
..............
.........#.#..
..............
Using two red tiles as opposite corners, what is the largest area of any rectangle you can make?

---

PART TWO

The Elves just remembered: they can only switch out tiles that are red or green. So, your rectangle can only include red or green tiles.

In your list, every red tile is connected to the red tile before and after it by a straight line of green tiles. The list wraps, so the first red tile is also connected to the last red tile. Tiles that are adjacent in your list will always be on either the same row or the same column.

Using the same example as before, the tiles marked X would be green:

..............
.......#XXX#..
.......X...X..
..#XXXX#...X..
..X........X..
..#XXXXXX#.X..
.........X.X..
.........#X#..
..............
In addition, all of the tiles inside this loop of red and green tiles are also green. So, in this example, these are the green tiles:

..............
.......#XXX#..
.......XXXXX..
..#XXXX#XXXX..
..XXXXXXXXXX..
..#XXXXXX#XX..
.........XXX..
.........#X#..
..............
The remaining tiles are never red nor green.

...

The largest rectangle you can make in this example using only red and green tiles has area 24. One way to do this is between 9,5 and 2,3:

..............
.......#XXX#..
.......XXXXX..
..OOOOOOOOXX..
..OOOOOOOOXX..
..OOOOOOOOXX..
.........XXX..
.........#X#..
..............
Using two red tiles as opposite corners, what is the largest area of any rectangle you can make using only red and green tiles?

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class LargestRectangleIdentifier
{

    private static readonly int[][] AvailableDirections = [[-1,0], [1,0], [0,-1], [0,1]];

    public static void GetLargestRectangleAreaFromTwoTiles(string fileName, bool verboseMode = false)
    {
        string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 9 - Movie Theater/" + fileName;
        string[] lines = System.IO.File.ReadAllLines(filePath);
        
        // Corners furthest apart have the largest area
        // Hence, calculate distance between all points like yesterday's puzzle and get area of biggest one
        List<Tuple<int,int>> tiles = new List<Tuple<int,int>>();
        int maxRow = 0, maxColumn = 0, minRow = Int32.MaxValue, minColumn = Int32.MaxValue;
        foreach(string line in lines)
        {
            tiles.Add(new Tuple<int,int>(
                int.Parse(line.Split(',')[0]),
                int.Parse(line.Split(',')[1])));
        }

        // Find width and height of grid
        for(int i = 0; i < tiles.Count; i++)
        {
            maxColumn = Math.Max(tiles[i].Item1, maxColumn);
            minColumn = Math.Min(tiles[i].Item1, minColumn);
            maxRow = Math.Max(tiles[i].Item2, maxRow);
            minRow = Math.Min(tiles[i].Item2, minRow);
        }
        int width = maxColumn - minColumn + 1;
        int height = maxRow - minRow + 1;
        Console.WriteLine($"Grid size: {width}x{height}");

        // Offset tiles to be 0-based relative to minRow and minColumn
        List<Tuple<int,int>> offsetTiles = new List<Tuple<int,int>>();
        foreach(var tile in tiles)
        {
            offsetTiles.Add(new Tuple<int,int>(tile.Item1 - minColumn, tile.Item2 - minRow));
        }
        tiles = offsetTiles;

        // As per the puzzle input, the points are provided in order of adjacency
        // We can travel from one point to the other and add any intermediate point as a tile
        // Then, BFS from all those points to "fill up" the inside of the shape
        char[][] grid = new char[height][];
        for(int i = 0; i < height; i++)
        {
            grid[i] = new char[width];
            Array.Fill(grid[i], '.');
        }

        // We need to fill the shape formed by the adjacent red tiles
        Console.WriteLine("Adding outline between tiles and filling the resulting shape...");
        AddOutlineOfGreenTiles(tiles, ref grid);
        FillOutlineWithGreenTiles(tiles, verboseMode, ref grid);
        // PrintFilledShape(ref grid);

        // Finally, once filled, for each set of corners getting inspected, we need to ensure that 
        // all corners are part of the shape for the set of corners to be valid for part two
        Console.WriteLine("Calculating max areas...");
        long largestAreaRegardlessOfShape = 0;
        long largestAreaWithinShape = 0;

        for(int i = 0; i < tiles.Count; i++)
        {
            // Check if this is greater than the currently known width/height of the grid. If so, edit dimensions
            maxColumn = Math.Max(tiles[i].Item1, maxColumn);
            maxRow = Math.Max(tiles[i].Item2, maxRow);

            for(int j = i+1; j < tiles.Count; j++)
            {
                // Calculate area
                // Add +1 since a thin rectangle (points on the same axis) has a minimal length of 1
                long deltaX = Math.Abs(tiles[j].Item1 - tiles[i].Item1) + 1;
                long deltaY = Math.Abs(tiles[j].Item2 - tiles[i].Item2) + 1;
                long area = deltaX * deltaY;

                largestAreaRegardlessOfShape = Math.Max(largestAreaRegardlessOfShape, area);

                // Check if all corners are within the shape
                Tuple<int,int> intermediateCorner1 = new Tuple<int,int>(tiles[i].Item1, tiles[j].Item2);
                Tuple<int,int> intermediateCorner2 = new Tuple<int,int>(tiles[j].Item1, tiles[i].Item2);
                if (grid[intermediateCorner1.Item2][intermediateCorner1.Item1] != '.' 
                    && grid[intermediateCorner2.Item2][intermediateCorner2.Item1] != '.')
                {
                    largestAreaWithinShape = Math.Max(largestAreaWithinShape, area);
                }
            }
        }
        Console.WriteLine($"[DAY 9] The largest area possible with those corners regardless of the filled shape is {largestAreaRegardlessOfShape}");
        Console.WriteLine($"[DAY 9] The largest area possible with those corners within the filled shape is {largestAreaWithinShape}");
        
    }

    private static void AddOutlineOfGreenTiles(List<Tuple<int,int>> tiles, ref char[][] grid)
    {
        if (tiles.Count <= 1)
        {
            return;     // Nothing to do
        }

        for(int i = 0; i < tiles.Count; i++)
        {
            Tuple<int,int> currentTile = tiles[i];
            Tuple<int,int> nextTile = i < tiles.Count - 1 ? tiles[i+1] : tiles[0];  // If last pair, the next tile is the first one

            // Tiles are adjacent, either from the same row or column. Fill accordingly
            if (currentTile.Item1 == nextTile.Item1)    // Same column (y)
            {
                int lowerBound = Math.Min(currentTile.Item2, nextTile.Item2);
                int upperBound = Math.Max(currentTile.Item2, nextTile.Item2);
                for(int j = lowerBound; j <= upperBound; j++)
                {
                    grid[j][currentTile.Item1] = '#';
                }
            }
            else // Same row (x)
            {
                int lowerBound = Math.Min(currentTile.Item1, nextTile.Item1);
                int upperBound = Math.Max(currentTile.Item1, nextTile.Item1);
                for(int j = lowerBound; j <= upperBound; j++)
                {
                    grid[currentTile.Item2][j] = '#';
                }
            }
        }
    }

    private static Tuple<int, int> GetStartingPointOfFloodFillAlgoList(List<Tuple<int,int>> tiles, ref char[][] grid)
    {
        // Get pair with lowest column index. They will be adjacent so sliding window works
        int lowestColumnIndex = Int32.MaxValue, rowIndex = -1;
        for(int i = 0; i < tiles.Count - 1; i++)
        {
            Tuple<int,int> firstTile = tiles[i];
            Tuple<int,int> secondTile = tiles[i+1];

            if (firstTile.Item1 == secondTile.Item1 && firstTile.Item1 < lowestColumnIndex)
            {
                lowestColumnIndex = firstTile.Item1;
                rowIndex = Math.Min(firstTile.Item2, secondTile.Item2);
            }
        }

        // Output result. Lowest column + 1 and lowest row + 1. +1 ensures we are inside the shape
        return new Tuple<int, int>(lowestColumnIndex + 1, rowIndex + 1);
    }

    
    private static void FillOutlineWithGreenTiles(List<Tuple<int,int>> tiles, bool verboseMode, ref char[][] grid)
    {
        // Pick a point we know for sure is inside the shape
        // If we take the column furthest left, get the halfway point and add +1 column, 
        // we are guaranteed to be inside the shape
        Tuple<int,int> startingPoint = GetStartingPointOfFloodFillAlgoList(tiles, ref grid);
        // Console.WriteLine($"The starting point is: {startingPoint.Item1} : {startingPoint.Item2}");

        // BFS through the shape until there are no more tiles to fill
        Queue<Tuple<int,int>> pointsToExplore = new Queue<Tuple<int,int>>();
        pointsToExplore.Enqueue(startingPoint);
        while (pointsToExplore.Count > 0)
        {
            Tuple<int,int> currentTile = pointsToExplore.Dequeue();

            foreach(int[] direction in AvailableDirections)
            {
                int candidateX = currentTile.Item2 + direction[1];
                int candidateY = currentTile.Item1 + direction[0];
                if (grid[candidateX][candidateY] == '.')
                {
                    grid[candidateX][candidateY] = '#';
                    pointsToExplore.Enqueue(new Tuple<int,int>(candidateY, candidateX));
                }
            }
        }
    }

    // Show filled shape, for testing purposes
    private static void PrintFilledShape(ref char[][] grid)
    {
        for(int i = 0; i < grid.Length; i++)
        {
            for(int j = 0; j < grid[0].Length; j++)
            {
                Console.Write(grid[i][j]);
            }
            Console.WriteLine();
        }
    }
}

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

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class LargestRectangleIdentifier
{
    public static void GetLargestRectangleAreaFromTwoTiles(string fileName)
    {
        string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 9 - Movie Theater/" + fileName;
        string[] lines = System.IO.File.ReadAllLines(filePath);
        
        // Corners furthest apart have the largest area
        // Hence, calculate distance between all points like yesterday's puzzle and get area of biggest one
        List<Tuple<int,int>> tiles = new List<Tuple<int,int>>();
        foreach(string line in lines)
        {
            tiles.Add(new Tuple<int,int>(
                int.Parse(line.Split(',')[0]), 
                int.Parse(line.Split(',')[1])));
        }

        long largestArea = 0;

        for(int i = 0; i < tiles.Count; i++)
        {
            for(int j = i+1; j < tiles.Count; j++)
            {
                // Add +1 since a thin rectangle (points on the same axis) has a minimal length of 1
                long deltaX = Math.Abs(tiles[j].Item1 - tiles[i].Item1) + 1;
                long deltaY = Math.Abs(tiles[j].Item2 - tiles[i].Item2) + 1;
                long area = deltaX * deltaY;

                if (area > largestArea)
                {
                    largestArea = area;
                }
            }
        }

        Console.WriteLine($"[DAY 9] The largest area possible with those corners is {largestArea}");
    }
}
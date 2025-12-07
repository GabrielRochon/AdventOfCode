/*

PART ONE

You quickly locate a diagram of the tachyon manifold (your puzzle input). A tachyon beam enters the manifold at the location marked S; tachyon beams always move downward. Tachyon beams pass freely through empty space (.). However, if a tachyon beam encounters a splitter (^), the beam is stopped; instead, a new tachyon beam continues from the immediate left and from the immediate right of the splitter.

For example:

.......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............

...

This process continues until all of the tachyon beams reach a splitter or exit the manifold:

.......S.......
.......|.......
......|^|......
......|.|......
.....|^|^|.....
.....|.|.|.....
....|^|^|^|....
....|.|.|.|....
...|^|^|||^|...
...|.|.|||.|...
..|^|^|||^|^|..
..|.|.|||.|.|..
.|^|||^||.||^|.
.|.|||.||.||.|.
|^|^|^|^|^|||^|
|.|.|.|.|.|||.|
To repair the teleporter, you first need to understand the beam-splitting properties of the tachyon manifold. In this example, a tachyon beam is split a total of 21 times.

Analyze your manifold diagram. How many times will the beam be split?

*/

using System;
using System.Collections.Generic;

public static class TachyonBeamCounter
{
    public static void CountTachyonBeams(string fileName)
    {
        string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day 7 - Laboratories", fileName);
        string[] lines = System.IO.File.ReadAllLines(filePath);

        HashSet<int> beamPositions = new HashSet<int>();    // Prevent duplicates
        int splitCount = 0;

        foreach(string line in lines)
        {
            if (line.Contains('S'))
            {
                beamPositions.Add(line.IndexOf("S"));   // Starting beam
            }

            // Identify if beams need to be split
            for(int i = 0; i < line.Length; i++)
            {
                if (line[i] == '^' && beamPositions.Contains(i))
                {
                    splitCount++;

                    beamPositions.Remove(i);
                    if (i - 1 >= 0)
                    {
                        beamPositions.Add(i - 1);
                    }
                    if (i + 1 < line.Length)
                    {
                        beamPositions.Add(i + 1);
                    }
                }
            }
        }

        Console.WriteLine("[DAY 7] Total tachyon beam splits: " + splitCount);
    }
}
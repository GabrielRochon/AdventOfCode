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

---

PART TWO

With your analysis of the manifold complete, you begin fixing the teleporter. However, as you open the side of the teleporter to replace the broken manifold, you are surprised to discover that it isn't a classical tachyon manifold - it's a quantum tachyon manifold.

With a quantum tachyon manifold, only a single tachyon particle is sent through the manifold. A tachyon particle takes both the left and right path of each splitter encountered.

Since this is impossible, the manual recommends the many-worlds interpretation of quantum tachyon splitting: each time a particle reaches a splitter, it's actually time itself which splits. In one timeline, the particle went left, and in the other timeline, the particle went right.

To fix the manifold, what you really need to know is the number of timelines active after a single particle completes all of its possible journeys through the manifold.

In the above example, there are many timelines. For instance, there's the timeline where the particle always went left:

.......S.......
.......|.......
......|^.......
......|........
.....|^.^......
.....|.........
....|^.^.^.....
....|..........
...|^.^...^....
...|...........
..|^.^...^.^...
..|............
.|^...^.....^..
.|.............
|^.^.^.^.^...^.
|..............

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

    // Part two methods
    public static void CountQuantumTachyonTimelines(string fileName)
    {
        string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day 7 - Laboratories", fileName);
        string[] lines = System.IO.File.ReadAllLines(filePath);

        // Get starting position
        int startColumn = lines[0].IndexOf('S');
        var memo = new Dictionary<(int, int), long>();
        long timelineCount = ExploreTimeline(lines, 1, startColumn, memo);
        Console.WriteLine("[DAY 7] Total quantum timelines: " + timelineCount);
    }

    // Recursive function with memorization
    private static long ExploreTimeline(string[] lines, int row, int beamColumn, Dictionary<(int, int), long> memo)
    {
        // Base case
        if (row == lines.Length - 1)
        {
            return 1;   // Reached the end of the manifold, this was one timeline
        }

        // Add caching to prevent recalculating the # of timelines for a set row x col
        var key = (row, beamColumn);
        if (memo.ContainsKey(key))
        {
            return memo[key];
        }

        // Explore timelines
        long result, leftTimelines = 0, rightTimelines = 0;
        if (lines[row][beamColumn] == '^')
        {
            // Split beam
            if (beamColumn - 1 >= 0)
            {
                leftTimelines = ExploreTimeline(lines, row + 1, beamColumn - 1, memo);  // Go left
            }
            if (beamColumn + 1 < lines[0].Length)
            {
                rightTimelines = ExploreTimeline(lines, row + 1, beamColumn + 1, memo);  // Go right
            }
            result = leftTimelines + rightTimelines;
        }
        // Otherwise, go to next row
        else
        {
            result = ExploreTimeline(lines, row + 1, beamColumn, memo);
        }
        memo[key] = result;
        return result;
    }
}

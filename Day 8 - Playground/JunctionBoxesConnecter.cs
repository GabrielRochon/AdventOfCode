/*

PART ONE

Their plan is to connect the junction boxes with long strings of lights. Most of the junction boxes don't provide electricity; however, when two junction boxes are connected by a string of lights, electricity can pass between those two junction boxes.

The Elves are trying to figure out which junction boxes to connect so that electricity can reach every junction box. They even have a list of all of the junction boxes' positions in 3D space (your puzzle input).

For example:

162,817,812
57,618,57
906,360,560
592,479,940
352,342,300
466,668,158
542,29,236
431,825,988
739,650,466
52,470,668
216,146,977
819,987,18
117,168,530
805,96,715
346,949,466
970,615,88
941,993,340
862,61,35
984,92,344
425,690,689
This list describes the position of 20 junction boxes, one per line. Each position is given as X,Y,Z coordinates. So, the first junction box in the list is at X=162, Y=817, Z=812.

To save on string lights, the Elves would like to focus on connecting pairs of junction boxes that are as close together as possible according to straight-line distance. In this example, the two junction boxes which are closest together are 162,817,812 and 425,690,689.

By connecting these two junction boxes together, because electricity can flow between them, they become part of the same circuit. After connecting them, there is a single circuit which contains two junction boxes, and the remaining 18 junction boxes remain in their own individual circuits.

Now, the two junction boxes which are closest together but aren't already directly connected are 162,817,812 and 431,825,988. After connecting them, since 162,817,812 is already connected to another junction box, there is now a single circuit which contains three junction boxes and an additional 17 circuits which contain one junction box each.

The next two junction boxes to connect are 906,360,560 and 805,96,715. After connecting them, there is a circuit containing 3 junction boxes, a circuit containing 2 junction boxes, and 15 circuits which contain one junction box each.

The next two junction boxes are 431,825,988 and 425,690,689. Because these two junction boxes were already in the same circuit, nothing happens!

This process continues for a while, and the Elves are concerned that they don't have enough extension cables for all these circuits. They would like to know how big the circuits will be.

After making the ten shortest connections, there are 11 circuits: one circuit which contains 5 junction boxes, one circuit which contains 4 junction boxes, two circuits which contain 2 junction boxes each, and seven circuits which each contain a single junction box. Multiplying together the sizes of the three largest circuits (5, 4, and one of the circuits of size 2) produces 40.

Your list contains many junction boxes; connect together the 1000 pairs of junction boxes which are closest together. Afterward, what do you get if you multiply together the sizes of the three largest circuits?

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public static class JunctionBoxesConnecter
{
    public static void ConnectClosestJunctionBoxes(string fileName, int connectionCount, bool verboseMode = false)
    {
        string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 8 - Playground/" + fileName;
        string[] lines = System.IO.File.ReadAllLines(filePath);
        List<Tuple<int, int, long>>  distanceBetweenBoxes = new List<Tuple<int, int, long>>();

        // Calculate distance between all boxes
        for(int i = 0; i < lines.Length; i++)
        {
            for(int j = i+1; j < lines.Length; j++)
            {
                // Straight line distance in 3D: (x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2
                int deltaX = Int32.Parse(lines[j].Split(',')[0]) - Int32.Parse(lines[i].Split(',')[0]);
                int deltaY = Int32.Parse(lines[j].Split(',')[1]) - Int32.Parse(lines[i].Split(',')[1]);
                int deltaZ = Int32.Parse(lines[j].Split(',')[2]) - Int32.Parse(lines[i].Split(',')[2]);
                long distance = (long)(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ, 2));
                distanceBetweenBoxes.Add(new Tuple<int, int, long>(i, j, distance));
            }
        }

        // Sort distances
        distanceBetweenBoxes.Sort((a,b) => a.Item3.CompareTo(b.Item3));     // Item3 is the distance

        // Connect boxes based on distances and required connections to make (connectionCount)
        // Boxes are labeled with their line # from the input file
       List<List<int>> connectedBoxesList = new List<List<int>>();

        for(int i = 0; i < connectionCount; i++)
        {
            Tuple<int, int, long> groupToCheck = distanceBetweenBoxes[i];

            // Check if the first box or second box is in one of the lists
            bool foundBox1 = false, foundBox2 = false;
            int indexBox1 = -1, indexBox2 = -1;
            for(int j = 0; j < connectedBoxesList.Count; j++)
            {
                List<int> group = connectedBoxesList[j];
                if (group.Contains(groupToCheck.Item1))
                {
                    foundBox1 = true;
                    indexBox1 = j;
                }
                if (group.Contains(groupToCheck.Item2))
                {
                    foundBox2 = true;
                    indexBox2 = j;
                }
            }

            // Add to group with box 1
            if (foundBox1 && !foundBox2)
            {
                connectedBoxesList[indexBox1].Add(groupToCheck.Item2);
                if (verboseMode) Console.WriteLine($"Added {lines[groupToCheck.Item2]} to {lines[groupToCheck.Item1]}'s group");
            }
            // Add to group with box 2
            else if (foundBox2 && !foundBox1)
            {
                connectedBoxesList[indexBox2].Add(groupToCheck.Item1);
                if (verboseMode) Console.WriteLine($"Added {lines[groupToCheck.Item1]} to {lines[groupToCheck.Item2]}'s group");
            }
            // If the two boxes are already in the same group, nothing should happen -- we should move on to the next pair with the smallest distance
            else if (foundBox1 && foundBox2 && indexBox1 == indexBox2)
            {
                if (verboseMode) Console.WriteLine($"{lines[groupToCheck.Item1]} and {lines[groupToCheck.Item2]} are already in the same group, skipping.");
            }
            // If the two boxes are found but are in different groups, we should merge the groups
            else if (foundBox1 && foundBox2 && indexBox1 != indexBox2)
            {
                foreach(int box in connectedBoxesList[indexBox2])
                {
                    connectedBoxesList[indexBox1].Add(box);
                }
                connectedBoxesList[indexBox2] = new List<int>();    // Delete
                if (verboseMode) Console.WriteLine($"{lines[groupToCheck.Item1]} and {lines[groupToCheck.Item2]} are in different groups, merging groups.");
            }
            // If all previous checks fail, create new group with those 2 boxes
            else
            {
                connectedBoxesList.Add(new List<int> { groupToCheck.Item1, groupToCheck.Item2 });
                if (verboseMode) Console.WriteLine($"Added a new group with {lines[groupToCheck.Item1]} and {lines[groupToCheck.Item2]}");
            }
        }

        // Now that connections are made, sort by count of circuit size
        connectedBoxesList = connectedBoxesList.OrderByDescending(x => x.Count).ToList();
        int productResult = 1, circuitsToMultiplyAgainst = Math.Min(connectedBoxesList.Count, 3);
        for(int i = 0; i < circuitsToMultiplyAgainst; i++)
        {
            productResult *= connectedBoxesList[i].Count;
        }
        Console.WriteLine($"[DAY 8] The product of the size of the {circuitsToMultiplyAgainst} largest circuits is: {productResult}");
    }
}
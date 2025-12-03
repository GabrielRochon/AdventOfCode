/*

PART ONE

You arrive at the secret entrance to the North Pole base ready to start decorating. Unfortunately, the password seems to have been changed, so you can't get in. A document taped to the wall helpfully explains:

"Due to new security protocols, the password is locked in the safe below. Please see the attached document for the new combination."

The safe has a dial with only an arrow on it; around the dial are the numbers 0 through 99 in order. As you turn the dial, it makes a small click noise as it reaches each number.

The attached document (your puzzle input) contains a sequence of rotations, one per line, which tell you how to open the safe. A rotation starts with an L or R which indicates whether the rotation should be to the left (toward lower numbers) or to the right (toward higher numbers). Then, the rotation has a distance value which indicates how many clicks the dial should be rotated in that direction.

So, if the dial were pointing at 11, a rotation of R8 would cause the dial to point at 19. After that, a rotation of L19 would cause it to point at 0.

Because the dial is a circle, turning the dial left from 0 one click makes it point at 99. Similarly, turning the dial right from 99 one click makes it point at 0.

So, if the dial were pointing at 5, a rotation of L10 would cause it to point at 95. After that, a rotation of R5 could cause it to point at 0.

The dial starts by pointing at 50.

You could follow the instructions, but your recent required official North Pole secret entrance security training seminar taught you that the safe is actually a decoy. The actual password is the number of times the dial is left pointing at 0 after any rotation in the sequence.

For example, suppose the attached document contained the following rotations:

L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
Following these rotations would cause the dial to move as follows:

The dial starts by pointing at 50.
The dial is rotated L68 to point at 82.
The dial is rotated L30 to point at 52.
The dial is rotated R48 to point at 0.
The dial is rotated L5 to point at 95.
The dial is rotated R60 to point at 55.
The dial is rotated L55 to point at 0.
The dial is rotated L1 to point at 99.
The dial is rotated L99 to point at 0.
The dial is rotated R14 to point at 14.
The dial is rotated L82 to point at 32.
Because the dial points at 0 a total of three times during this process, the password in this example is 3.

Analyze the rotations in your attached document. What's the actual password to open the door?

---

PART TWO

You remember from the training seminar that "method 0x434C49434B" means you're actually supposed to count the number of times any click causes the dial to point at 0, regardless of whether it happens during a rotation or at the end of one.

Following the same rotations as in the above example, the dial points at zero a few extra times during its rotations:

The dial starts by pointing at 50.
The dial is rotated L68 to point at 82; during this rotation, it points at 0 once.
The dial is rotated L30 to point at 52.
The dial is rotated R48 to point at 0.
The dial is rotated L5 to point at 95.
The dial is rotated R60 to point at 55; during this rotation, it points at 0 once.
The dial is rotated L55 to point at 0.
The dial is rotated L1 to point at 99.
The dial is rotated L99 to point at 0.
The dial is rotated R14 to point at 14.
The dial is rotated L82 to point at 32; during this rotation, it points at 0 once.
In this example, the dial points at 0 three times at the end of a rotation, plus three more times during a rotation. So, in this example, the new password would be 6.

Be careful: if the dial were pointing at 50, a single rotation like R1000 would cause the dial to point at 0 ten times before returning back to 50!
    
*/

// Read the input from a file
using System;
using System.IO;
using System.Reflection;

public static class PasswordCracker
{
    public static void CrackPassword(string fileName)
    {
        string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Day 1 - Secret Entrance/" + fileName;
        int exactlyZeroCount = 0;
        int overallZeroCount = 0;
        try
        {
            string[] lines = File.ReadAllLines(filepath);
            int dialPosition = 50;

            foreach (string line in lines)
            {
                // Keep track of the dial position for every rotation, keep track of any 0s
                char direction = line[0];
                int distance = int.Parse(line.Substring(1));

                int passedZeros = GetTimesDialPassedZero(dialPosition, distance, direction);
                overallZeroCount += passedZeros;

                if (direction == 'L')
                {
                    dialPosition = (dialPosition - distance + 100) % 100;
                }
                else if (direction == 'R')
                {
                    dialPosition = (dialPosition + distance) % 100;
                }

                if (dialPosition == 0)
                {
                    exactlyZeroCount++;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while reading the file: " + e.Message);
        }

        // Return the number of times it was 0
        Console.WriteLine("[DAY 1] Password 1 is: " + exactlyZeroCount);
        Console.WriteLine("[DAY 1] Password 2 is: " + overallZeroCount);    // Wrong
    }

    public static int GetTimesDialPassedZero(int startPosition, int distance, char direction)
    {
        // No movement
        if (distance == 0)
        {
            return 0;
        }

        if (direction == 'R')
        {
            return (startPosition + distance) / 100;
        }
        else // direction == 'L'
        {
            if (startPosition > 0)
            {
                // We reach 0 at distances: startPosition, startPosition + 100, startPosition + 200, ...
                if (startPosition <= distance)
                {
                    return 1 + (distance - startPosition) / 100;
                }
                return 0;
            }
            else // startPosition == 0
            {
                return distance / 100;
            }
        }
    }
}
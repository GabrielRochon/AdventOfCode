/*
    ADVENT OF CODE
*/

using System;

internal static class EntryPoint
{
    public static void Main(string[] args)
    {
        // Day 1 - Secret Entrance
        PasswordCracker.CrackPassword("PasswordSequence.txt");
        Console.WriteLine();

        // Day 2 - Gift Shop
        InvalidAdder.AddSimpleInvalidIds("PuzzleInput.txt");
        InvalidAdder.AddComplexInvalidIds("PuzzleInput.txt");
        Console.WriteLine();
    }
}
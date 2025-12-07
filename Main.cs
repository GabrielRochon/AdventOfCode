/*
    ADVENT OF CODE 2025
*/

using System;

internal static class EntryPoint
{
    private static readonly int ColumnWidth = 100;

    public static void Main(string[] args)
    {
        PrintHeader();

        // Day 1 - Secret Entrance
        PasswordCracker.CrackPassword("PuzzleInput.txt");
        PrintDivisionLine();

        // Day 2 - Gift Shop
        InvalidAdder.AddSimpleInvalidIds("PuzzleInput.txt");
        InvalidAdder.AddComplexInvalidIds("PuzzleInput.txt");
        PrintDivisionLine();

        // Day 3 - Lobby
        MaxJoltageAdder.SumSimpleMaxVoltageOfPowerBanks("PuzzleInput.txt", verboseMode: false);
        MaxJoltageAdder.SumComplexMaxVoltageOfPowerBanks("PuzzleInput.txt", verboseMode : false);    // To track progress
        PrintDivisionLine();

        // Day 4 - Printing Department
        ForkliftAccessibleRolls.GetForkliftAccessibleRolls("PuzzleInput.txt");
        PrintDivisionLine();

        // Day 5 - Cafeteria
        FreshIngredientChecker.GetFreshAvailableIngredients("PuzzleInput.txt");
        PrintDivisionLine();

        // Day 6 - Trash Compactor
        CephalopodMathSolver.SumAllProblems("PuzzleInput.txt");
        PrintDivisionLine();

        // Day 7 - Laboratories
        TachyonBeamCounter.CountTachyonBeams("PuzzleInput.txt");
        TachyonBeamCounter.CountQuantumTachyonTimelines("PuzzleInput.txt");
        PrintDivisionLine();
    }

    private static void PrintHeader()
    {
        Console.WriteLine(new string('=', ColumnWidth));
        Console.WriteLine("ADVENT OF CODE 2025".PadLeft((ColumnWidth + "ADVENT OF CODE 2025".Length) / 2));
        Console.WriteLine(new string('=', ColumnWidth));
    }

    private static void PrintDivisionLine()
    {
        Console.WriteLine(new string('-', ColumnWidth));
    }
}

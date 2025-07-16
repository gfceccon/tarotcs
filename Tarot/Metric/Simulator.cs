using System;

namespace Tarot.Metric;

// TODO Setup simulator and battery test
public class Simulator
{
    public static void RunSimulation(SimulateArgs args)
    {
        // Implement the simulation logic here
        Console.WriteLine("Starting simulation...");
        // Example usage of arguments
        Console.WriteLine($"Number of games: {args.NumberOfGames}");
        Console.WriteLine($"Strategy: {args.Strategy}");
        Console.WriteLine($"Verbose output: {args.Verbose}");

        // Dummy simulation
        for (int i = 0; i < args.NumberOfGames; i++)
        {
            // Simulation logic for each game
            if (args.Verbose)
            {
                Console.WriteLine($"Game {i + 1} completed.");
            }
        }
    }
}

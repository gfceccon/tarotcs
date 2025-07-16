using System;
using Tarot;
using Tarot.Metric;

class Program
{
    static void Main(string[] args)
    {
        var parsedArgs = SimulateParser.Parse(args);
        Console.WriteLine($"Output file: {parsedArgs.Output}");
        Console.WriteLine($"Number of games: {parsedArgs.NumberOfGames}");
        Console.WriteLine($"Strategy: {parsedArgs.Strategy}");
        Console.WriteLine($"Verbose output: {parsedArgs.Verbose}");
        Metrics.Instance.SetOutputFile(parsedArgs.Output);
    }
}


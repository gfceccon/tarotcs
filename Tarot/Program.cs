using System;
using Tarot;

class Program
{
    static void Main(string[] args)
    {
        var parsedArgs = SimulateParser.Parse(args);
        if (parsedArgs == null)
        {
            Console.WriteLine("Erro ao analisar os argumentos.");
            throw new ArgumentException("Argumentos inválidos.");
        }
        Console.WriteLine($"Output: {parsedArgs.Output}");
        Console.WriteLine($"Games: {parsedArgs.NumberOfGames}");
        Console.WriteLine($"Strategies: {parsedArgs.Strategy}");
        Console.WriteLine($"Verbose: {parsedArgs.Verbose}");
        // Aqui você pode adicionar a lógica principal do programa usando os argumentos
    }
}


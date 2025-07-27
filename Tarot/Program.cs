using System.Runtime.InteropServices;
using Tarot;
using Tarot.Game;
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

        // Estimativa de tamanho via GC.GetTotalMemory
        long memAntes = GC.GetTotalMemory(true);
        var tarotGame = new TarotGameState();
        long memDepois = GC.GetTotalMemory(true);
        Console.WriteLine($"TarotGame estimativa de memória: {memDepois - memAntes} bytes");

        // Estimativa de tamanho via Marshal.SizeOf para structs
        Console.WriteLine($"CardAction tamanho (Marshal): {Marshal.SizeOf<CardAction>()} bytes");
        Console.WriteLine($"Player tamanho (Marshal): {Marshal.SizeOf<Player>()} bytes");
        Console.WriteLine($"BidAction tamanho (Marshal): {Marshal.SizeOf<BidAction>()} bytes");
        Console.WriteLine($"GenericAction tamanho (Marshal): {Marshal.SizeOf<GenericAction>()} bytes");

        // Tarot game state info
        Console.WriteLine($"TarotGameState: \n {tarotGame}");

        Metrics.Instance.SetOutputFile(parsedArgs.Output);
    }
}


namespace Tarot;
using CommandLine;
using Tarot.Game;

public class SimulateArgs
{
    [Option('o', "output", Required = false, Default ="output.csv", HelpText = "Arquivo de saída.")]
    public string? Output { get; set; }

    [Option('g', "games", Required = false, Default = 10, HelpText = "Número de jogos.")]
    public int NumberOfGames { get; set; }

    [Option('s', "strategy", Required = false, Default = Strategy.Random, HelpText = "Estratégia de simulação.")]
    public Strategy Strategy { get; set; }

    [Option('v', "verbose", Required = false, Default = false, HelpText = "Saída detalhada.")]
    public bool Verbose { get; set; }
}

public static class SimulateParser
{
    public static SimulateArgs? Parse(string[] args)
    {
        SimulateArgs? parsed = null;
        Parser.Default.ParseArguments<SimulateArgs>(args)
            .WithParsed(opts => parsed = opts)
            .WithNotParsed(errs => throw new ArgumentException("Argumentos inválidos."));
        return parsed;
    }
}

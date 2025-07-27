namespace Tarot.Metric;

using Tarot.Game;

// TODO Metrics handler
public class Metrics
{
    private static Metrics? _instance;
    private string _outputFile = "metrics.csv";

    public static Metrics Instance => _instance ??= new Metrics();

    private readonly Dictionary<string, List<long>> _data = [];


    private Metrics()
    {
    }

    public void SetOutputFile(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("O caminho do arquivo de saída não pode ser nulo ou vazio.");
        _outputFile = filePath;
    }

    public void RecordMetric(string metricName, TarotGameState state, long value)
    {
        if (!_data.ContainsKey(metricName))
            _data[metricName] = [];

        _data[metricName].Add(value);
    }
}
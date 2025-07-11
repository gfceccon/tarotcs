namespace Tarot.Metric;

using Tarot.Game;


public class Metrics
{
    private static Metrics? _instance;

    public static Metrics Instance => _instance ??= new Metrics();

    private Dictionary<string, List<long>> _data = new();


    private Metrics()
    {
    }

    public void SetOutputFile(string filePath)
    {
        // Logic to set output file for metrics
    }

    public void RecordMetric(string metricName, TarotGameState state, long value)
    {
        if (!_data.ContainsKey(metricName))
        {
            _data[metricName] = new List<long>();
        }

        _data[metricName].Add(value);
    }
}
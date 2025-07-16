namespace Tarot;

public class Rng
{
    private static readonly int SEED = 10000;
    public static readonly Random Random = new(SEED);
}
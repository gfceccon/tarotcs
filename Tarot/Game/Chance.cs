namespace Tarot.Game;

public readonly struct ChanceAction(byte value, ActionKind kind, float probability)
{
    public byte Value { get; } = value;
    public float Probability { get; } = probability;
    public ActionKind Kind { get; } = kind;
}

public static class Chance
{
    public static ChanceAction[] GetBidChances(TarotGameState state)
    {
        // TODO Implement logic to determine bid chances
        throw new NotImplementedException("Bid chances not implemented yet.");
    }

    public static ChanceAction[] GetDiscardChances(TarotGameState state)
    {
        // TODO Implement logic to determine discard chances
        throw new NotImplementedException("Bid chances not implemented yet.");
    }

    public static ChanceAction[] GetDeclarationChances(TarotGameState state)
    {
        // TODO Implement logic to determine declaration chances
        throw new NotImplementedException("Bid chances not implemented yet.");
    }
}
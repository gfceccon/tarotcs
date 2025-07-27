namespace Tarot.Game;

public readonly struct ChanceAction(byte value, ActionKind kind, float probability)
{
    public byte Value { get; } = value;
    public float Probability { get; } = probability;
    public ActionKind Kind { get; } = kind;
}

// TODO: Implement heuristic and/or machine learning to determine the best action based on game state
public static class Chance
{
    public static ChanceAction[] GetBidChances(TarotGameState state)
    {
        var legalBids = LegalAction.GetLegalBidActions(state);
        var chances = new List<ChanceAction>();
        foreach (var bid in legalBids)
        {
            float probability = 1.0f / legalBids.Length;
            chances.Add(new ChanceAction(bid.Value, ActionKind.Bid, probability));
        }
        return chances.ToArray();
    }

    public static ChanceAction[] GetDiscardChances(TarotGameState state)
    {
        var legalDiscards = LegalAction.GetLegalDiscardActions(state);
        var chances = new List<ChanceAction>();
        foreach (var card in legalDiscards)
        {
            float probability = 1.0f / legalDiscards.Length;
            chances.Add(new ChanceAction(card.Value, ActionKind.Card, probability));
        }
        return chances.ToArray();
    }

    public static ChanceAction[] GetDeclarationChances(TarotGameState state)
    {
        var legalDeclarations = LegalAction.GetLegalDeclarationActions(state);
        var chances = new List<ChanceAction>();
        foreach (var declaration in legalDeclarations)
        {
            float probability = 1.0f / legalDeclarations.Length;
            chances.Add(new ChanceAction(declaration.Value, ActionKind.Declaration, probability));
        }
        return chances.ToArray();
    }
}
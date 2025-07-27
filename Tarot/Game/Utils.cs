namespace Tarot.Game;

// TODO Util for fool and heuristics
public static class Utils
{
    public static int GetFoolReplacement(TarotGameState state)
    {
        // TODO Implement logic to determine the fool replacement
        throw new NotImplementedException();
    }

    public static Player GetTrickWinner(TarotGameState state)
    {
        var leadSuit = Card.Suit(state.CurrentTrick[0]);
        var bestCard = state.CurrentTrick[0];
        for (int i = 1; i < state.CurrentTrick.Length; i++)
        {
            var currentCard = state.CurrentTrick[i];

            var bestRank = Card.Rank(bestCard);
            var currentSuit = Card.Suit(currentCard);
            var currentRank = Card.Rank(currentCard);

            if (currentSuit != leadSuit && !Card.IsTrump(currentCard))
                continue;

            if (leadSuit != Constants.Trumps && currentSuit == Constants.Trumps)
            {
                bestCard = currentCard;
                leadSuit = currentSuit;
            }
            else if (currentSuit == leadSuit && currentRank > bestRank)
            {
                bestCard = currentCard;
            }
        }
        var playerIndex = Array.IndexOf(state.CurrentTrick, bestCard);
        return new Player((byte)playerIndex);
    }

    public static int GetChienDiscard(TarotGameState state)
    {
        // TODO Implement logic to determine the chien discard
        throw new NotImplementedException();
    }
}
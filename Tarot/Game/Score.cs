namespace Tarot.Game;

/// <summary>
/// This class contains the score actions for the Tarot game.
/// It defines the possible declarations that can be made by players.
/// It also provides methods to calculate partial and total scores.
/// </summary>
public static class Score
{
    public static readonly Dictionary<int, float> MinimumPoints = new()
    {
        {0, 51f},
        {1, 46f},
        {2, 41f},
        {3, 36f},
    };

    /// <summary>
    /// Returns the partial score for the Tarot taker.
    /// If the taker achieved the bid, the score is positive.
    /// If the taker failed, the score is negative.
    /// </summary>
    /// <param name="game">The game state</param>
    /// <returns>The current private know points</returns>
    public static Tuple<float, bool> PartialScore(TarotGameState state)
    {
        Player taker = state.Taker;
        float score = 0;
        int bouts = 0;
        // TODO Change logic to new tricks system
        for (int trick = 0; trick < Constants.TricksSize; trick++)
        {
            if (state.PlayedTricks[trick * Constants.TrickSize] == taker.Value)
            {
                for (int j = 1; j < Constants.TrickSize && j < state.PlayedTricks.Length; j++)
                {
                    byte card = state.PlayedTricks[trick + j];
                    if (Card.IsBout(card))
                        bouts++;
                    score += Card.Value(card);
                }
            }
            if (state.FoolTrickIndex == trick && state.FoolPlayer == taker.Value)
            {
                bouts++;
                score += Card.Value(Constants.Fool);
            }
        }
        var achieved = score >= MinimumPoints[state.TakerBid];
        return Tuple.Create(score, achieved);
    }

    /// <summary>
    /// Returns the total score for the Tarot game.
    /// This includes bonus points for declarations.
    /// It returns the total score for the taker.
    /// If the taker failed, the score is negative.
    /// </summary>
    /// <param name="game">The game state</param>
    /// <returns>The final taker score considering bonus</returns>
    public static float TotalScore(TarotGame game)
    {
        // TODO Total Score
        // Only taker can declare chelem.
        // Any player can declare poignee.
        // The fool does not counter for chelem
        // If the declared poignee is not achieved, the score goes to the other team.
        // Petit goes to whoever won the last trick, and deduced from the other team. Times the multiplier.
        // Remember score is equal to (25 + abs(target - hand)) * multiplier + poignee + chelem
        throw new NotImplementedException("This method is not implemented yet.");
    }
}
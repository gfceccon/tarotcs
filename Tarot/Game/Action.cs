namespace Tarot.Game;

public static class Action
{

    /// <summary>
    /// Declare a bid from the current player.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The bid code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyBid(TarotGameState state, BidAction _action)
    {
        var action = _action.Value;
        if (action < Constants.StartBid || action > Constants.EndBid)
            throw new ArgumentOutOfRangeException(nameof(_action), "Bid action is out of range.");
        if (state.Bids.Length > Constants.MaxBidding)
            throw new InvalidOperationException("Maximum number of bids reached.");

        // Set the bid for the current player
        state.Bids[state.BidCounter++] = new BidAction(action);

        // Check if all players have made their bids
        if (state.BidCounter == Constants.Players)
        {
            // If all players have made their bids, determine the taker
            var maxBid = state.Bids.MaxBy(b => b.Value);
            var takerIndex = Array.IndexOf(state.Bids, maxBid);
            state.Taker = new Player((byte)takerIndex);
            state.Phase = Phase.Chien; // Move to the chien phase
            state.DiscardCounter = 0; // Reset discard counter for chien
            return state.Taker;
        }
        else
        {
            // Move to the next player
            var currentPlayer = state.Current.Value;
            return new Player((byte)((currentPlayer + 1) % Constants.Players));
        }
    }

    /// <summary>
    /// Discard a card to the chien pile.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The card code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyChien(TarotGameState state, CardAction _action)
    {
        var action = _action.Value;
        if (action < Constants.StartCard || action > Constants.EndCard)
            throw new ArgumentOutOfRangeException(nameof(_action), "Card action is out of range.");
        if (state.DiscardCounter < Constants.ChienSize)
        {
            // Add the card to the chien pile and increment the discard counter
            state.Chien[state.DiscardCounter++] = new CardAction(action);
            if (state.DiscardCounter == Constants.ChienSize)
                state.Phase = Phase.PoigneeDeclaration; // Move to the next phase after chien
            return state.Taker; // Continue with the taker
        }
        else throw new InvalidOperationException("Chien pile is full.");
    }

    /// <summary>
    /// Declare a chelem or poignee, or none.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The action code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyDeclaration(TarotGameState state, DeclarationAction _action)
    {
        var action = _action.Value;
        if (action < Constants.StartDeclaration || action > Constants.EndDeclaration)
            throw new ArgumentOutOfRangeException(nameof(_action),
            "Declaration action is out of range.");
        return state.Current; // Continue with the current player
    }

    /// <summary>
    /// Play a card.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The card code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyCard(TarotGameState state, CardAction _action)
    {
        var action = _action.Value;
        if (action < Constants.StartCard || action > Constants.EndCard)
            throw new ArgumentOutOfRangeException(nameof(_action), "Card action is out of range.");

        state.CurrentTrick[state.TrickCounter++] = new CardAction(action);
        if (state.TrickCounter == Constants.TrickSize)
        {
            // Determine the winner of the trick
            var winner = Utils.GetTrickWinner(state);
            var trickIndex = state.TricksCounter / Constants.TrickSize;
            state.Winners[trickIndex] = winner;
            state.CurrentTrick = new CardAction[Constants.TrickSize]; // Reset current trick
            state.TrickCounter = 0; // Reset trick counter

            if (state.TricksCounter == Constants.TricksSize)
                state.Phase = Phase.End; // Move to end phase if all tricks are played
            return winner; // Return the winner of the trick
        }

        // TODO Fool logic

        // TODO Next phase and player
        if (state.TricksCounter == 0 && state.DeclarationCounter < Constants.Players)
            state.Phase = Phase.PoigneeDeclaration;
        else if (state.TricksCounter == Constants.TricksSize)
            state.Phase = Phase.End;
        return state.Current;
    }
}
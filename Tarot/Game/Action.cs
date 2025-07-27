namespace Tarot.Game;

public static class Action
{

    /// <summary>
    /// Declare a bid from the current player.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The bid code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyBid(TarotGameState state, BidAction action)
    {
        if (action < Constants.StartBid || action > Constants.EndBid)
            throw new ArgumentOutOfRangeException(nameof(action), "Bid action is out of range.");
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
            state.DiscardCounter = 0; // Reset discard counter for chien
        }
        return TarotGame.Next(state);
    }

    /// <summary>
    /// Discard a card to the chien pile.
    /// If the discard pile is full, fill the player's hand with the chien cards
    /// that are not in the discard pile.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The card code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyChien(TarotGameState state, CardAction action)
    {
        if (action < Constants.StartCard || action > Constants.EndCard)
            throw new ArgumentOutOfRangeException(nameof(action), "Card action is out of range.");

        if (state.DiscardCounter < Constants.ChienSize)
        {
            state.Discard[state.DiscardCounter++] = action;
            return TarotGame.Next(state);
        }

        var startIndex = state.Taker.Index * Constants.HandSize;
        var playerHand = state.PlayerHands
            .Skip(startIndex)
            .Take(Constants.HandSize)
            .Concat(state.Chien)
            .Where(card => !state.Discard.Contains(card))
            .ToArray();

        Array.Copy(playerHand, 0, state.PlayerHands, startIndex, playerHand.Length);

        state.Chien = state.Discard;
        state.DeclarationCounter = 0;
        return TarotGame.Next(state);
    }

    /// <summary>
    /// Declare a chelem or poignee, or none.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The action code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyDeclaration(TarotGameState state, DeclarationAction action)
    {
        if (action < Constants.StartDeclaration || action > Constants.EndDeclaration)
            throw new ArgumentOutOfRangeException(nameof(action),
            "Declaration action is out of range.");

        state.Declarations[state.DeclarationCounter++] = action;
        if (state.DeclarationCounter == 1)
        {
            state.Phase = Phase.PoigneeDeclaration; // Move to poignee declaration phase
            return state.Taker; // Continue with the taker
        }
        state.Phase = Phase.Play; // Move to the play phase after all declarations            
        return state.Current; // Continue with the current player
    }

    /// <summary>
    /// Play a card.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The card code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyCard(TarotGameState state, CardAction action)
    {
        if (action < Constants.StartCard || action > Constants.EndCard)
            throw new ArgumentOutOfRangeException(nameof(action), "Card action is out of range.");

        state.CurrentTrick[state.TrickCounter++] = new CardAction(action);
        if (action == Constants.Fool)
        {
            state.FoolTrickIndex = state.TricksCounter;
            state.FoolPlayer = state.Current;
        }
        if (state.TrickCounter < Constants.TrickSize)
            return TarotGame.Next(state);

        // Determine the winner of the trick
        var winner = Utils.GetTrickWinner(state);
        var trickIndex = state.TricksCounter;
        state.Winners[trickIndex] = winner;
        state.CurrentTrick = new CardAction[Constants.TrickSize]; // Reset current trick
        state.TrickCounter = 0; // Reset trick counter
        state.TricksCounter++; // Increment tricks counter

        return TarotGame.Next(state);
    }
}
namespace Tarot.Game;

using BidAction = byte;
using DeclarationAction = byte;
using PlayAction = byte;
using Player = byte;

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
        state.Bids[state.BidCounter++] = action;

        // Check if all players have made their bids
        if (state.BidCounter == Constants.Players)
        {
            // If all players have made their bids, determine the taker
            state.Taker = (byte)Array.IndexOf(state.Bids, state.Bids.Max());
            state.Phase = Phase.Chien; // Move to the chien phase
            state.DiscardCounter = 0; // Reset discard counter for chien
            return state.Taker;
        }
        // Move to the next player
        else return (byte)((state.Current + 1) % (Constants.Players + 1));
    }

    /// <summary>
    /// Discard a card to the chien pile.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The card code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyChien(TarotGameState state, PlayAction action)
    {
        if (action < Constants.StartCard || action > Constants.EndCard)
            throw new ArgumentOutOfRangeException(nameof(action), "Card action is out of range.");
        if (state.DiscardCounter < Constants.ChienSize)
        {
            // Add the card to the chien pile and increment the discard counter
            state.Chien[state.DiscardCounter++] = action;
            if (state.DiscardCounter == Constants.ChienSize)
                state.Phase = Phase.Declaration; // Move to the next phase after chien
            return state.Taker; // Continue with the taker
        }
        else throw new InvalidOperationException("Chien pile is full.");
    }

    /// <summary>
    /// Declare a chelem or poignee.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The action code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyDeclaration(TarotGameState state, byte action)
    {
        if (action < Constants.StartDeclaration || action > Constants.EndDeclaration)
            throw new ArgumentOutOfRangeException(nameof(action), "Declaration action is out of range.");
        if (state.DeclarationCounter < Constants.MaxDeclarations)
        {
            // Set the declaration for the current player
            state.Declarations[state.DeclarationCounter++] = action;

            if (action == Declaration.Chelem)
                state.ChelemDeclared = true;

            // Check if all players have made their declarations
            if (state.DeclarationCounter == Constants.Players)
            {
                state.Phase = Phase.Playing; // Move to the playing phase
                state.TrickCounter = 0; // Reset trick counter for playing
                return state.Taker; // Continue with the taker
            }
            else return (byte)((state.Current + 1) % Constants.Players);
        }
        else throw new InvalidOperationException("Maximum number of declarations reached.");
    }

    /// <summary>
    /// Play a card.
    /// </summary>
    /// <param name="state">The current game state</param>
    /// <param name="action">The card code</param>
    /// <returns>The next player to play</returns>
    public static Player ApplyCard(TarotGameState state, PlayAction action)
    {
        // TODO Apply Card Action
        // Implementation for applying a card play action
        throw new NotImplementedException("This method is not implemented yet.");
    }
}
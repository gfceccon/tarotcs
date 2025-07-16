namespace Tarot.Game;

/// <summary>
/// This enum represents the different phases of the Tarot game.
/// The game progresses through these phases:
/// 1. Bidding: Players place their bids, in 4 rounds.
/// 2. Declaration: Players declare their intentions, such as Chelem or Poignee.
/// 3. Chien: The chien cards are exchanged with the player who won the bid.
/// 4. Playing: Players play their cards in tricks.
/// </summary>
public enum Phase
{
    Bidding,
    Declaration,
    Chien,
    Playing,
    End
}
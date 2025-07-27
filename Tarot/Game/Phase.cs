namespace Tarot.Game;

/// <summary>
/// This enum represents the different phases of the Tarot game.
/// The game progresses through these phases:
/// 1. Bidding: Players place their bids, in 4 rounds.
/// 2. ChelemDeclaration: Taker declare chelem.
/// 3. PoigneeDeclaration: Players declare poignee.
/// 4. Chien: The chien cards are exchanged with the player who won the bid.
/// 5. Playing: Players play their cards in tricks.
/// </summary>
public enum Phase
{
    Bidding,
    Chien,
    ChelemDeclaration,
    PoigneeDeclaration,
    Playing,
    End
}
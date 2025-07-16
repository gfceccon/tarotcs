namespace Tarot.Game;

using BidAction = byte;

/// <summary>
/// This class contains the bid actions for the Tarot game.
/// The bid actions are used to calculate the score
/// Possible bid actions are:
/// - Passe: The player passes their turn.
/// - Petit: The player bids for a small game, multiplier 1.
/// - Garde: The player bids for a guard game, multiplier 2.
/// - Garde Sans: The player bids for a guard game without the dog, multiplier 4.
/// - Garde Contre: The player bids for a guard game against the dog, multiplier 6.
/// </summary>
public static class Bid
{
    public const BidAction Passe = Constants.StartBid + 0;
    public const BidAction Petit = Constants.StartBid + 1;
    public const BidAction Garde = Constants.StartBid + 2;
    public const BidAction GardeSans = Constants.StartBid + 3;
    public const BidAction GardeContre = Constants.StartBid + 4;

    /// <summary>
    /// Returns the name of the bid action.
    /// </summary>
    /// <param name="bid">The bid action</param>
    /// <returns>A string with the name</returns>
    public static string Name(BidAction bid)
    {
        return bid switch
        {
            Petit => "Petit",
            Garde => "Garde",
            GardeSans => "Garde Sans Chien",
            GardeContre => "Garde Contre Chien",
            Passe => "Passe",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Returns the multiplier for the bid action.
    /// From 1 to 6, depending on the bid type.
    /// Passe has no multiplier.
    /// </summary>
    /// <param name="bid">The bid action</param>
    /// <returns>Int with score multiplier</returns>
    public static int Multiplier(BidAction bid)
    {
        return bid switch
        {
            Petit => 1,
            Garde => 2,
            GardeSans => 4,
            GardeContre => 6,
            _ => 0
        };
    }
}
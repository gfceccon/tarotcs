namespace Tarot.Game;

public readonly struct BidAction(byte value) : IEquatable<BidAction>
{
    public byte Value { get; } = value;
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return obj is BidAction bid && Value == bid.Value;
    }

    public bool Equals(BidAction other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public static bool operator ==(BidAction left, BidAction right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BidAction left, BidAction right)
    {
        return !(left == right);
    }
    
    public static bool operator <(BidAction left, BidAction right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >(BidAction left, BidAction right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(BidAction left, byte right)
    {
        return left.Value < right;
    }

    public static bool operator >(BidAction left, byte right)
    {
        return left.Value > right;
    }

    public static implicit operator byte(BidAction action) => action.Value;
    public static implicit operator BidAction(byte value) => new(value);
}

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
    public static readonly BidAction Passe = new(Constants.StartBid + 0);
    public static readonly BidAction Petit = new(Constants.StartBid + 1);
    public static readonly BidAction Garde = new(Constants.StartBid + 2);
    public static readonly BidAction GardeSans = new(Constants.StartBid + 3);
    public static readonly BidAction GardeContre = new(Constants.StartBid + 4);

    /// <summary>
    /// Returns the name of the bid action.
    /// </summary>
    /// <param name="bid">The bid action</param>
    /// <returns>A string with the name</returns>
    public static string Name(BidAction bid)
    {
        if (bid == Passe) return "Passe";
        if (bid == Petit) return "Petit";
        if (bid == Garde) return "Garde";
        if (bid == GardeSans) return "Garde Sans Chien";
        if (bid == GardeContre) return "Garde Contre Chien";
        return "Unknown";
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
        if (bid == Passe) return 0;
        if (bid == Petit) return 1;
        if (bid == Garde) return 2;
        if (bid == GardeSans) return 4;
        if (bid == GardeContre) return 6;
        throw new ArgumentOutOfRangeException(nameof(bid), "Bid action is out of range.");
    }
}
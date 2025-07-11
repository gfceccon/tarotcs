namespace Tarot.Game;

using BidAction = byte;
public static class Bid
{
     public const BidAction Pass = Constants.MaxCards + 0;
     public const BidAction Petit = Constants.MaxCards + 1;
     public const BidAction Garde = Constants.MaxCards + 2;
     public const BidAction GardeSans = Constants.MaxCards + 3;
     public const BidAction GardeContre = Constants.MaxCards + 4;
        
    public static string Name(BidAction bid)
    {
        return bid switch
        {
            Bid.Petit => "Petit",
            Bid.Garde => "Garde",
            Bid.GardeSans => "Garde Sans",
            Bid.GardeContre => "Garde Contre",
            Bid.Pass => "Pass",
            _ => "Unknown"
        };
    }

    public static int Multiplier(BidAction bid)
    {
        return bid switch
        {
            Bid.Petit => 1,
            Bid.Garde => 2,
            Bid.GardeSans => 4,
            Bid.GardeContre => 6,
            _ => 0
        };
    }
}
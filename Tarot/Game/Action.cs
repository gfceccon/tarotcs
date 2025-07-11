namespace Tarot.Game;


public static class Action
{
    public static byte[] GetLegalBidActions(byte[] currentBids)
    {
        var legalBids = new List<byte>
        {
            Bid.Pass,
            Bid.Petit,
            Bid.Garde,
            Bid.GardeSans,
            Bid.GardeContre,
        };
        var maxBid = currentBids.Max();
        
        return legalBids.Where(bid => bid == Bid.Pass || bid > maxBid).ToArray();
    }
    
    public static byte[] GetLegalPlayActions()
    {
        throw new NotImplementedException();
    }
    public static byte[] GetLegalDiscardActions()
    {
        throw new NotImplementedException();
    }
    public static byte[] GetLegalDeclareActions()
    {
        throw new NotImplementedException();
    }

    public static List<Tuple<byte, float>> GetBidChances()
    {
        throw new NotImplementedException();
    }

    public static List<Tuple<byte, float>> GetDiscardChances()
    {
        throw new NotImplementedException();
    }

    public static List<Tuple<byte, float>> GetDeclareChances()
    {
        throw new NotImplementedException();
    }
}
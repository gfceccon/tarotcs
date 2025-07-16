using System;

namespace Tarot.Game;


// TODO All Legal Actions
public class LegalAction
{
    public static byte[] GetLegalBidActions(TarotGameState state)
    {
        byte[] currentBids = state.Bids;
        var legalBids = new List<byte>
        {
            Bid.Passe,
            Bid.Petit,
            Bid.Garde,
            Bid.GardeSans,
            Bid.GardeContre,
        };
        var maxBid = currentBids.Max();

        return [.. legalBids.Where(bid => bid == Bid.Passe || bid > maxBid)];
    }

    public static byte[] GetLegalPlayActions(TarotGameState state)
    {
        throw new NotImplementedException();
    }
    public static byte[] GetLegalDiscardActions(TarotGameState state)
    {
        throw new NotImplementedException();
    }
    public static byte[] GetLegalDeclareActions(TarotGameState state)
    {
        throw new NotImplementedException();
    }

    public static List<Tuple<byte, float>> GetBidChances(TarotGameState state)
    {
        throw new NotImplementedException();
    }

    public static List<Tuple<byte, float>> GetDiscardChances(TarotGameState state)
    {
        throw new NotImplementedException();
    }

    public static List<Tuple<byte, float>> GetDeclareChances(TarotGameState state)
    {
        throw new NotImplementedException();
    }
}

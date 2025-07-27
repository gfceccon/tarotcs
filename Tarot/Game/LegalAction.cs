using System;

namespace Tarot.Game;

public class LegalAction
{
    public static BidAction[] GetLegalBidActions(TarotGameState state)
    {
        BidAction[] currentBids = state.Bids;
        var legalBids = new List<BidAction>
        {
            Bid.Passe,
            Bid.Petit,
            Bid.Garde,
            Bid.GardeSans,
            Bid.GardeContre,
        };
        var maxBid = currentBids.Max();

        return [.. legalBids.Where(bid => bid == Bid.Passe || bid.Value > maxBid.Value)];
    }

    public static CardAction[] GetLegalPlayActions(TarotGameState state)
    {
        // TODO Implement logic to determine legal play actions
        throw new NotImplementedException();
    }
    public static CardAction[] GetLegalDiscardActions(TarotGameState state)
    {
        // TODO Implement logic to determine legal discard actions
        throw new NotImplementedException();
    }
    public static DeclarationAction[] GetLegalDeclarationActions(TarotGameState state)
    {
        // TODO Implement logic to determine legal declaration actions
        throw new NotImplementedException();
    }

    public static BidAction[] GetBidChances(TarotGameState state)
    {
        // TODO Implement logic to determine bid chances
        throw new NotImplementedException();
    }

    public static CardAction[] GetDiscardChances(TarotGameState state)
    {
        // TODO Implement logic to determine discard chances
        throw new NotImplementedException();
    }

    public static DeclarationAction[] GetDeclarationChances(TarotGameState state)
    {
        // TODO Implement logic to determine declaration chances
        throw new NotImplementedException();
    }
}

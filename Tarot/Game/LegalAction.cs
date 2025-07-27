using System;

namespace Tarot.Game;

public class LegalAction
{
    public static BidAction[] GetLegalBidActions(TarotGameState state)
    {
        var legalBids = new List<BidAction>
        {
            Bid.Passe,
            Bid.Petit,
            Bid.Garde,
            Bid.GardeSans,
            Bid.GardeContre,
        };
        var maxBid = state.Bids.MaxBy(bid => bid.Value);

        return legalBids.Where(bid => bid == Bid.Passe || bid.Value > maxBid.Value).ToArray();
    }

    public static CardAction[] GetLegalPlayActions(TarotGameState state)
    {
        if (state.Phase != Phase.Play)
            throw new InvalidOperationException("Cannot play cards outside of the playing phase.");

        var playerHand = state.PlayerHands
            .Skip(state.Taker.Index * Constants.HandSize)
            .Take(Constants.HandSize);
        if (state.TrickCounter == 0)
            return playerHand.ToArray();

        var lead = state.CurrentTrick.First();
        var leadSuit = Card.Suit(lead);
        var followTrump = state.CurrentTrick.Any(Card.IsTrump);
        var bestCard = followTrump ? state.CurrentTrick.Where(Card.IsTrump).MaxBy(Card.Rank) :
            state.CurrentTrick.Where(card => Card.Suit(card) == leadSuit).MaxBy(Card.Rank);
        var legalSuits = playerHand
            .Where(card => (Card.Suit(card) == leadSuit && !followTrump) || Card.IsTrump(card))
            .Where(card => Card.Rank(card) > Card.Rank(bestCard));
        if (legalSuits.Any())
            return legalSuits.ToArray();
        return playerHand.ToArray();
    }
    public static CardAction[] GetLegalDiscardActions(TarotGameState state)
    {
        if (state.Phase != Phase.Chien)
            throw new InvalidOperationException("Discard actions can only be made during the chien phase.");
        var startIndex = state.Taker.Index * Constants.HandSize;
        var playerHand = state.PlayerHands
            .Skip(startIndex)
            .Take(Constants.HandSize)
            .Concat(state.Chien)
            .Where(card => !state.Discard.Contains(card));
        return playerHand.ToArray();
    }
    public static DeclarationAction[] GetLegalDeclarationActions(TarotGameState state)
    {
        if (state.Phase != Phase.ChelemDeclaration && state.Phase != Phase.PoigneeDeclaration)
            throw new InvalidOperationException("Declarations can only be made during the Chelem or Poignee declaration phases.");
        if (state.Phase == Phase.ChelemDeclaration)
            return [Declaration.Chelem, Declaration.None];
        if (state.Phase == Phase.PoigneeDeclaration)
        {
            var playerCards = state.PlayerHands
                .Skip(state.Current.Index * Constants.HandSize)
                .Take(Constants.HandSize);
            var trumps = playerCards.Count(Card.IsTrump);
            if (trumps >= Declaration.PoigneeMinTrumps[Declaration.TriplePoignee])
                return [Declaration.SinglePoignee, Declaration.DoublePoignee, Declaration.TriplePoignee, Declaration.None];
            if (trumps >= Declaration.PoigneeMinTrumps[Declaration.DoublePoignee])
                return [Declaration.SinglePoignee, Declaration.DoublePoignee, Declaration.None];
            if (trumps >= Declaration.PoigneeMinTrumps[Declaration.SinglePoignee])
                return [Declaration.SinglePoignee, Declaration.None];
            return [Declaration.None];
        }
        return [Declaration.None];
    }
}

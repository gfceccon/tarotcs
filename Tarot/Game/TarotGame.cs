namespace Tarot.Game;

public struct TarotGameState
{
    public TarotGameState(byte current, byte taker, byte[] currentTrick, byte[] publicCards, byte[] playedCards,
        byte[] playedTricks, byte[][] playerHands, byte[] chien, byte[] discards, byte[] bids, byte[] declarations)
    {
        if (currentTrick.Length != Constants.Players || publicCards.Length != Constants.DeckSize ||
            playedCards.Length != Constants.DeckSize ||
            playedTricks.Length != Constants.TricksSize * (1 + Constants.Players) ||
            playerHands.Length != Constants.HandSize || chien.Length != Constants.ChienSize ||
            discards.Length != Constants.ChienSize || bids.Length != Constants.BidSize ||
            declarations.Length != Constants.DeclarationSize)
            throw new Exception("Invalid game state initialization.");
        Taker = taker;
        Current = current;
        CurrentTrick = currentTrick;
        PublicCards = publicCards;
        for (var i = 0; i < playerHands.Length && i < Constants.Players; i++)
            if (playerHands[i].Length != Constants.HandSize)
                throw new Exception("Invalid player hand size.");
        if (playerHands[^1].Length != Constants.ChienSize) throw new Exception("Invalid chien size.");
        PlayedCards = playedCards;
        PlayedTricks = playedTricks;
        PlayerHands = playerHands;
        Chien = chien;
        Discards = discards;
        Bids = bids;
        Declarations = declarations;
    }

    public byte Taker = 0;
    public byte Current = 0;
    public byte[] CurrentTrick;
    public byte[] PublicCards;
    public byte[] PlayedCards;
    public byte[] PlayedTricks;
    public byte[][] PlayerHands;
    public byte[] Chien;
    public byte[] Discards;
    public byte[] Bids;
    public byte[] Declarations;
}

public class TarotGame
{
    public TarotGameState State;
    public Phase Phase = Phase.Bidding;
    public bool FoolPaid = false;
    public int FoolTrickIndex;
    public int FoolPlayer;

    public TarotGame()
    {
        var hands = Card.DealCards();
        State = new TarotGameState(
            current: 0,
            taker: 0,
            currentTrick: new byte[Constants.Players],
            publicCards: new byte[Constants.DeckSize],
            playedCards: new byte[Constants.DeckSize],
            playedTricks: new byte[Constants.TricksSize * (1 + Constants.Players)],
            playerHands: hands.ToArray(),
            chien: hands[^1],
            discards: new byte[Constants.ChienSize],
            bids: new byte[Constants.BidSize],
            declarations: new byte[Constants.DeclarationSize]
        );
    }

    public byte[] GetLegalActions()
    {
        return Phase switch
        {
            Phase.Bidding => Action.GetLegalBidActions(State.Bids),
            Phase.Chien => Action.GetLegalDiscardActions(),
            Phase.Declaration => Action.GetLegalDeclareActions(),
            Phase.Playing => Action.GetLegalPlayActions(),
            _ => []
        };
    }

    public List<Tuple<byte, float>> GetChanceActions()
    {
        return Phase switch
        {
            Phase.Bidding => Action.GetBidChances(),
            Phase.Chien => Action.GetDiscardChances(),
            Phase.Declaration => Action.GetDeclareChances(),
            _ => []
        };
    }

    public void ApplyAction(byte action)
    {
        throw new NotImplementedException();
    }

    public List<float> Results()
    {
        throw new NotImplementedException();
    }
    
    public bool IsTerminal()
    {
        return Phase is Phase.End;
    }
    
    public bool IsChance()
    {
        return Phase is Phase.Bidding or Phase.Chien or Phase.Declaration;
    }

    public TarotGame Clone()
    {
        return TarotGame.FromState(this.State);
    }

    public static TarotGame FromState(TarotGameState gameState)
    {
        return new TarotGame
        {
            State = new TarotGameState
            {
                Current = gameState.Current,
                Taker = gameState.Taker,
                CurrentTrick = (byte[])gameState.CurrentTrick.Clone(),
                PublicCards = (byte[])gameState.PublicCards.Clone(),
                PlayedCards = (byte[])gameState.PlayedCards.Clone(),
                PlayedTricks = (byte[])gameState.PlayedTricks.Clone(),
                PlayerHands = (byte[][])gameState.PlayerHands.Clone(),
                Chien = (byte[])gameState.Chien.Clone(),
                Discards = (byte[])gameState.Discards.Clone(),
                Bids = (byte[])gameState.Bids.Clone(),
                Declarations = (byte[])gameState.Declarations.Clone()
            }
        };
    }
}
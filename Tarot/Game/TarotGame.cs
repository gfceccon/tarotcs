namespace Tarot.Game;

/// <summary>
/// Represents the complete state of a Tarot game, including player hands, bids, tricks, and other game elements.
/// </summary>
public struct TarotGameState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TarotGameState"/> struct with the provided game state arrays.
    /// Throws an exception if any array does not match the expected size.
    /// </summary>
    /// <param name="current">The index of the current player.</param>
    /// <param name="taker">The index of the taker (player who won the bid).</param>
    /// <param name="currentTrick">The current trick being played.</param>
    /// <param name="publicCards">The public cards visible to all players.</param>
    /// <param name="playedCards">All cards that have been played so far.</param>
    /// <param name="playedTricks">All tricks that have been played.</param>
    /// <param name="playerHands">The hands of each player.</param>
    /// <param name="chien">The chien (dog) cards set aside during the deal.</param>
    /// <param name="discards">Cards discarded by the taker.</param>
    /// <param name="bids">Bids made by each player.</param>
    /// <param name="declarations">Declarations made by each player.</param>
    public TarotGameState(byte current, byte taker, byte[] currentTrick, byte[] publicCards, byte[] playedCards,
        byte[] playedTricks, byte[][] playerHands, byte[] chien, byte[] discards, byte[] bids, byte takerBid, byte chelem, byte[] declarations)
    {
        // Validate the sizes of the arrays to ensure they match expected constants.
        if (currentTrick.Length != Constants.Players || publicCards.Length != Constants.DeckSize ||
            playedCards.Length != Constants.DeckSize ||
            playedTricks.Length != Constants.TricksSize * (1 + Constants.Players) ||
            playerHands.Length != Constants.HandSize || chien.Length != Constants.ChienSize ||
            discards.Length != Constants.ChienSize || bids.Length != Constants.BidSize ||
            declarations.Length != Constants.DeclarationSize)
            throw new Exception("Invalid game state initialization.");

        // Validate player hands to ensure each player's hand has the correct size.
        for (var i = 0; i < playerHands.Length && i < Constants.Players; i++)
            if (playerHands[i].Length != Constants.HandSize)
                throw new Exception("Invalid player hand size.");
        if (playerHands[^1].Length != Constants.ChienSize) throw new Exception("Invalid chien size.");

        // Assign the arrays to the game state properties.
        Bids = bids;
        Taker = taker;
        TakerBid = takerBid;
        Current = current;
        Declarations = declarations;
        ChelemDeclared = chelem > 0;
        CurrentTrick = currentTrick;
        PublicCards = publicCards;
        PlayedCards = playedCards;
        PlayedTricks = playedTricks;
        PlayerHands = playerHands;
        Chien = chien;
        Discards = discards;
        // Initialize counters and flags.
        BidCounter = 0;
        DeclarationCounter = 0;
        DiscardCounter = 0;
        TrickCounter = 0;
        FoolPaid = false;
        FoolPlayer = 0;
        FoolTrickIndex = -1;
    }

    /// <summary>Index of the taker (player who won the bid).</summary>
    public byte Taker = 0;
    /// <summary>Index of the current player.</summary>
    public byte Current = 0;
    /// <summary>The current trick being played.</summary>
    public byte[] CurrentTrick;
    /// <summary>The public cards visible to all players.</summary>
    public byte[] PublicCards;
    /// <summary>All cards that have been played so far.</summary>
    public byte[] PlayedCards;
    /// <summary>All tricks that have been played.</summary>
    public byte[] PlayedTricks;
    /// <summary>The hands of each player.</summary>
    public byte[][] PlayerHands;
    /// <summary>The chien (dog) cards set aside during the deal.</summary>
    public byte[] Chien;
    /// <summary>Cards discarded by the taker.</summary>
    public byte[] Discards;
    /// <summary>Bids made by each player.</summary>
    public byte[] Bids;
    /// <summary>Taker's current bid.</summary>
    public byte TakerBid;
    /// <summary>Poignee declarations made by each player.</summary>
    public byte[] Declarations;
    /// <summary> Indicates whether a chelem has been declared. </summary>
    public bool ChelemDeclared;
    /// <summary> Counter for the number of bids made by players. </summary>
    public int BidCounter = 0;
    /// <summary> Counter for the number of declarations made by players. </summary>
    public int DeclarationCounter = 0;
    /// <summary> Counter for the number of cards discarded to the chien pile. </summary>
    public int DiscardCounter = 0;
    /// <summary> Counter for the number of tricks played. </summary>
    public int TrickCounter = 0;
    /// <summary> Indicates whether the Fool card has been paid. </summary>
    public bool FoolPaid = false;
    /// <summary> The index of the player who played the Fool. </summary>
    public byte FoolPlayer;
    /// <summary> The index of the trick in which the Fool was played. </summary>
    public int FoolTrickIndex;
    /// <summary>
    /// The current phase of the game (e.g., Bidding, Chien, Declaration, Playing, End).
    /// </summary>
    public Phase Phase = Phase.Bidding;
}

/// <summary>
/// Main class for managing the Tarot game logic, state transitions, and player actions.
/// </summary>
public class TarotGame
{
    /// <summary>
    /// The current state of the game, including all cards, bids, and player hands.
    /// </summary>
    public TarotGameState State;

    /// <summary>
    /// Initializes a new TarotGame with a fresh deal and default state.
    /// </summary>
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
            playerHands: [.. hands],
            chien: hands[^1],
            discards: new byte[Constants.ChienSize],
            bids: new byte[Constants.BidSize],
            takerBid: 0,
            chelem: 0,
            declarations: new byte[Constants.DeclarationSize]
        );
    }

    /// <summary>
    /// Returns the legal actions available to the current player, depending on the game phase.
    /// </summary>
    /// <returns>Array of legal action codes.</returns>
    public byte[] GetLegalActions()
    {
        return State.Phase switch
        {
            Phase.Bidding => LegalAction.GetLegalBidActions(State),
            Phase.Chien => LegalAction.GetLegalDiscardActions(State),
            Phase.Declaration => LegalAction.GetLegalDeclareActions(State),
            Phase.Playing => LegalAction.GetLegalPlayActions(State),
            _ => []
        };
    }

    /// <summary>
    /// Returns the possible chance actions and their probabilities for the current phase.
    /// </summary>
    /// <returns>List of tuples containing action code and probability.</returns>
    public List<Tuple<byte, float>> GetChanceActions()
    {
        return State.Phase switch
        {
            Phase.Bidding => LegalAction.GetBidChances(State),
            Phase.Chien => LegalAction.GetDiscardChances(State),
            Phase.Declaration => LegalAction.GetDeclareChances(State),
            _ => []
        };
    }

    /// <summary>
    /// Applies the given action to the game state, updating the game accordingly.
    /// Then updates the current player to the next player based on the action taken.
    /// </summary>
    /// <param name="action">The action code to apply.</param>
    public void ApplyAction(byte action)
    {
        var player = State.Phase switch
        {
            Phase.Bidding => Action.ApplyBid(State, action),
            Phase.Chien => Action.ApplyChien(State, action),
            Phase.Declaration => Action.ApplyDeclaration(State, action),
            Phase.Playing => Action.ApplyCard(State, action),
            _ => State.Current
        };
        State.Current = player;
    }

    /// <summary>
    /// Returns the final results (scores) for each player at the end of the game.
    /// </summary>
    /// <returns>List of scores for each player.</returns>
    public float[] Results()
    {
        float[] scores = new float[Constants.Players];
        float takerScore = Score.TotalScore(this);
        float defendersScore = -takerScore / (Constants.Players - 1);
        for (int i = 0; i < Constants.Players; i++)
            scores[i] = i == State.Taker ? takerScore : defendersScore;


        return scores;
    }

    /// <summary>
    /// Determines whether the game has reached a terminal (end) state.
    /// </summary>
    /// <returns>True if the game is over; otherwise, false.</returns>
    public bool IsTerminal()
    {
        return State.Phase is Phase.End;
    }

    /// <summary>
    /// Determines whether the current phase is a chance node (random event).
    /// </summary>
    /// <returns>True if the phase is a chance node; otherwise, false.</returns>
    public bool IsChance()
    {
        return State.Phase is Phase.Bidding or Phase.Chien or Phase.Declaration;
    }

    /// <summary>
    /// Creates a deep copy of the current game, including its state.
    /// </summary>
    /// <returns>A new TarotGame instance with the same state.</returns>
    public TarotGame Clone()
    {
        return FromState(State);
    }

    /// <summary>
    /// Creates a new TarotGame from a given game state.
    /// </summary>
    /// <param name="gameState">The game state to copy.</param>
    /// <returns>A new TarotGame instance initialized with the provided state.</returns>
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
                TakerBid = gameState.TakerBid,
                Declarations = (byte[])gameState.Declarations.Clone(),
                ChelemDeclared = gameState.ChelemDeclared,
                BidCounter = gameState.BidCounter,
                DeclarationCounter = gameState.DeclarationCounter,
                DiscardCounter = gameState.DiscardCounter,
                TrickCounter = gameState.TrickCounter,
                FoolPaid = gameState.FoolPaid,
                FoolPlayer = gameState.FoolPlayer,
                FoolTrickIndex = gameState.FoolTrickIndex,
                Phase = gameState.Phase
            }
        };
    }
}
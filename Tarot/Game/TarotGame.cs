namespace Tarot.Game;

public enum ActionKind: byte { Bid, Card, Declaration }
public readonly struct GenericAction(byte value, ActionKind kind)
{
    public byte Value { get; } = value;
    public ActionKind Kind { get; } = kind;

    public static implicit operator byte(GenericAction result)
        => result.Value;
    public static implicit operator BidAction(GenericAction result)
        => new(result.Value);
    public static implicit operator CardAction(GenericAction result)
        => new(result.Value);
    public static implicit operator DeclarationAction(GenericAction result)
        => new(result.Value);

    public static ActionKind FromPhase(Phase phase)
    {
        return phase switch
        {
            Phase.Bidding => ActionKind.Bid,
            Phase.Chien => ActionKind.Card,
            Phase.PoigneeDeclaration => ActionKind.Declaration,
            Phase.ChelemDeclaration => ActionKind.Declaration,
            Phase.Playing => ActionKind.Card,
            _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
        };
    }
}

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
    public TarotGameState(Player current, Player taker, CardAction[] currentTrick, CardAction[] publicCards, CardAction[] playedCards, Player[] winners,
        CardAction[] playedTricks, List<CardAction[]> playerHands, CardAction[] chien, CardAction[] discards,
        BidAction[] bids, BidAction takerBid, bool isChelemDeclared, DeclarationAction[] declarations)
    {
        // Validate the sizes of the arrays to ensure they match expected constants.
        if (currentTrick.Length != Constants.Players || publicCards.Length != Constants.DeckSize ||
            playedCards.Length != Constants.DeckSize || winners.Length != Constants.TricksSize ||
            playedTricks.Length != Constants.TricksSize * Constants.Players ||
            playerHands.Count != Constants.Players || chien.Length != Constants.ChienSize ||
            discards.Length != Constants.ChienSize || bids.Length != Constants.BidSize ||
            declarations.Length != Constants.DeclarationSize)
            throw new Exception("Invalid game state initialization.");

        // Validate player hands to ensure each player's hand has the correct size.
        for (var i = 0; i < playerHands.Count && i < Constants.Players; i++)
            if (playerHands[i].Length != Constants.HandSize)
                throw new Exception("Invalid player hand size.");

        // Conversão dos parâmetros byte[] para structs
        Bids = new BidAction[bids.Length];
        for (int i = 0; i < bids.Length; i++)
            Bids[i] = new BidAction(bids[i]);

        Taker = taker;
        TakerBid = new BidAction(takerBid);
        Current = current;

        Declarations = new DeclarationAction[declarations.Length];
        for (int i = 0; i < declarations.Length; i++)
            Declarations[i] = declarations[i];

        ChelemDeclared = isChelemDeclared;

        CurrentTrick = new CardAction[currentTrick.Length];
        for (int i = 0; i < currentTrick.Length; i++)
            CurrentTrick[i] = currentTrick[i];

        PublicCards = new CardAction[publicCards.Length];
        for (int i = 0; i < publicCards.Length; i++)
            PublicCards[i] = publicCards[i];

        PlayedCards = new CardAction[playedCards.Length];
        for (int i = 0; i < playedCards.Length; i++)
            PlayedCards[i] = playedCards[i];

        Winners = new Player[winners.Length];
        for (int i = 0; i < winners.Length; i++)
            Winners[i] = winners[i];

        PlayedTricks = new CardAction[playedTricks.Length];
        for (int i = 0; i < playedTricks.Length; i++)
            PlayedTricks[i] = playedTricks[i];

        PlayerHands = new();
        for (int i = 0; i < playerHands.Count; i++)
        {
            PlayerHands.Add(new CardAction[playerHands[i].Length]);
            for (int j = 0; j < playerHands[i].Length; j++)
                PlayerHands[i][j] = playerHands[i][j];
        }

        Chien = new CardAction[chien.Length];
        for (int i = 0; i < chien.Length; i++)
            Chien[i] = chien[i];

        Discards = new CardAction[discards.Length];
        for (int i = 0; i < discards.Length; i++)
            Discards[i] = discards[i];

        // Initialize counters and flags.
        BidCounter = 0;
        DeclarationCounter = 0;
        DiscardCounter = 0;
        TrickCounter = 0;
        TricksCounter = 0;
        FoolPaid = false;
        FoolPlayer = 0;
        FoolTrickIndex = -1;
    }

    /// <summary>Index of the taker (player who won the bid).</summary>
    public Player Taker = new(0);
    /// <summary>Index of the current player.</summary>
    public Player Current = new(0);
    /// <summary>The current trick being played.</summary>
    public CardAction[] CurrentTrick;
    /// <summary>The public cards visible to all players.</summary>
    public CardAction[] PublicCards;
    /// <summary>All cards that have been played so far.</summary>
    public CardAction[] PlayedCards;
    /// <summary>All tricks that have been played.</summary>
    public CardAction[] PlayedTricks;
    /// <summary>Array of winners for each trick played.</summary>
    public Player[] Winners;
    /// <summary>The hands of each player.</summary>
    public List<CardAction[]> PlayerHands;
    /// <summary>The chien (dog) cards set aside during the deal.</summary>
    public CardAction[] Chien;
    /// <summary>Cards discarded by the taker.</summary>
    public CardAction[] Discards;
    /// <summary>Bids made by each player.</summary>
    public BidAction[] Bids;
    /// <summary>Taker's current bid.</summary>
    public BidAction TakerBid;
    /// <summary>Poignee declarations made by each player.</summary>
    public DeclarationAction[] Declarations;
    /// <summary> Indicates whether a chelem has been declared. </summary>
    public bool ChelemDeclared;
    /// <summary> Counter for the number of bids made by players. </summary>
    public int BidCounter = 0;
    /// <summary> Counter for the number of declarations made by players. </summary>
    public int DeclarationCounter = 0;
    /// <summary> Counter for the number of cards discarded to the chien pile. </summary>
    public int DiscardCounter = 0;
    /// <summary> Counter for the current trick index being played. </summary>
    public int TrickCounter = 0;
    /// <summary> Counter for the number of tricks played. </summary>
    public int TricksCounter = 0;
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
        var (hands, chien) = Card.DealCards();
        State = new TarotGameState(
            current: new Player(0),
            taker: new Player(0),
            currentTrick: new CardAction[Constants.TrickSize],
            publicCards: new CardAction[Constants.DeckSize],
            playedCards: new CardAction[Constants.DeckSize],
            winners: new Player[Constants.TricksSize],
            playedTricks: new CardAction[Constants.TricksSize * Constants.Players],
            playerHands: hands,
            chien: chien,
            discards: new CardAction[Constants.ChienSize],
            bids: new BidAction[Constants.BidSize],
            takerBid: new BidAction(0),
            declarations: new DeclarationAction[Constants.DeclarationSize],
            isChelemDeclared: false
        );
    }

    protected static GenericAction[] ToActionResults<T>(T[] actions, ActionKind kind) where T : struct
    {
        var results = new GenericAction[actions.Length];
        for (int i = 0; i < actions.Length; i++)
            results[i] = new GenericAction(Convert.ToByte(actions[i]), kind);
        return results;
    }

    /// <summary>
    /// Returns the legal actions available to the current player, depending on the game phase.
    /// </summary>
    /// <returns>Array of legal action codes.</returns>
    public GenericAction[] GetLegalActions()
    {
        var type = GenericAction.FromPhase(State.Phase);
        switch (State.Phase)
        {
            case Phase.Bidding:
                var legalBids = LegalAction.GetLegalBidActions(State);
                return ToActionResults(legalBids, type);
            case Phase.Chien:
                var legalDiscards = LegalAction.GetLegalDiscardActions(State);
                return ToActionResults(legalDiscards, type);
            case Phase.PoigneeDeclaration:
            case Phase.ChelemDeclaration:
                var legalDeclarations = LegalAction.GetLegalDeclarationActions(State);
                return ToActionResults(legalDeclarations, type);
            case Phase.Playing:
                var legalPlays = LegalAction.GetLegalPlayActions(State);
                return ToActionResults(legalPlays, type);
            default:
                throw new InvalidOperationException("Invalid game phase for legal actions.");
        }
    }

    /// <summary>
    /// Returns the possible chance actions and their probabilities for the current phase.
    /// </summary>
    /// <returns>List of tuples containing action code and probability.</returns>
    public ChanceAction[] GetChanceActions()
    {
        return State.Phase switch
        {
            Phase.Bidding => Chance.GetBidChances(State),
            Phase.Chien => Chance.GetDiscardChances(State),
            Phase.PoigneeDeclaration => Chance.GetDeclarationChances(State),
            Phase.ChelemDeclaration => Chance.GetDeclarationChances(State),
            _ => throw new InvalidOperationException("Invalid game phase for chance actions.")
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
            Phase.Bidding => Action.ApplyBid(State, new BidAction(action)),
            Phase.Chien => Action.ApplyChien(State, new CardAction(action)),
            Phase.ChelemDeclaration => Action.ApplyDeclaration(State, new DeclarationAction(action)),
            Phase.PoigneeDeclaration => Action.ApplyDeclaration(State, new DeclarationAction(action)),
            Phase.Playing => Action.ApplyCard(State, new CardAction(action)),
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
            scores[i] = i == State.Taker.Value ? takerScore : defendersScore;

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
        return State.Phase is Phase.Bidding or Phase.Chien or Phase.PoigneeDeclaration;
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
                CurrentTrick = gameState.CurrentTrick,
                PublicCards = gameState.PublicCards,
                PlayedCards = gameState.PlayedCards,
                PlayedTricks = gameState.PlayedTricks,
                PlayerHands = gameState.PlayerHands,
                Chien = gameState.Chien,
                Discards = gameState.Discards,
                Bids = gameState.Bids,
                TakerBid = gameState.TakerBid,
                Declarations = gameState.Declarations,
                ChelemDeclared = gameState.ChelemDeclared,
                BidCounter = gameState.BidCounter,
                DeclarationCounter = gameState.DeclarationCounter,
                DiscardCounter = gameState.DiscardCounter,
                TrickCounter = gameState.TrickCounter,
                TricksCounter = gameState.TricksCounter,
                FoolPaid = gameState.FoolPaid,
                FoolPlayer = gameState.FoolPlayer,
                FoolTrickIndex = gameState.FoolTrickIndex,
                Phase = gameState.Phase
            }
        };
    }
}
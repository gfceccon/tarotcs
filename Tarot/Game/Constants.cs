namespace Tarot.Game;

/// <summary>
/// This class contains the game constants for the Tarot game.
/// It defines the number of players, card indices for special cards, deck size, hand size
/// and other game-related constants.
/// It also includes constants for the maximum number of actions, bids, and declarations.
/// It also defines constants for MCTS parameters used in the simulations.
/// </summary>
public static class Constants
{
    // Game configuration constants
    public const int Players = 4;
    public const byte ChienId = 5;
    public const int SuitSize = 14;
    public const int RankSize = 4;
    public const int Fool = 57;
    public const int Petit = 58;
    public const int Monde = 78;

    // Card constants
    /// Suit index (0: Spades, 1: Hearts, 2: Clubs, 3: Diamonds, 4: Trump)
    public const byte EndSpades = 14; // 1-14 for Spades
    public const byte EndHearts = 28; // 14-28 for Hearts
    public const byte EndClubs = 42; // 28-42 for Clubs
    public const byte EndDiamonds = 56; // 42-56 for Diamonds
    public const byte EndSuits = 56; // 1-56 for Suits
    public const byte EndTrumps = 78; // 56-78 for Trumps
    public const byte Spades = 0;
    public const byte Hearts = 1;
    public const byte Clubs = 2;
    public const byte Diamonds = 3;
    public const byte Trumps = 4;
    public const byte Jack = 11;
    public const byte Queen = 12;
    public const byte Knight = 13;
    public const byte King = 14;
    public const byte CardsPerSuit = 14;

    // Game constants
    public const int DeckSize = 78;
    public const int HandSize = 18;
    public const int TrickSize = 4;
    public const int TricksSize = 19;
    public const int BidSize = 4;
    public const int ChienSize = 6;
    public const int DeclarationSize = 4;

    // Number of actions in the game
    public const int MaxCards = DeckSize; // 78 cards in the Tarot deck
    public const int MaxBidding = 5; // 0: Pass, 1: Petit, 2: Garde, 3: Garde Sans, 4: Garde Contre
    public const int MaxDeclarations = 5; // 0: None, 1: Chelem, 2: Single Poignee, 3: Double Poignee, 4: Triple Poignee
    public const int MaxActions = MaxCards + MaxBidding + MaxDeclarations;

    // Action codes
    public const byte StartCard = 1; // 1 - 78 for cards
    public const byte EndCard = StartCard + MaxCards - 1;
    public const byte StartBid = EndCard + 1; // 79 - 83 for bids
    public const byte EndBid = StartBid + MaxBidding - 1;
    public const byte StartDeclaration = EndBid + 1; // 84 - 86 for declarations
    public const byte EndDeclaration = StartDeclaration + MaxDeclarations - 1;

    // MCTS parameters
    public const float ExplorationConstant = 1.41f;
    public const float ProgWideningConstant = 2f;
    public const float ProgWideningAlpha = 0.5f;
}
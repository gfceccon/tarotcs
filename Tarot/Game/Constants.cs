namespace Tarot.Game;

public static class Constants
{
    public const int Players = 4;
    public const byte ChienId = 5;
    
    public const int Fool = 14 * 4;
    public const int Petit = Fool + 1;
    public const int Monde = Fool + 21;
    
    // Game constants
    public const int DeckSize = 78;
    public const int HandSize = 18;
    public const int TrickSize = 4;
    public const int TricksSize = 19;
    public const int BidSize = 4;
    public const int ChienSize = 6;
    public const int DeclarationSize = 8;
    
    // Number of actions in the game
    public const int MaxCards = DeckSize;
    public const int MaxBidding = 5; // 0: Pass, 1: Petit, 2: Garde, 3: Garde Sans, 4: Garde Contre
    public const int MaxDeclarations = 3; // 0: None, 1: Chelem, 2: Poignee
    public const int MaxActions = MaxCards + MaxBidding + MaxDeclarations;
    
    // MCTS parameters
    public const float ExplorationConstant = 1.41f;
    public const float ProgWideningConstant = 2f;
    public const float ProgWideningAlpha = 0.5f;
}
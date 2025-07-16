
namespace Tarot.Game;

/// <summary>
/// This class contains the card actions for the Tarot game.
/// It provides methods to deal cards, determine suits and ranks, and calculate card values.
/// All cards are index based, from 0 to 77.
/// The suits are Spades, Hearts, Clubs, Diamonds.
/// The suits cards range from 0 to 55 (14 cards each for Spades, Hearts, Clubs, Diamonds).
/// The trumps are from 56 to 77, with special cards like:
/// - The Fool (56)
/// - The Petit (57)
/// - The Monde (77)
/// </summary>
public static class Card
{
    /// <summary>
    /// Shuffle the deck and deals cards to players and returns the hands and chien.
    /// </summary>
    /// <returns>List of byte array, first 4 are player hands, last array is the chien cards.</returns>
    public static List<byte[]> DealCards()
    {
        var cards = Enumerable.Range(0, Constants.DeckSize).Select(i => (byte)i).ToArray();
        Rng.Random.Shuffle(cards);
        var chien = cards.Take(Constants.ChienSize).ToArray();
        cards = [.. cards.Skip(Constants.ChienSize)];
        var hands = cards.Chunk(Constants.HandSize).ToList();
        hands.Add(chien);
        return hands;
    }
    /// <summary>
    /// Returns the suit of a card.
    /// </summary>
    /// <param name="card">The card value.</param>
    /// <returns>Suit index (0: Spades, 1: Hearts, 2: Clubs, 3: Diamonds, 4: Trump).</returns>
    public static byte Suit(byte card)
    {
        return card switch
        {
            < 14 => 0,
            < 28 => 1,
            < 42 => 2,
            < 56 => 3,
            _ => 4
        };
    }
    /// <summary>
    /// Returns the rank of a card.
    /// </summary>
    /// <param name="card">The card value.</param>
    /// <returns>Rank index (0-13 for suits, 0-21 for trumps).</returns>
    public static byte Rank(byte card)
    {
        if (card < 56)
            return (byte)(card % 14); // 0 to 13 for suits
        return (byte)(card - 56);     // 0 to 21 for trumps
    }

    private static readonly string[] SuitNames = ["Spades", "Hearts", "Clubs", "Diamonds"];
    private static readonly string[] RankNames = ["Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Knight", "Queen", "King"
    ];

    /// <summary>
    /// Returns the name of a card in English.
    /// </summary>
    /// <param name="card">The card value.</param>
    /// <returns>Card name as a string.</returns>
    public static string Name(byte card)
    {
        var suit = Suit(card);
        var rank = Rank(card);
        if (!IsTrump(card)) return $"{RankNames[rank]} of {SuitNames[suit]}";
        if (rank == Constants.Petit)
            return "The Little One";
        else if (rank == Constants.Monde)
            return "The World";
        else if (rank == Constants.Fool)
            return "The Fool";
        return $"Trump {rank}";
    }
    /// <summary>
    /// Returns the value of a card according to French Tarot rules
    /// 4.5 for Kings and Bout cards, 3.5 for Queens, 2.5 for Knights, 1.5 for Jacks, and 0.5 for others.
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>The value</returns>
    public static float Value(byte card)
    {
        var rank = Rank(card);
        if (IsFool(card) || IsBout(card)) return 4.5f;
        if (IsKing(card)) return 4.5f;
        return rank switch
        {
            12 => 3.5f, // Queen
            11 => 2.5f, // Knight
            10 => 1.5f, // Jack
            _ => 0.5f    // Others
        };
    }

    /// <summary>
    /// Checks if the card is a King.
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsKing(byte card)
    {
        return Suit(card) < 4 && Rank(card) == 13;
    }

    /// <summary>
    /// Checks if the card is a Trump
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsTrump(byte card)
    {
        return Suit(card) == 4;
    }

    /// <summary>
    /// Checks if the card is The Fool
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsFool(byte card)
    {
        return card == Constants.Fool;
    }

    /// <summary>
    /// Checks if the card is a Bout (The Fool, Petit, or Monde)
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsBout(byte card)
    {
        return IsFool(card) || (IsTrump(card) && (Rank(card) == Constants.Petit || Rank(card) == Constants.Monde));
    }

    // 

    /// <summary>
    /// Sums the points of a list of cards
    /// </summary>
    /// <param name="cards">The card value</param>
    /// <returns>True or False</returns>
    public static float Points(List<byte> cards)
    {
        return cards.Sum(Value);
    }
}
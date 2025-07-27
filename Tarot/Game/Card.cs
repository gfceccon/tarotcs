
namespace Tarot.Game;

public readonly struct CardAction
{
    public byte Value { get; }
    public CardAction(byte value)
    {
        var v = value + 1;
        if (v < Constants.StartCard || v > Constants.EndCard)
            throw new ArgumentOutOfRangeException(nameof(value), "Card action is out of range.");
        Value = (byte)v; // Adjusting to 1-based index
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return obj is CardAction card && Value == card.Value;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public static bool operator ==(CardAction left, CardAction right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CardAction left, CardAction right)
    {
        return !(left == right);
    }

    public static implicit operator byte(CardAction action) => action.Value;
    public static implicit operator CardAction(byte value) => new(value);
}


/// <summary>
/// This class contains the card actions for the Tarot game.
/// It provides methods to deal cards, determine suits and ranks, and calculate card values.
/// All cards are 1-based, from 1 to 78.
/// The suits are Spades, Hearts, Clubs, Diamonds.
/// The suits cards range from 1 to 56 (14 cards each for Spades, Hearts, Clubs, Diamonds).
/// The trumps are from 57 to 78, with special cards like:
/// - The Fool (57)
/// - The Petit (58)
/// - The Monde (78)
/// </summary>
public static class Card
{
    /// <summary>
    /// Shuffle the deck and deals cards to players and returns the hands and chien.
    /// </summary>
    /// <returns>Tuple: List of player hands, chien cards.</returns>
    public static (List<CardAction[]> hands, CardAction[] chien) DealCards()
    {
        var cards = Enumerable.Range(0, Constants.DeckSize).Select(i => new CardAction((byte)i)).ToArray();
        Rng.Random.Shuffle(cards);
        var chien = cards.Take(Constants.ChienSize).ToArray();
        cards = cards.Skip(Constants.ChienSize).ToArray();
        var hands = cards.Chunk(Constants.HandSize).ToList();
        return (hands, chien);
    }
    /// <summary>
    /// Returns the suit of a card.
    /// </summary>
    /// <param name="card">The card value.</param>
    /// <returns>Suit index (0: Spades, 1: Hearts, 2: Clubs, 3: Diamonds, 4: Trump).</returns>
    public static byte Suit(CardAction _card)
    {
        var card = _card.Value;
        return card switch
        {
            <= Constants.EndSpades => Constants.Spades,
            <= Constants.EndHearts => Constants.Hearts,
            <= Constants.EndClubs => Constants.Clubs,
            <= Constants.EndDiamonds => Constants.Diamonds,
            <= Constants.EndTrumps => Constants.Trumps,
            _ => throw new ArgumentOutOfRangeException(nameof(_card), "Card action is out of range.")
        };
    }
    /// <summary>
    /// Returns the rank of a card.
    /// </summary>
    /// <param name="card">The card value.</param>
    /// <returns>Rank index (1-14 for suits, 0-22 for trumps).</returns>
    public static byte Rank(CardAction _card)
    {
        var card = _card.Value - 1; // Adjusting to 0-based index
        if (card < Constants.EndSuits)
            return (byte)(card % Constants.CardsPerSuit + 1); // 1 to 14 for suits
        return (byte)(card - Constants.Fool + 1);     // 0 to 22 for trumps
    }

    private static readonly string[] SuitNames = ["Spades", "Hearts", "Clubs", "Diamonds"];
    private static readonly string[] RankNames = ["Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Knight", "Queen", "King"
    ];

    /// <summary>
    /// Returns the name of a card in English.
    /// </summary>
    /// <param name="card">The card value.</param>
    /// <returns>Card name as a string.</returns>
    public static string Name(CardAction _card)
    {
        var suit = Suit(_card);
        var rank = Rank(_card);
        if (!IsTrump(_card)) return $"{RankNames[rank - 1]} of {SuitNames[suit]}";
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
    public static float Value(CardAction _card)
    {
        var rank = Rank(_card);
        if (IsFool(_card) || IsBout(_card)) return 4.5f;
        return rank switch
        {
            Constants.King => 4.5f,
            Constants.Queen => 3.5f,
            Constants.Knight => 2.5f,
            Constants.Jack => 1.5f,
            _ => 0.5f
        };
    }

    /// <summary>
    /// Checks if the card is a King.
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsKing(CardAction _card)
    {
        return _card.Value <= Constants.EndSuits && Rank(_card) == Constants.King;
    }

    /// <summary>
    /// Checks if the card is a Trump
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsTrump(CardAction _card)
    {
        var card = _card.Value;
        return card > Constants.EndSuits && card <= Constants.EndTrumps;
    }

    /// <summary>
    /// Checks if the card is The Fool
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsFool(CardAction _card)
    {
        var card = _card.Value;
        return card == Constants.Fool;
    }

    /// <summary>
    /// Checks if the card is a Bout (The Fool, Petit, or Monde)
    /// </summary>
    /// <param name="card">The card value</param>
    /// <returns>True or False</returns>
    public static bool IsBout(CardAction _card)
    {
        return IsFool(_card) ||
        Rank(_card) == Constants.Petit ||
        Rank(_card) == Constants.Monde;
    }

    /// <summary>
    /// Sums the points of a list of cards
    /// </summary>
    /// <param name="cards">The card value</param>
    /// <returns>True or False</returns>
    public static float Points(List<CardAction> cards)
    {
        return cards.Sum(Value);
    }
}
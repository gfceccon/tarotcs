using System;

namespace Tarot.Game;

public readonly struct DeclarationAction(byte value) : IEquatable<DeclarationAction>
{
    public byte Value { get; } = value;
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return obj is DeclarationAction declaration && Value == declaration.Value;
    }

    public bool Equals(DeclarationAction other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public static bool operator ==(DeclarationAction left, DeclarationAction right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DeclarationAction left, DeclarationAction right)
    {
        return !(left == right);
    }
    public static implicit operator byte(DeclarationAction action) => action.Value;
    public static implicit operator DeclarationAction(byte value) => new(value);

}

/// <summary>
/// This class contains the declaration actions for the Tarot game.
/// The declaration actions are used to declare bonus.
/// </summary>
public class Declaration
{
    public static readonly DeclarationAction None = new(Constants.StartDeclaration);
    public static readonly DeclarationAction Chelem = new(Constants.StartDeclaration + 1);
    public static readonly DeclarationAction SinglePoignee = new(Constants.StartDeclaration + 2);
    public static readonly DeclarationAction DoublePoignee = new(Constants.StartDeclaration + 3);
    public static readonly DeclarationAction TriplePoignee = new(Constants.StartDeclaration + 4);

    public const int ChelemBonus = 200;
    public const int ChelemBonusDeclared = 400;

    public static readonly Dictionary<DeclarationAction, int> PoigneeBonus = new()
    {
        { SinglePoignee, 20 },
        { DoublePoignee, 30 },
        { TriplePoignee, 40 },
    };
    public static readonly Dictionary<DeclarationAction, int> PoigneeMinTrumps = new()
    {
        { SinglePoignee, 10 },
        { DoublePoignee, 13 },
        { TriplePoignee, 15 },
    };

    /// <summary>
    /// Returns the name of the declaration action.
    /// </summary>
    /// <param name="declaration">Declaration action</param>
    /// <returns>Name of the declaration action</returns>
    public static string Name(DeclarationAction declaration)
    {
        if (declaration == None) return "None";
        if (declaration == Chelem) return "Chelem";
        if (declaration == SinglePoignee) return "Single Poignee";
        if (declaration == DoublePoignee) return "Double Poignee";
        if (declaration == TriplePoignee) return "Triple Poignee";
        return "Unknown";
    }

    /// <summary>
    /// Counts the number of trumps played by the player in the game.
    /// </summary>
    /// <param name="state">Game state</param>
    /// <param name="player">Player</param>
    /// <returns>Number of trumps played</returns>
    public static int CountTrumps(TarotGameState state, Player player)
    {
        var count = 0;
        var isPlayerTaker = state.Taker == player;
        Player taker = state.Taker;
        for (int i = 0; i <= state.TrickCounter; i++)
        {
            var trickWinner = state.Winners[i];
            var winner = trickWinner == player;
            var defenderWinner = trickWinner != taker && !isPlayerTaker;
            if (winner || defenderWinner)
            {
                if (Card.IsTrump(state.PlayedCards[trickWinner.Value]))
                    count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Calculates the bonus for the declarations at the end of the game.
    /// </summary>
    /// <param name="state">Game state</param>
    /// <returns>Tuple containing the Chelem and Poignee bonuses</returns>
    public static Tuple<int, int> Bonus(TarotGameState state)
    {
        if (state.Phase != Phase.End)
            throw new InvalidOperationException("Bonus calculation can only be done at the end of the game.");

        int poignee = 0;
        for (int i = 0; i < Constants.Players; i++)
        {
            var trumps = CountTrumps(state, new Player((byte)i));
            var declaration = state.Declarations[i];
            if (trumps >= PoigneeMinTrumps[declaration])
                poignee += PoigneeBonus[declaration];
            else
                poignee -= PoigneeBonus[declaration];
        }


        int chelemCount = 0;
        for (int i = 0; i < Constants.TricksSize; i++)
        {
            var winner = state.PlayedTricks[i * Constants.TrickSize].Value;
            if (state.Taker.Value == winner)
                chelemCount++;
        }

        var chelem = -ChelemBonus;
        if (chelemCount >= Constants.TricksSize - 1)
            chelem = state.ChelemDeclared ? ChelemBonusDeclared : ChelemBonus;

        var multiplier = Bid.Multiplier(state.Bids[state.Taker.Value]);
        return Tuple.Create(chelem, poignee * multiplier);
    }
}

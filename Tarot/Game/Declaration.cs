using System;

namespace Tarot.Game;

using DeclarationAction = byte;

/// <summary>
/// This class contains the declaration actions for the Tarot game.
/// The declaration actions are used to declare bonus.
/// </summary>
public class Declaration
{
    public const DeclarationAction None = Constants.StartDeclaration;
    public const DeclarationAction Chelem = Constants.StartDeclaration + 1;
    public const DeclarationAction SinglePoignee = Constants.StartDeclaration + 2;
    public const DeclarationAction DoublePoignee = Constants.StartDeclaration + 3;
    public const DeclarationAction TriplePoignee = Constants.StartDeclaration + 4;

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

    public static string Name(DeclarationAction declaration)
    {
        return declaration switch
        {
            Chelem => "Chelem",
            SinglePoignee => "Single Poignee",
            DoublePoignee => "Double Poignee",
            TriplePoignee => "Triple Poignee",
            None => "None",
            _ => "Unknown"
        };
    }

    public static int CountTrumps(TarotGameState state, byte player)
    {
        int count = 0;
        bool isPlayerTaker = state.Taker == player;
        byte taker = state.Taker;
        for (int i = 0; i <= Constants.TricksSize; i++)
        {
            byte trickWinner = state.PlayedTricks[i * Constants.TrickSize];
            bool winner = trickWinner == player;
            bool defenderWinner = trickWinner != taker && !isPlayerTaker;
            if (winner || defenderWinner)
            {
                for (int j = 1; j < Constants.TrickSize && j < state.PlayedTricks.Length; j++)
                {
                    byte card = state.PlayedTricks[i + j];
                    if (Card.IsTrump(card))
                        count++;
                }
            }
        }
        return count;
    }

    public static Tuple<int, int> Bonus(TarotGameState state)
    {
        if (state.Phase != Phase.End)
            throw new InvalidOperationException("Bonus calculation can only be done at the end of the game.");

        int poignee = 0;
        for (int i = 0; i < Constants.Players; i++)
        {
            var trumps = CountTrumps(state, (byte)i);
            var declaration = state.Declarations[i];
            if (trumps >= PoigneeMinTrumps[declaration])
                poignee += PoigneeBonus[declaration];
            else
                poignee -= PoigneeBonus[declaration];
        }


        int chelemCount = 0;
        for (int i = 0; i < Constants.TricksSize; i++)
            if (state.PlayedTricks[i * Constants.TrickSize] == state.Taker)
                chelemCount++;

        var chelem = -ChelemBonus;
        if (chelemCount >= Constants.TricksSize - 1)
            chelem = state.ChelemDeclared ? ChelemBonusDeclared : ChelemBonus;

        var multiplier = Bid.Multiplier(state.Bids[state.Taker]);
        return Tuple.Create(chelem, poignee * multiplier);
    }
}

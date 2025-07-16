namespace Tarot.Game;

/// <summary>
/// This enum represents the different strategies that can be used in the Tarot game.
/// The strategies include:
/// - Min: A strategy that use the minimum value card.
/// - Max: A strategy that use the maximum value card.
/// - Random: A strategy that randomly selects a card.
/// - RisMcts: A strategy that uses Re-determinization Information Set Monte Carlo Tree Search (RIS-MCTS).
/// - RaveMcts: A strategy that uses Rapid Action Value Estimation MCTS.
/// </summary>
public enum Strategy
{
    Min,
    Max,
    Random,
    RisMcts,
    RaveMcts,
}
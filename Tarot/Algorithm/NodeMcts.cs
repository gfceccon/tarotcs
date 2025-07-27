namespace Tarot.Algorithm;

using Math = MathF;
using System;
using Tarot.Game;

public class NodeMcts
{
    public GenericAction Action = new();
    public NodeMcts? Parent;
    public Dictionary<GenericAction, NodeMcts> Children = new();
    public InformationSet InformationSet = new();
    public HashSet<GenericAction> ExpandedActions = new();
    public float Value = 0.0f;
    public int Visits = 0;

    /// <summary>
    /// Calculates the UCB1 value for this node.
    /// </summary>
    /// <param name="explorationConstant">UCB exploration constant (default 1.41)</param>
    /// <returns></returns>
    public float Ucb1(float explorationConstant = 1.41f)
    {
        var q = Value / Visits;
        var u = explorationConstant * Math.Sqrt(Math.Log(Parent!.Visits) / Visits);
        return q + u;
    }

    /// <summary>
    /// Selects a child node based on the UCB1 value.
    /// If no children are available, selects randomly.
    /// </summary>
    /// <param name="legalActions"></param>
    /// <returns></returns>
    public NodeMcts Select(GenericAction[] legalActions)
    {
        // Check if there are expanded actions within the legal actions
        var validActions = ExpandedActions.Intersect(legalActions);
        if (validActions.Any()) return Children.MaxBy(pair => pair.Value.Ucb1()).Value;

        // If no expanded actions, select a random action from the legal actions
        var randomIndex = Random.Shared.Next(legalActions.Length);
        var randomAction = legalActions[randomIndex];

        if (Children.TryGetValue(randomAction, out var select)) return select;

        var node = new NodeMcts { Action = randomAction, Parent = this };
        Children[randomAction] = node;
        ExpandedActions.Add(randomAction);
        return node;

    }

    /// <summary>
    /// Progressive Widening equation.
    /// </summary>
    /// <param name="constant">PW constant (default 2.0)</param>
    /// <param name="alpha">PW exponent (default 0.5)</param>
    /// <returns></returns>
    public bool ShouldExpand(float constant = 2.0f, float alpha = 0.5f)
    {
        if (Visits == 0) return true;
        return ExpandedActions.Count < constant * Math.Pow(Visits, alpha);
    }
}
namespace Tarot.Algorithm;

using Math = MathF;
using System;
public class NodeMcts
{
    public byte Action = 0;
    public NodeMcts? Parent;
    public Dictionary<byte, NodeMcts> Children = new();
    public InformationSet InformationSet = new();
    public HashSet<byte> ExpandedActions = [];
    public float Value = 0.0f;
    public int Visits = 0;

    public float Ucb1(float explorationConstant = 1.41f)
    {
        var q = Value / Visits;
        var u = explorationConstant * Math.Sqrt(Math.Log(Parent!.Visits) / Visits);
        return q + u;
    }

    public NodeMcts Select(byte[] legalActions)
    {
        var validActions = ExpandedActions.Intersect(legalActions);
        if (validActions.Any()) return Children.MaxBy(pair => pair.Value.Ucb1()).Value;

        var randomIndex = Random.Shared.Next(legalActions.Length);
        var randomAction = legalActions[randomIndex];

        if (Children.TryGetValue(randomAction, out var select)) return select;

        var node = new NodeMcts { Action = randomAction, Parent = this };
        Children[randomAction] = node;
        ExpandedActions.Add(randomAction);
        return node;

    }

    public bool ShouldExpand(float constant, float alpha)
    {
        if (Visits == 0) return true;
        return ExpandedActions.Count < constant * Math.Pow(Visits, alpha);
    }
}
namespace Tarot.Algorithm;

using Tarot.Game;
using Tarot.Metric;


public abstract class IsMcts(Player player, int iterations)
{

    public NodeMcts Root = new();
    public int Iterations = iterations;
    public float ExplorationConstant = Constants.ExplorationConstant;
    public float PwConstant = Constants.ProgWideningConstant;
    public float PwAlpha = Constants.ProgWideningAlpha;

    public Player Player = player;
    public long TotalNodesCreated;
    public long TotalSimulationsRun;
    public long TotalNodesExpanded;
    public long TotalDeterminizations;

    public long IterationsRun;
    public long NodesCreated;
    public long SimulationsRun;
    public long NodesExpanded;
    public long Determinizations;
    protected abstract TarotGameState Determinize(TarotGameState state, NodeMcts node);

    public virtual void Run(TarotGameState state)
    {
        var node = PreRun(state);
        for (int i = 0; i < Iterations; i++)
        {
            var _state = Determinize(state, node);
            node = Select(_state, node);
            Expand(_state, node);
            Simulate(_state, node);
            Backpropagate(node, _state);
        }
        PostRun(state, node);
    }
    protected virtual bool ShouldExpand(TarotGameState state, NodeMcts node)
    {
        var legalActions = GetLegalActions(state);
        if (legalActions.Any(action => !node.ExpandedActions.Contains(action)))
            return true;
        return false;
    }

    public virtual NodeMcts Select(TarotGameState state, NodeMcts node)
    {
        var legalActions = GetLegalActions(state);
        return node.Select(legalActions);
    }

    public virtual void Expand(TarotGameState state, NodeMcts node)
    {
        var legalActions = GetLegalActions(state);
        foreach (var action in legalActions)
        {
            if (!ShouldExpand(state, node))
                break;

            var childNode = new NodeMcts { Action = action, Parent = node };
            node.Children[action] = childNode;
            node.ExpandedActions.Add(action);
            NodesCreated++;
        }
        NodesExpanded++;
    }

    public virtual void Simulate(TarotGameState state, NodeMcts node)
    {
        TarotGameState clonedState = state.Clone();
        while (!TarotGame.IsTerminal(clonedState))
            SimulationStep(clonedState, node);
    }
    public virtual bool SimulationStep(TarotGameState state, NodeMcts node)
    {
        var legalActions = GetLegalActions(state);
        if (legalActions.Length == 0) return false;
        var action = legalActions[Random.Shared.Next(legalActions.Length)];
        TarotGame.ApplyAction(state, action);
        return true;
    }

    public virtual void Backpropagate(NodeMcts? node, TarotGameState state)
    {
        float reward = TarotGame.Results(state)[Player.Index];
        while (node != null)
        {
            node.Value += reward;
            node.Visits++;
            node = node.Parent;
        }
    }

    public virtual GenericAction[] GetLegalActions(TarotGameState state)
    {
        return TarotGame.GetLegalActions(state);
    }

    public virtual NodeMcts PreRun(TarotGameState state)
    {
        IterationsRun = 0;
        NodesCreated = 0;
        SimulationsRun = 0;
        NodesExpanded = 0;
        Determinizations = 0;

        return Root;
    }
    public virtual void PostRun(TarotGameState state, NodeMcts node)
    {
        TotalNodesCreated += NodesCreated;
        TotalSimulationsRun += SimulationsRun;
        TotalNodesExpanded += NodesExpanded;
        TotalDeterminizations += Determinizations;
        Metrics.Instance.RecordMetric("IterationsRun", state, IterationsRun);
        Metrics.Instance.RecordMetric("NodesCreated", state, TotalNodesCreated);
        Metrics.Instance.RecordMetric("SimulationsRun", state, TotalSimulationsRun);
        Metrics.Instance.RecordMetric("NodesExpanded", state, TotalNodesExpanded);
        Metrics.Instance.RecordMetric("Determinizations", state, TotalDeterminizations);
    }
}

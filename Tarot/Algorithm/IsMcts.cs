namespace Tarot.Algorithm;

using Tarot.Game;
using Tarot.Metric;


public abstract class IsMcts(int iterations, byte player)
{

    public NodeMcts Root = new();
    public int Iterations = iterations;
    public float ExplorationConstant = Constants.ExplorationConstant;
    public float PwConstant = Constants.ProgWideningConstant;
    public float PwAlpha = Constants.ProgWideningAlpha;

    public byte Player = player;
    public long TotalNodesCreated;
    public long TotalSimulationsRun;
    public long TotalNodesExpanded;
    public long TotalDeterminizations;

    public long IterationsRun;
    public long NodesCreated;
    public long SimulationsRun;
    public long NodesExpanded;
    public long Determinizations;
    protected abstract TarotGameState Determinize(TarotGame game, NodeMcts node);

    public virtual void Run(TarotGame game)
    {
        var node = PreRun(game);
        for (int i = 0; i < Iterations; i++)
        {
            var state = Determinize(game, node);
            var _game = TarotGame.FromState(state);
            node = Select(_game, node);
            Expand(_game, node);
            Simulate(_game, node);
            Backpropagate(node, _game);
        }
        PostRun(game, node);
    }
    protected virtual bool ShouldExpand(TarotGame game, NodeMcts node)
    {
        var legalActions = GetLegalActions(game);
        if (legalActions.Any(action => !node.ExpandedActions.Contains(action)))
            return true;
        return false;
    }

    public virtual NodeMcts Select(TarotGame game, NodeMcts node)
    {
        var legalActions = GetLegalActions(game);
        return node.Select(legalActions);
    }

    public virtual void Expand(TarotGame game, NodeMcts node)
    {
        var legalActions = GetLegalActions(game);
        foreach (var action in legalActions)
        {
            if (!ShouldExpand(game, node))
                break;

            var childNode = new NodeMcts { Action = action, Parent = node };
            node.Children[action] = childNode;
            node.ExpandedActions.Add(action);
            NodesCreated++;
        }
        NodesExpanded++;
    }

    public virtual void Simulate(TarotGame game, NodeMcts node)
    {
        TarotGame state = game.Clone();
        while (!state.IsTerminal())
            SimulationStep(state, node);
    }
    public virtual bool SimulationStep(TarotGame game, NodeMcts node)
    {
        var legalActions = GetLegalActions(game);
        if (legalActions.Length == 0) return false;
        var action = legalActions[Random.Shared.Next(legalActions.Length)];
        game.ApplyAction(action);
        return true;
    }

    public virtual void Backpropagate(NodeMcts? node, TarotGame game)
    {
        float reward = game.Results()[Player];
        while (node != null)
        {
            node.Value += reward;
            node.Visits++;
            node = node.Parent;
        }
    }

    public virtual byte[] GetLegalActions(TarotGame game)
    {
        return game.GetLegalActions();
    }

    public virtual NodeMcts PreRun(TarotGame game)
    {
        IterationsRun = 0;
        NodesCreated = 0;
        SimulationsRun = 0;
        NodesExpanded = 0;
        Determinizations = 0;

        return Root;
    }
    public virtual void PostRun(TarotGame game, NodeMcts node)
    {
        TotalNodesCreated += NodesCreated;
        TotalSimulationsRun += SimulationsRun;
        TotalNodesExpanded += NodesExpanded;
        TotalDeterminizations += Determinizations;
        Metrics.Instance.RecordMetric("IterationsRun", game.State, IterationsRun);
        Metrics.Instance.RecordMetric("NodesCreated", game.State, TotalNodesCreated);
        Metrics.Instance.RecordMetric("SimulationsRun", game.State, TotalSimulationsRun);
        Metrics.Instance.RecordMetric("NodesExpanded", game.State, TotalNodesExpanded);
        Metrics.Instance.RecordMetric("Determinizations", game.State, TotalDeterminizations);
    }
}

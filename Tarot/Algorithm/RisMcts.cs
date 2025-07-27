namespace Tarot.Algorithm;

using Tarot.Game;

public class RisMcts(Player player, int iterations) : IsMcts(player, iterations)
{

    protected override TarotGameState Determinize(TarotGameState state, NodeMcts node)
    {
        throw new NotImplementedException();
    }
}
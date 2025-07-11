namespace Tarot.Algorithm;

using Tarot.Game;

public class RaveMcts(byte player, int iterations) : IsMcts(iterations, player)
{
    protected override TarotGameState Determinize(TarotGame game, NodeMcts node)
    {
        throw new NotImplementedException();
    }
}

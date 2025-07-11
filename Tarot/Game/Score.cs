namespace Tarot.Game;

using DeclareAction = byte;

public static class Score
{
    public const DeclareAction None = Constants.MaxCards + Constants.MaxBidding + 0;
    public const DeclareAction Chelem = Constants.MaxCards + Constants.MaxBidding + 1;
    public const DeclareAction Poignee = Constants.MaxCards + Constants.MaxBidding + 2;
    
    public static float PartialScore()
    {
        throw new System.NotImplementedException();
    }
    public static float TotalScore()
    {
        throw new System.NotImplementedException();
    }
}
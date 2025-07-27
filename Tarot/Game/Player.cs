namespace Tarot.Game;

public readonly struct Player(byte value)
{
    public byte Value { get; } = value;
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return obj is Player player && Value == player.Value;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public static bool operator ==(Player left, Player right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Player left, Player right)
    {
        return !(left == right);
    }
}
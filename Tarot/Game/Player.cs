namespace Tarot.Game;

public readonly struct Player(byte value)
{
    public byte Value { get; } = value;
    public byte Index => (byte)(Value - 1);
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

    public static Player operator ++(Player player)
    {
        var index = player.Index;
        var newIndex = (index + 1) % Constants.Players; // Wrap around to ensure index is within bounds
        return new Player((byte)(newIndex + 1)); // Convert back to 1-based index
    }
    public static Player operator --(Player player)
    {
        var index = player.Index;
        var newIndex = (index - 1 + Constants.Players) % Constants.Players; // Ensure non-negative index
        return new Player((byte)(newIndex + 1)); // Convert back to 1-based index
    }
}
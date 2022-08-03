public enum HexDirection
{
    NE, E, SE, SW, W, NW

    // SE, S, SW, NW, N, NE atual
}

public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}
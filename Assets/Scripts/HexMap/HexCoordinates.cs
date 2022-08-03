using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, y;

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public int Z
    {
        get
        {
            return -X - Y;
        }
    }

    public override string ToString()
    {
        return "(" +
            X.ToString() + ", " + Z.ToString() + ", " + Y.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Z.ToString() + "\n" + Y.ToString();
    }

    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.INNER_RADIUS * 2f);
        float y = -x;
        float offset = position.y / (HexMetrics.OUTER_RADIUS * 3f);
        x -= offset;
        y -= offset;
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(-x - y);
        int iZ = Mathf.RoundToInt(y);
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(-x - y - iY);
            float dZ = Mathf.Abs(y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iZ - iY;
            }
            else if (dY > dZ)
            {
                iY = -iX - iZ;
            }
        }

        return new HexCoordinates(iX, iY);
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - (y / 2), y);
    }
}
using System.Linq;
using UnityEngine;

public static class HexMetrics
{
    public const float OUTER_RADIUS = 1;
    public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f;

    public static readonly Vector3[] Corners = {
        new Vector3(0f, OUTER_RADIUS, 0f),
        new Vector3(INNER_RADIUS, 0.5f * OUTER_RADIUS, 0f),
        new Vector3(INNER_RADIUS, -0.5f * OUTER_RADIUS, 0f),
        new Vector3(0f, -OUTER_RADIUS, 0f),
        new Vector3(-INNER_RADIUS, -0.5f * OUTER_RADIUS, 0f),
        new Vector3(-INNER_RADIUS, 0.5f * OUTER_RADIUS, 0f),
        new Vector3(0f, OUTER_RADIUS, 0f)
    };

    private static readonly float Sqrt3 = Mathf.Sqrt(3);
    //public static readonly Vector3[] VertexDirections =
    //{
    //    Vector3.zero,
    //    new Vector3(0, 1f/2, 0),
    //    new Vector3(Sqrt3/4, 1f/4, 0),
    //    new Vector3(Sqrt3/4, -1f/4, 0),
    //    new Vector3(0, -1f/2, 0),
    //    new Vector3(-Sqrt3/4, -1f/4, 0),
    //    new Vector3(-Sqrt3/4, 1f/4, 0)
    //};

    public static readonly Vector3[] VertexDirections =
    {
        Vector3.zero,
        new Vector3(1f/2,0 , 0),
        new Vector3(1f/4, Sqrt3/4, 0),
        new Vector3(-1f/4, Sqrt3/4, 0),
        new Vector3(-1f/2, 0, 0),
        new Vector3(-1f/4, -Sqrt3/4, 0),
        new Vector3(1f/4,-Sqrt3/4 , 0)
    };

    public static readonly Vector2[] UVDirections =
        VertexDirections.Select(v => new Vector2(v.x, v.y) + (Vector2.one * 0.5f)).ToArray();
}
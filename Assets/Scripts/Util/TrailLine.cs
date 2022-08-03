using System.Linq;
using UnityEngine;

public class TrailLine : Trailer
{
    [SerializeField] private UILineRenderer LineRenderer;

    public override void Trail(Vector2[] points)
    {
        LineRenderer.gameObject.SetActive(points.Length > 1);
        //LineRenderer.positionCount = points.Length;
        //LineRenderer.SetPositions(points.Select(p => (Vector3)p).ToArray());
        LineRenderer.Points = points.Select(p => p).ToArray();
    }
}

public abstract class Trailer :MonoBehaviour
{
    public abstract void Trail(Vector2[] points);
}
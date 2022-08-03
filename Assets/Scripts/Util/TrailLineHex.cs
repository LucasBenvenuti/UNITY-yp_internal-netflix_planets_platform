using UnityEngine;

public class TrailLineHex : Trailer
{
    //public float HexCellSize;
    public GameObject ArrowPrefab;
    public float CellOffset = 10;
    private GameObject[] _spawnedArrows;

    public override void Trail(Vector2[] points)
    {
        if (_spawnedArrows != null)
        {
            for (int i = 0; i < _spawnedArrows.Length; i++)
            {
                Destroy(_spawnedArrows[i]);
            }
        }
        if (points.Length <= 1)
        {
            return;
        }

        int pointsLenghtMinusLast = points.Length - 1;
        _spawnedArrows = new GameObject[pointsLenghtMinusLast];

        for (int i = 0; i < pointsLenghtMinusLast; i++)
        {
            Vector2 halfDirection = (points[i] - points[i + 1]) * 0.5f;
            var rotation = Quaternion.LookRotation(Vector3.forward, -halfDirection);
            _spawnedArrows[i] = Instantiate(ArrowPrefab,
                                            points[i] - halfDirection + (CellOffset * halfDirection),
                                            rotation,
                                            transform);
        }
    }
}
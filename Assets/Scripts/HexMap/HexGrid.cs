using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HexGrid : MonoBehaviour
{
    [Header("References")]
    public HexCell CellPrefab;

    [Header("Settings")]
    public int XCount = 6;
    public int YCount = 6;
    public Color DefaultColor = Color.white;
    public float CellScale = 60;
    public float SpacingX = 60;
    public float SpacingY = 60;

    public HexCell[] Cells;

    public Action<HexCell[]> CellsSpawned;

    //private void Awake()
    //{
    //    Cells = new HexCell[YCount * XCount];

    //    for (int y = 0, i = 0; y < YCount; y++)
    //    {
    //        for (int x = 0; x < XCount; x++)
    //        {
    //            CreateCell(x, y, i++);
    //        }
    //    }
    //    for (int i = 0; i < Cells.Length; i++)
    //    {
    //        Cells[i].transform.SetAsFirstSibling();
    //    }
    //    CellsSpawned?.Invoke(Cells);
    //}

    //private void OnValidate()
    //{
    //    if (Cells != null)
    //    {
    //        for (int y = 0, i = 0; y < YCount; y++)
    //        {
    //            for (int x = 0; x < XCount; x++)
    //            {
    //                if (i < Cells.Length)
    //                {
    //                    print(i);
    //                    Cells[i].transform.localPosition = LineSpawn(x, y);
    //                    Cells[i].CellScale = CellScale;
    //                    i++;
    //                }
    //            }
    //        }
    //    }
    //}

    //private void Start()
    //{
    //    _hexMesh.SetCells(_cells);
    //}

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + (coordinates.Y * XCount) + (coordinates.Y / 2);
        HexCell cell = Cells[index];
        cell.color = color;
        //_hexMesh.SetCells(_cells);
    }

    private void CreateCell(int x, int y, int i)
    {
        Vector3 position = LineSpawn(x, y);

        HexCell cell = Cells[i] = Instantiate(CellPrefab);
        cell.CellScale = CellScale;
        cell.transform.SetParent(transform, false);
        cell.transform.localEulerAngles = new Vector3(0, 0, 90);
        cell.transform.localPosition = position;
        cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.color = DefaultColor;
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, Cells[i - 1]);
        }
        if (y > 0)
        {
            if ((y & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, Cells[i - XCount]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, Cells[i - XCount - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, Cells[i - XCount]);
                if (x < XCount - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, Cells[i - XCount + 1]);
                }
            }
        }

        cell.Label = cell.Coordinates.ToStringOnSeparateLines();
    }

    // 90 rotation (do not delete)
    //private Vector3 LineSpawn(int x, int y)
    //{
    //    Vector3 position;
    //    position.x = (x + (y * 0.5f) - (y / 2)) * ((CellScale * 0.5f * HexMetrics.INNER_RADIUS * 2f) + Spacing);
    //    position.y = y * ((CellScale * 0.5f * HexMetrics.OUTER_RADIUS * 1.5f) + Spacing);
    //    position.z = 0f;
    //    return position;
    //}

    private Vector3 LineSpawn(int x, int y)
    {
        Vector3 position;
        position.x = (x + (y * 0.5f) - (y / 2)) * ((CellScale * 0.5f * HexMetrics.INNER_RADIUS * 2f) + SpacingX);
        position.y = y * ((CellScale * 0.5f * HexMetrics.OUTER_RADIUS * 1.5f) + SpacingY);
        position.z = 0f;
        return position;
    }

    //private Vector3 CircleSpawn(int x, int y)
    //{
    //    Vector3 position;
    //    position.x = (x + (y * 0.5f) - (y / 2)) * (CellScale * HexMetrics.INNER_RADIUS * 2f);
    //    position.y = y * (CellScale * HexMetrics.OUTER_RADIUS * 1.5f);
    //    position.z = 0f;
    //    return position;
    //}
}
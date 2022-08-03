using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MaskableGraphic
{
    private Mesh _hexMesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private MeshCollider _meshCollider;
    private List<Color> _colors;
    private HexCell[] _cells;

    [SerializeField] private Texture _texture;

    // make it such that unity will trigger our ui element to redraw whenever we change the texture in the inspector
    public Texture Texture
    {
        get
        {
            return _texture;
        }
        set
        {
            if (_texture == value)
                return;

            _texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        SetVerticesDirty();
        SetMaterialDirty();
    }

    // if no texture is configured, use the default white texture as mainTexture
    public override Texture mainTexture
    {
        get
        {
            return _texture == null ? s_WhiteTexture : _texture;
        }
    }

    //protected override void Awake()
    //{
    //    base.Awake();
    //    GetComponent<MeshFilter>().mesh = _hexMesh = new Mesh();
    //    _meshCollider = gameObject.AddComponent<MeshCollider>();
    //    _hexMesh.name = "Hex Mesh";
    //    _vertices = new List<Vector3>();
    //    _triangles = new List<int>();
    //    _colors = new List<Color>();
    //}

    // actually update our mesh
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // Clear vertex helper to reset vertices, indices etc.
        vh.Clear();

        // Bottom left corner of the full RectTransform of our UI element
        // Vector2 bottomLeftCorner = new Vector2(0, 0) - rectTransform.pivot;
        // bottomLeftCorner.x *= rectTransform.rect.width;
        // bottomLeftCorner.y *= rectTransform.rect.height;

        DrawCells(_cells, vh);

        Debug.Log("Mesh was redrawn!");
    }

    public void SetCells(HexCell[] cells)
    {
        _cells = cells;
        SetVerticesDirty();
    }

    private void DrawCells(HexCell[] cells, VertexHelper vh)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i], vh);
        }
    }

    private void Triangulate(HexCell cell, VertexHelper vh)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            var v = vh.currentVertCount;
            var vert = new UIVertex();

            vert.position = center;
            vert.color = cell.color;
            vh.AddVert(vert);

            vert.position = center + HexMetrics.Corners[i];
            vert.color = cell.color;
            vh.AddVert(vert);

            vert.position = center + HexMetrics.Corners[i + 1];
            vert.color = cell.color;
            vh.AddVert(vert);

            vh.AddTriangle(v, v + 1, v + 2);
        }
    }


    //public void Triangulate(HexCell[] cells)
    //{
    //    _hexMesh.Clear();
    //    _vertices.Clear();
    //    _triangles.Clear();
    //    _colors.Clear();
    //    for (int i = 0; i < cells.Length; i++)
    //    {
    //        Triangulate(cells[i]);
    //    }
    //    _hexMesh.vertices = _vertices.ToArray();
    //    _hexMesh.triangles = _triangles.ToArray();
    //    _hexMesh.colors = _colors.ToArray();
    //    _hexMesh.RecalculateNormals();
    //    _meshCollider.sharedMesh = _hexMesh;
    //}

    //private void Triangulate(HexCell cell)
    //{
    //    Vector3 center = cell.transform.localPosition;
    //    for (int i = 0; i < 6; i++)
    //    {
    //        AddTriangle(
    //            center,
    //            center + HexMetrics.Corners[i],
    //            center + HexMetrics.Corners[i + 1]
    //        );
    //        AddTriangleColor(cell.Color);
    //    }
    //}

    //private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    //{
    //    int vertexIndex = _vertices.Count;
    //    _vertices.Add(v1);
    //    _vertices.Add(v2);
    //    _vertices.Add(v3);
    //    _triangles.Add(vertexIndex);
    //    _triangles.Add(vertexIndex + 1);
    //    _triangles.Add(vertexIndex + 2);
    //}

    private void AddTriangleColor(Color color)
    {
        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);
    }
}
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGraphic : MaskableGraphic, ICanvasRaycastFilter
{
    [SerializeField] private float _cellScale;
    [SerializeField] private float _hitboxScale =1;

    public float CellScale
    {
        get { return _cellScale; }
        set
        {
            _cellScale = value;
            rectTransform.sizeDelta = new Vector2(_cellScale, _cellScale);
            SetLayoutDirty();
            SetVerticesDirty();
        }
    }

    private Vector2[] _hitboxPolyPoints;

    [SerializeField] private Texture _texture;
    public Texture Texture
    {
        get { return _texture; }
        set
        {
            if (_texture == value)
                return;

            _texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    public override Texture mainTexture
    {
        get { return _texture == null ? s_WhiteTexture : _texture; }
    }

    //protected override void Awake()
    //{
    //    base.Awake();
    //    _polyPoints = new Vector2[6];
    //}

    protected void OnValidate()
    {
        CellScale = _cellScale;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _hitboxPolyPoints.Length; i++)
        {
            Gizmos.DrawSphere(_hitboxPolyPoints[i] + (Vector2)transform.position, 0.7f);
        }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        SetVerticesDirty();
        SetMaterialDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        _hitboxPolyPoints = new Vector2[6];
        Triangulate(vh);
        AddQuad(vh, Vector2.one * 0.5f * -CellScale, Vector2.one * 0.5f * CellScale, Vector2.zero, Vector2.one);
    }

    private void Triangulate(VertexHelper vh)
    {
        //Vector3 center = transform.position;
        var vert = new UIVertex();

        for (int i = 0; i < 7; i++)
        {
            vert.position = /*center +*/ (HexMetrics.VertexDirections[i] * CellScale);
            vert.uv0 = HexMetrics.UVDirections[i];
            vert.normal = Vector3.up;
            vert.color = color;
            //vh.AddVert(vert);

            if (i != 0) //if is not center
            {
                _hitboxPolyPoints[i - 1] = HexMetrics.VertexDirections[i] * CellScale * _hitboxScale;
            }
        }
        //Removido para utilizar o quad
        //vh.AddTriangle(0, 1, 2);
        //vh.AddTriangle(0, 2, 3);
        //vh.AddTriangle(0, 3, 4);
        //vh.AddTriangle(0, 4, 5);
        //vh.AddTriangle(0, 5, 6);
        //vh.AddTriangle(0, 6, 1);
    }

    //private void Triangulate(VertexHelper vh)
    //{
    //    Vector3 center = transform.localPosition;
    //    for (int i = 0; i < 6; i++)
    //    {
    //        var v = vh.currentVertCount;
    //        var vert = new UIVertex();

    //        vert.position = center;
    //        vert.uv0 = HexMetrics.UVDirections[0];
    //        vert.color = Color;
    //        vh.AddVert(vert);

    //        vert.position = center + HexMetrics.Corners[i];
    //        vert.uv0 = HexMetrics.UVDirections[i];
    //        vert.color = Color;
    //        vh.AddVert(vert);

    //        vert.position = center + HexMetrics.Corners[i + 1];
    //        vert.uv0 = HexMetrics.UVDirections[i + 1];
    //        vert.color = Color;
    //        vh.AddVert(vert);

    //        vh.AddTriangle(v, v + 1, v + 2);
    //    }
    //}

    private void AddQuad(VertexHelper vh, Vector2 corner1, Vector2 corner2, Vector2 uvCorner1, Vector2 uvCorner2)
    {
        var i = vh.currentVertCount;

        UIVertex vert = new UIVertex();
        vert.color = color;  // Do not forget to set this, otherwise

        vert.position = corner1;
        vert.uv0 = uvCorner1;
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner1.y);
        vert.uv0 = new Vector2(uvCorner2.x, uvCorner1.y);
        vh.AddVert(vert);

        vert.position = corner2;
        vert.uv0 = uvCorner2;
        vh.AddVert(vert);

        vert.position = new Vector2(corner1.x, corner2.y);
        vert.uv0 = new Vector2(uvCorner1.x, uvCorner2.y);
        vh.AddVert(vert);

        vh.AddTriangle(i + 0, i + 2, i + 1);
        vh.AddTriangle(i + 3, i + 2, i + 0);
    }


    public bool IsRaycastLocationValid(Vector2 screenPosition, Camera eventCamera)
    {
        var isInside = RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            screenPosition,
            eventCamera,
            out Vector3 worldPoint
        );

        var worldPoly = new Vector2[_hitboxPolyPoints.Length];

        for (int i = 0; i < _hitboxPolyPoints.Length; i++)
        {
            worldPoly[i] = _hitboxPolyPoints[i] + (Vector2)transform.position;
        }

        if (isInside)
            isInside = Poly.ContainsPoint(worldPoly, worldPoint);

        return isInside;
    }
}
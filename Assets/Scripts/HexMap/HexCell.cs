using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    public HexCoordinates Coordinates;
    [SerializeField] private HexCell[] _neighbors;

    public Text LabelDisplay;
    [SerializeField] private HexGraphic HexGraphic;
    [SerializeField] private Graphic Graphic;

    public float CellScale
    {
        get
        {
            return HexGraphic ? HexGraphic.CellScale : 0;
        }
        set
        {
            if (HexGraphic)
            {
                HexGraphic.CellScale = value;
            }
        }
    }

    public Color color
    {
        get
        {
            return Graphic ? HexGraphic.color : Color.white;
        }
        set
        {
            if (Graphic)
            {
                Graphic.color = value;
            }
        }
    }

    public string Label
    {
        get { return LabelDisplay.text; }
        set { LabelDisplay.text = value; }
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return _neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        _neighbors[(int)direction] = cell;
        cell._neighbors[(int)direction.Opposite()] = this;
    }

    public bool IsNeighbourTo(HexCell hexCell)
    {
        return _neighbors.Contains(hexCell);
    }
}
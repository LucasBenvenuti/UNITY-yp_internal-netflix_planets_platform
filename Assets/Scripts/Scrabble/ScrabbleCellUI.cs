using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrabbleCellUI : MonoBehaviour, IPointerDownHandler,
                                             IPointerUpHandler,
                                             IPointerEnterHandler,
                                             IPointerExitHandler
{
    public HexCell HexCell;
    public HexGraphic HexGraphic;
    public Text TextDisplay;
    public Texture SelectedTexture;
    public Texture PositiveTexture;
    public Texture NegativeTexture;

    private Texture _defaultTexture;

    [SerializeField] private char _char;
    public char Character
    {
        get { return _char; }
        set
        {
            _char = value;
            if (TextDisplay)
            {
                TextDisplay.text = _char.ToString();
            }
        }
    }

    public Action<ScrabbleCellUI, PointerEventData> PointerDown;
    public Action<ScrabbleCellUI, PointerEventData> PointerUp;
    public Action<ScrabbleCellUI, PointerEventData> PointerEnter;
    public Action<ScrabbleCellUI, PointerEventData> PointerExit;

    private void OnValidate()
    {
        Character = _char;
    }

    private void Awake()
    {
        if (HexGraphic == null)
        {
            HexGraphic = GetComponent<HexGraphic>();
        }
        _defaultTexture = HexGraphic.Texture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown?.Invoke(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUp?.Invoke(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke(this, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke(this, eventData);
    }

    public void SetHighlightActive(bool active)
    {
        HexGraphic.Texture = active ? SelectedTexture : _defaultTexture;
        CancelInvoke("ResetColor");
    }

    public bool IsNeighbourTo(ScrabbleCellUI lastSelectedCell)
    {
        return lastSelectedCell != null
               && HexCell.IsNeighbourTo(lastSelectedCell.HexCell);
    }

    public void ActivatePositiveFeedback()
    {
        HexGraphic.Texture = PositiveTexture;
        Invoke("ResetColor", 0.5f);
    }

    public void ActivateNegativeFeedback()
    {
        HexGraphic.Texture = NegativeTexture;
        Invoke("ResetColor", 0.5f);
    }

    private void ResetColor()
    {
        HexGraphic.Texture = _defaultTexture;
    }
}
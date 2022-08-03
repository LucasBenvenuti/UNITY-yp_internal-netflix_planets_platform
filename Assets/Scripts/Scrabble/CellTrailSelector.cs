using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using SF = UnityEngine.SerializeField;

public class CellTrailSelector : MonoBehaviour
{
    [Header("References")]
    [SF] private Trailer _trailLine;

    public Action<ScrabbleCellUI> SelectionBegan;
    public Action<ScrabbleCellUI[]> SelectionUpdated;
    public Action<ScrabbleCellUI[]> SelectionEnded;

    public ScrabbleCellUI LastSelectedCell
    {
        get
        {
            return _selectedCells.Count > 0
                   ? _selectedCells[_selectedCells.Count - 1]
                   : null;
        }
    }

    public bool IsSelecting => _isSelecting;

    private bool _isSelecting;
    private List<ScrabbleCellUI> _selectedCells;

    public void Setup(ScrabbleCellUI[] activeCells)
    {
        _selectedCells = new List<ScrabbleCellUI>();
        for (int i = 0; i < activeCells.Length; i++)
        {
            activeCells[i].PointerDown += OnCellPointerDown;
            activeCells[i].PointerUp += OnCellPointerUp;
            activeCells[i].PointerEnter += OnCellPointerEnter;
        }
        RetrailSelectionLine();
    }

    private void ResetSelection()
    {
        _selectedCells.Clear();
        RetrailSelectionLine();
    }

    private void BeginSelection(ScrabbleCellUI cell)
    {
        AddCellToSelection(cell);
        _isSelecting = true;
        SelectionBegan?.Invoke(cell);
    }

    private void EndSelection()
    {
        _isSelecting = false;
        foreach (ScrabbleCellUI cell in _selectedCells)
        {
            cell.SetHighlightActive(false);
        }
        SelectionEnded?.Invoke(_selectedCells.ToArray());
    }

    private void AddCellToSelection(ScrabbleCellUI cell)
    {
        _selectedCells.Add(cell);
        cell.SetHighlightActive(true);
        RetrailSelectionLine();
        SelectionUpdated?.Invoke(_selectedCells.ToArray());
    }

    private void RemoveCellFromSelection(ScrabbleCellUI cell)
    {
        cell.SetHighlightActive(false);
        _selectedCells.Remove(cell);
        RetrailSelectionLine();
        SelectionUpdated?.Invoke(_selectedCells.ToArray());
    }

    private void RetrailSelectionLine()
    {
        Vector2[] trailPoints =
            _selectedCells.Select(c => (Vector2)c.transform.position).ToArray();
        _trailLine.Trail(trailPoints);
    }

    private void OnCellPointerDown(ScrabbleCellUI cell,
                                   PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ResetSelection();
            BeginSelection(cell);
        }
    }

    private void OnCellPointerUp(ScrabbleCellUI cell,
                                 PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EndSelection();
            ResetSelection();
        }
    }

    private void OnCellPointerEnter(ScrabbleCellUI cell,
                                    PointerEventData eventData)
    {
        if (!IsSelecting)
        {
            return;
        }

        if (_selectedCells.Contains(cell))
        {
            int repeatedCellIndex = _selectedCells.IndexOf(cell);
            if (repeatedCellIndex < _selectedCells.Count - 1)
            {
                int index = repeatedCellIndex + 1;
                List<ScrabbleCellUI> cellsToRemove =
                    _selectedCells.GetRange(index, _selectedCells.Count - index);
                for (int i = 0; i < cellsToRemove.Count; i++)
                {
                    RemoveCellFromSelection(cellsToRemove[i]);
                }
            }
        }
        else if (cell.IsNeighbourTo(LastSelectedCell))
        {
            AddCellToSelection(cell);
            RetrailSelectionLine();
        }
    }
}
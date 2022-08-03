using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

public class FiniteScrabbleMinigame : MonoBehaviour
{
    [Header("References")]
    [SF] private Text _currentSelectionDisplay;
    [SF] private CellTrailSelector _trailSelector;
    [SF] private HexGrid _hexGrid;
    [SF] private string[] _validWords;

    public Action AllWordsFound;
    public Action<string> SelectedValidWord;
    public Action<string> SelectedInvalidWord;
    public Action<string[]> WordsSet;
    public UnityEvent<string> WordSelected;

    private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    private ScrabbleCellUI[] _spawnedScrabbleCellUIs;

    private void Awake()
    {        

        for (int i = 0; i < _validWords.Length; i++)
        {
            _validWords[i] = Regex.Replace(_validWords[i], @"\s+", "").ToUpper();
            if (!_dictionary.Keys.Contains(_validWords[i]))
            {
                _dictionary.Add(_validWords[i], _validWords[i]);
            }
        }

        _trailSelector.SelectionUpdated += OnUpdateSelection;
        _trailSelector.SelectionEnded += OnEndSelection;
    }

    private void OnEnable()
    {
        WordsSet?.Invoke(_validWords);
        OnCellsSpawned(_hexGrid.Cells);
    }

    public bool IsWordValid(string word)
    {
        word = word.ToUpper();
        return _dictionary.ContainsKey(word);
    }

    private void OnCellsSpawned(HexCell[] spawnedCells)
    {
        _spawnedScrabbleCellUIs =
            spawnedCells.Select(cell => cell.GetComponent<ScrabbleCellUI>()).ToArray();

        for (int i = 0; i < _spawnedScrabbleCellUIs.Length; i++)
        {
            _spawnedScrabbleCellUIs[i].gameObject.SetActive(true);
        }
        _trailSelector.Setup(_spawnedScrabbleCellUIs);
    }

    private void OnUpdateSelection(ScrabbleCellUI[] selectedCells)
    {
        _currentSelectionDisplay.text = CellArrayToString(selectedCells).ToLower().FirstCharToUpper();
    }

    private void OnEndSelection(ScrabbleCellUI[] selectedCells)
    {
        _currentSelectionDisplay.text = "";
        string selectedWord = CellArrayToString(selectedCells);
        WordSelected?.Invoke(selectedWord);
        if (IsWordValid(selectedWord))
        {
            for (int i = 0; i < selectedCells.Length; i++)
            {
                selectedCells[i].gameObject.SetActive(false); //ou executar animacao
            }
            bool allCellsAreDisabled = _spawnedScrabbleCellUIs.All(cell =>
            {
                return !cell.gameObject.activeInHierarchy;
            });
            if (allCellsAreDisabled)
            {
                AllWordsFound?.Invoke();
            }

            SelectedValidWord?.Invoke(selectedWord);
            TriggerExistentWordHighlight(selectedCells);
            ScrabbleScoreManager.Instance?.OnSelectedValidWord(selectedWord);
        }
        else
        {
            SelectedInvalidWord?.Invoke(selectedWord);
            TriggerInexistentWordHighlight(selectedCells);
            ScrabbleScoreManager.Instance?.OnSelectedInvalidWord(selectedWord);
        }
    }

    private void TriggerExistentWordHighlight(ScrabbleCellUI[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].ActivatePositiveFeedback();
        }
    }

    private void TriggerInexistentWordHighlight(ScrabbleCellUI[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].ActivateNegativeFeedback();
        }
    }

    private string CellArrayToString(ScrabbleCellUI[] cells)
    {
        return new string(cells.Select(c => c.Character).ToArray());
    }

    [System.Serializable]
    public class CharWithProbability
    {
        public char Char;
        public float Probability;

        public CharWithProbability(char @char, float probability)
        {
            Char = @char;
            Probability = probability;
        }
    }
}
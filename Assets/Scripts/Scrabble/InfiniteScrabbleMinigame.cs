using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DataStructures.RandomSelector;
using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

public class InfiniteScrabbleMinigame : MonoBehaviour
{
    private const string ALL_UPPER_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";/*abcdefghijklmnopqrstuvwxyz0123456789*/
    private const string ALL_CHARS_PTBR_REGEX = "^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ]+$";
    private const string ALL_CHARS_NO_SPECIAL_REGEX = "^[A-Za-z]+$";

    [Header("References")]
    [SF] private Text _currentSelectionDisplay;
    [SF] private CellTrailSelector _trailSelector;
    [SF] private HexGrid _hexGrid;
    [SF] private TextAsset _dictionaryAsset;

    [Header("Settings")]
    [SF] private List<CharWithProbability> _spawningChars;

    public Action<string> SelectedValidWord;
    public Action<string> SelectedInvalidWord;

    private readonly string _validCharsRegex = ALL_CHARS_NO_SPECIAL_REGEX;
    private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    private ScrabbleCellUI[] _spawnedScrabbleCellUIs;

    private void OnValidate()
    {
        ValidateSpawningCharsField();
    }

    private void Awake()
    {
        foreach (var word in _dictionaryAsset.text.Split("\n"[0]))
        {
            //Debug.Log(word);
            //Regex.IsMatch(word, _validCharsRegex) &&

            var w = Regex.Replace(word, @"\s+", "");
            if (!_dictionary.Keys.Contains(w))
            {
                _dictionary.Add(w, w);
            }
        }

        _hexGrid.CellsSpawned += OnCellsSpawned;
        _trailSelector.SelectionBegan += OnBeginSelection;
        _trailSelector.SelectionUpdated += OnUpdateSelection;
        _trailSelector.SelectionEnded += OnEndSelection;
    }

    public bool IsWordValid(string word)
    {
        return word.Length > 2 && _dictionary.ContainsKey(word);
    }

    private void OnCellsSpawned(HexCell[] spawnedCells)
    {
        _spawnedScrabbleCellUIs =
            spawnedCells.Select(cell => cell.GetComponent<ScrabbleCellUI>()).ToArray();
        _trailSelector.Setup(_spawnedScrabbleCellUIs);
        SetRandomLettersToCells(_spawnedScrabbleCellUIs);
    }

    private void OnBeginSelection(ScrabbleCellUI selectedCell)
    {
    }

    private void OnUpdateSelection(ScrabbleCellUI[] selectedCells)
    {
        _currentSelectionDisplay.text = CellArrayToString(selectedCells);
    }

    private void OnEndSelection(ScrabbleCellUI[] selectedCells)
    {
        string selectedWord = CellArrayToString(selectedCells);
        if (IsWordValid(selectedWord))
        {
            SelectedValidWord?.Invoke(selectedWord);
            TriggerExistentWordHighlight(selectedCells);
            SetRandomLettersToCells(selectedCells);
        }
        else
        {
            SelectedInvalidWord?.Invoke(selectedWord);
            TriggerInexistentWordHighlight(selectedCells);
        }
    }

    private void SetRandomLettersToCells(ScrabbleCellUI[] cells)
    {
        var selector = new DynamicRandomSelector<char>();
        foreach (CharWithProbability c in _spawningChars)
        {
            selector.Add(c.Char, c.Probability);
        }
        selector.Build();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Character = selector.SelectRandomItem();
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

    private void ValidateSpawningCharsField()
    {
        var savedProbabilities = new List<float>();
        for (int i = 0; i < _spawningChars.Count; i++)
        {
            savedProbabilities.Add(_spawningChars[i].Probability);
        }

        if (_spawningChars.Count != ALL_UPPER_CHARS.Length)
        {
            _spawningChars = ALL_UPPER_CHARS.Select(c => new CharWithProbability(c, 0))
                                           .ToList();
            for (int i = 0; i < savedProbabilities.Count; i++)
            {
                _spawningChars[i].Probability = savedProbabilities[i];
            }
        }

        for (int i = 0; i < _spawningChars.Count; i++)
        {
            _spawningChars[i].Char = ALL_UPPER_CHARS[i];
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
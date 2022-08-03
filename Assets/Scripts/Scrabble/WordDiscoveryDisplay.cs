using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class WordDiscoveryDisplay : MonoBehaviour
{
    [SF] private List<ScrabbleCellUI> _scrabbleCells;
    [SF] private ScrabbleCellUI _scrabbleCellPrefab;
    [SF] private Texture _allRevealedTexture;

    public string WordToDiscover { get; private set; }

    public bool IsAllRevealed => _revealedCells.All(c => c);

    private bool[] _revealedCells;

    public int RevealedCount => _revealedCells.Count(c => c);

    public void Setup(FiniteScrabbleMinigame manager, string word)
    {
        manager.SelectedValidWord += OnValidWordSelected;
        WordToDiscover = word;

        for (int i = 0; i < word.Length; i++)
        {
            ScrabbleCellUI spawnedCell = Instantiate(_scrabbleCellPrefab, transform);
            spawnedCell.Character = ' ';
            spawnedCell.HexGraphic.color = Color.gray;
            _scrabbleCells.Add(spawnedCell);
        }
        _revealedCells = new bool[word.Length];
    }

    public void HideAll()
    {
        for (int i = 0; i < _scrabbleCells.Count; i++)
        {
            HideAtIndex(i);
        }
    }

    public void RevealFirst()
    {
        RevealAtIndex(0);
    }

    public void RevealAll()
    {
        for (int i = 0; i < _scrabbleCells.Count; i++)
        {
            RevealAtIndex(i);
        }
    }

    public void RevealRandom()
    {
        //if (IsAllRevealed)
        //{
        //    return;
        //}
        //Random.Range(0, _scrabbleCells.Count);
        //int random;
        //do
        //{
        //    random = Random.Range(0, _scrabbleCells.Count);
        //} while (_scrabbleCells[random].gameObject.activeInHierarchy);
        //RevealAtIndex(random);

        //segunda melhor opção fazer cache daqueles ainda nao selecionados
    }

    public void RevealAtIndex(int i)
    {
        if (i >= 0 && i < _scrabbleCells.Count)
        {
            _scrabbleCells[i].HexGraphic.color = Color.white;
            _scrabbleCells[i].Character = WordToDiscover[i];
            _revealedCells[i] = true;
        }
        if (IsAllRevealed)
        {
            for (int j = 0; j < _scrabbleCells.Count; j++)
            {
                _scrabbleCells[j].HexGraphic.color = Color.white;
                _scrabbleCells[j].HexGraphic.Texture = _allRevealedTexture;
            }
        }
    }

    public void HideAtIndex(int i)
    {
        if (i >= 0 && i < _scrabbleCells.Count)
        {
            _scrabbleCells[i].HexGraphic.color = Color.gray;
            _scrabbleCells[i].Character = ' ';
            _revealedCells[i] = false;
        }
    }

    private void OnValidWordSelected(string validWord)
    {
        if (validWord == WordToDiscover)
        {
            RevealAll();
        }
    }
}
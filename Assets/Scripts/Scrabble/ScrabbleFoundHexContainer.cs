using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrabbleFoundHexContainer : MonoBehaviour
{
    private const int BUY_LIMIT = 4;
    public WordDiscoveryDisplay DiscoveredHexTextPrefab;
    public RectTransform DiscoveredContainer;
    [Header("Tips shop")]
    public Text CostDisplay;
    public Button BuyButton;
    public int BuyTipsCost;
    public int BuyTipsMultiplierPerLetterRevealed;

    private FiniteScrabbleMinigame _scrabbleMinigame;
    private List<WordDiscoveryDisplay> _spawnedWordDisplays = new List<WordDiscoveryDisplay>();
    private int _lettersRevealed;
    private int TotalTipsCost => BuyTipsCost + (BuyTipsCost * _lettersRevealed);

    public void Setup(FiniteScrabbleMinigame manager)
    {
        _lettersRevealed = 0;
        SetCostDisplay("-" + TotalTipsCost);
        SetBuyButtonInteractable(true);

        if (_scrabbleMinigame != null)
        {
            _scrabbleMinigame.WordsSet -= SpawnWords;
        }

        _scrabbleMinigame = manager;
        manager.WordsSet += SpawnWords;
    }

    public void SpawnWords(string[] words)
    {
        if (_spawnedWordDisplays.Count > 0)
        {
            for (int i = 0; i < _spawnedWordDisplays.Count; i++)
            {
                Destroy(_spawnedWordDisplays[i].gameObject);
            }
            _spawnedWordDisplays.Clear();
        }

        for (int i = 0; i < words.Length; i++)
        {
            WordDiscoveryDisplay wD = Instantiate(DiscoveredHexTextPrefab,
                                                  DiscoveredContainer);
            _spawnedWordDisplays.Add(wD);
            wD.Setup(_scrabbleMinigame, words[i]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(DiscoveredContainer);
    }

    public void RevealFirstsCharacters()
    {
        if (ScrabbleScoreManager.Instance.Buy(BuyTipsCost))
        {
            for (int i = 0; i < _spawnedWordDisplays.Count; i++)
            {
                _spawnedWordDisplays[i].RevealFirst();
            }
        }
    }

    public void BuyCharacters()
    {
        if (ScrabbleScoreManager.Instance.Buy(TotalTipsCost))
        {
            for (int i = 0; i < _spawnedWordDisplays.Count; i++)
            {
                _spawnedWordDisplays[i].RevealAtIndex(_lettersRevealed);
            }
            _lettersRevealed++;
            SetCostDisplay("-" + TotalTipsCost);

            if (_spawnedWordDisplays.All(wd => wd.RevealedCount >= BUY_LIMIT))
            {
                SetCostDisplay("");
                SetBuyButtonInteractable(false);
            }
        }
    }

    private void SetBuyButtonInteractable(bool interactable)
    {
        if (BuyButton)
        {
            BuyButton.interactable = interactable;
        }
    }

    private void SetCostDisplay(string text)
    {
        if (CostDisplay)
        {
            CostDisplay.text = text;
        }
    }
}
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private const int SECRET_LEVEL_INDEX = 10;

    [Header("LevelComplete Refs")]
    public GameObject LevelCompletePanel;
    public ScrabbleFoundHexContainer LevelCompleteWordsFound;

    [Header("References")]
    public GameObject MenuGameObject;
    public GameObject FirstPanelGameObject;
    public GameObject FinalPanelGameObject;
    public ScrabbleFoundHexContainer WordsFoundDisplayer;
    public LevelTransitionPanel LevelTransitionPanel;
    public Text GameLevelNumberDisplay;
    public TextMeshProUGUI LevelHintDisplay;
    public Text LevelEndHintDisplay;
    public Image LevelProgressionFill;

    [Header("Level Buttons")]
    public LevelSelectorButton LevelButtonPrefab;
    public Transform LevelsContainer;

    [Header("Level Data")]
    public ScrabbleLevel[] Levels;

    private int _lastLoadedLevelIndex;
    private List<LevelSelectorButton> _levelSelectors = new List<LevelSelectorButton>();

    private void Awake()
    {
        LevelHintDisplay.text = "";
        for (int i = 0; i < Levels.Length; i++)
        {
            if (i != SECRET_LEVEL_INDEX)
            {
                SpawnLevelButtonByIndex(i);
            }
        }
    }

    private void SpawnLevelButtonByIndex(int i)
    {
        LevelSelectorButton levelButton = Instantiate(LevelButtonPrefab,
                                                      LevelsContainer);
        string levelName = i + 1 + "";
        levelButton.Setup(levelName, i);
        levelButton.Selected += LoadLevel;
        levelButton.SetInteractable(Levels[i].IsUnlocked);
        _levelSelectors.Add(levelButton);
    }

    public void UnlockNextLevel()
    {
        int nextLevelIndex = _lastLoadedLevelIndex + 1;
        if (nextLevelIndex < Levels.Length - 1)
        {
            UnlockLevel(nextLevelIndex);
        }
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = _lastLoadedLevelIndex + 1;
        if (nextLevelIndex < Levels.Length)
        {
            LoadLevel(nextLevelIndex);
        }
    }

    public void ReloadLevel()
    {
        LoadLevel(_lastLoadedLevelIndex);
    }

    private void UnlockLevel(int index)
    {
        Levels[index].IsUnlocked = true;
        //if (index == SECRET_LEVEL_INDEX)
        //{
        //    SpawnLevelButtonByIndex(SECRET_LEVEL_INDEX);
        //}
        _levelSelectors[index].SetInteractable(true);
    }

    private void LoadLevel(int index)
    {
        LevelProgressionFill.fillAmount = Mathf.InverseLerp(0,
                                                            Levels.Length,
                                                            index + 1);

        MenuGameObject.gameObject.SetActive(false);

        //Unload all
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].UnloadLevel();
        }
        Levels[_lastLoadedLevelIndex].Manager.AllWordsFound -= OnLevelComplete;

        //Load
        ScrabbleLevel levelToLoad = Levels[index];
        levelToLoad.Manager.AllWordsFound += OnLevelComplete;
        WordsFoundDisplayer.Setup(levelToLoad.Manager);
        LevelCompleteWordsFound.Setup(levelToLoad.Manager);
        levelToLoad.LoadLevel();
        _lastLoadedLevelIndex = index;

        GameLevelNumberDisplay.text = "Nível " + (index + 1);
        LevelHintDisplay.text = levelToLoad.LevelHintText;
        LevelEndHintDisplay.text = "";
        LevelTransitionPanel.SetInfo(levelToLoad.LevelHintText, index + 1);

        // if (index == 0)
        // {
            LevelTransitionPanel.SetActive(true);
        // }
        // else
        // {
        //     LevelTransitionPanel.SetActive(true);
        // }
        LevelCompletePanel.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnLevelComplete()
    {
        if (Levels.Length == _lastLoadedLevelIndex + 1)
        {
            FinalPanelGameObject.SetActive(true);
            return;
        }

        UnlockNextLevel();
        string levelEndText = Levels[_lastLoadedLevelIndex].LevelEndText;
        if (levelEndText.Length > 0)
        {
            LevelEndHintDisplay.text = levelEndText;
        }
        else
        {
            LevelEndHintDisplay.text = "Parabéns, você encontrou as pistas!";
        }
        LevelCompletePanel.SetActive(true);
    }
}
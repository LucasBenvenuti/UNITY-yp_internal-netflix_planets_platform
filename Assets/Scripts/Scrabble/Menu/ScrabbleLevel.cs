using System;
using UnityEngine;
[Serializable]
public class ScrabbleLevel
{
    public FiniteScrabbleMinigame Manager;
    public bool IsUnlocked;
    [TextArea] public string LevelHintText;
    [TextArea] public string LevelEndText;

    public ScrabbleLevel(bool isUnlocked, FiniteScrabbleMinigame manager)
    {
        IsUnlocked = isUnlocked;
        Manager = manager;
    }

    public void LoadLevel()
    {
        Manager.gameObject.SetActive(true);
    }

    public void UnloadLevel()
    {
        Manager.gameObject.SetActive(false);
    }
}
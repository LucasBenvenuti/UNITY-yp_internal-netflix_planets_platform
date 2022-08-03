using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

public class ScrabbleScoreManager : MonoBehaviour
{
    #region Singleton
    public static ScrabbleScoreManager Instance { get; private set; }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SF] private Text _scoreDisplay;
    public int PointsPerCharacter = 20;
    public int PointsPerWord = 20;

    public int CurrentScore { get; protected set; } = 30;

    private void Awake()
    {
        SetupSingleton();
        RefreshUI();
    }

    public void OnSelectedValidWord(string validWord)
    {
        AddScore(validWord.Length * PointsPerCharacter + PointsPerWord);
    }

    public void OnSelectedInvalidWord(string invalidWord)
    {

    }

    private void RefreshUI()
    {
        _scoreDisplay.text = CurrentScore + "";
    }

    public bool Buy(int price)
    {
        if (price <= CurrentScore)
        {
            RemoveScore(price);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddScore(int scoreToAdd)
    {
        scoreToAdd = Mathf.Abs(scoreToAdd);
        CurrentScore += scoreToAdd;
        RefreshUI();
    }

    private void RemoveScore(int scoreToRemove)
    {
        scoreToRemove = Mathf.Abs(scoreToRemove);
        CurrentScore -= scoreToRemove;
        RefreshUI();
    }
}
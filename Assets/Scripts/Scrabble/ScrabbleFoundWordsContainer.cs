using UnityEngine;
using UnityEngine.UI;

public class ScrabbleFoundWordsContainer : MonoBehaviour
{
    public Text DiscoveredTextPrefab;
    public InfiniteScrabbleMinigame ScrabbleMinigame;
    public Transform DiscoveredContainer;

    private void Awake()
    {
        ScrabbleMinigame.SelectedValidWord += SpawnWord;
    }

    public void SpawnWord(string word)
    {
        Instantiate(DiscoveredTextPrefab, DiscoveredContainer).text = word;
    }
}
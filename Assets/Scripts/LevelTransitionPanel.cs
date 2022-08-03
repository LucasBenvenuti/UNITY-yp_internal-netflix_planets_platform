using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionPanel : MonoBehaviour
{
    [SerializeField] private Text _tipsDisplay;
    [SerializeField] private Text _levelNumberDisplay;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetInfo(string text, int levelNumber)
    {
        _tipsDisplay.text = text;
        _levelNumberDisplay.text = levelNumber + "";
    }
}
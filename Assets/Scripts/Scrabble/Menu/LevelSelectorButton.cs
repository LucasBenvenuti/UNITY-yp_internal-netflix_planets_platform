using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorButton : MonoBehaviour
{
    public Action<int> Selected;
    public Text TextDisplay;
    public GameObject lockIcon;
    public Image[] Images;
    public Button Button;
    public int LevelIndex;

    public void Setup(string title, int levelIndex)
    {
        LevelIndex = levelIndex;
        SetText(title);
    }

    public void SetInteractable(bool interactable)
    {
        Button.interactable = interactable;
        if(interactable == true)
        {
            lockIcon.SetActive(false);
            TextDisplay.gameObject.SetActive(true);
        }
    }

    public void SetText(string text)
    {
        TextDisplay.text = text;
    }

    public void OnClick()
    {
        Selected?.Invoke(LevelIndex);
    }
}
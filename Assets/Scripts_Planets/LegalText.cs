using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalText : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public static LegalText Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowText()
    {
        StartCoroutine(DisplayChangeRoutine(true));
    }

    public void HideText()
    {
        StartCoroutine(DisplayChangeRoutine(false));
    }

    IEnumerator DisplayChangeRoutine(bool state)
    {
        if(state)
        {
            for(float f = 0; f < 1.0f; f += Time.deltaTime)
            {
                canvasGroup.alpha = f;
                yield return null;
            }
        }
        else
        {
            for(float f = 1.0f; f > 0f; f -= Time.deltaTime)
            {
                canvasGroup.alpha = f;
                yield return null;
            }
        }
    }
}

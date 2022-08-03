using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public CanvasGroup screen;
    public float tweenDuration;
    public LeanTweenType tweenType;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        Open();
    }

    public void Open()
    {
        StopCoroutine(OpenScene(1f));
        StartCoroutine(OpenScene(1f));
    }

    public void Close(string sceneToChange, float delayToChangeScene)
    {
        StopCoroutine(CloseScene(sceneToChange, delayToChangeScene));
        StartCoroutine(CloseScene(sceneToChange, delayToChangeScene));
    }

    IEnumerator OpenScene(float delayToChangeScene)
    {
        yield return new WaitForSeconds(delayToChangeScene);

        LeanTween.alphaCanvas(screen, 0f, tweenDuration).setEase(tweenType).setOnComplete(() =>
        {
            screen.blocksRaycasts = false;
            screen.interactable = false;
        });
    }

    IEnumerator CloseScene(string sceneToChange, float delayToChangeScene)
    {
        yield return null;

        LeanTween.alphaCanvas(screen, 1f, tweenDuration).setEase(tweenType).setOnStart(() =>
        {
            screen.blocksRaycasts = true;
            screen.interactable = true;
        });

        yield return new WaitForSeconds(tweenDuration);

        if (sceneToChange != "" || sceneToChange == null)
        {
            yield return new WaitForSeconds(delayToChangeScene);

            SceneManager.LoadScene(sceneToChange, LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        StartCoroutine(OpenScene(1f));
    }
}

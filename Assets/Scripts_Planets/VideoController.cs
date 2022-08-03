using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public static VideoController instance;
    public CanvasGroup videoCanvas;
    public VideoPlayer videoPlayer;
    public Animator animator;
    public Color idleColor;
    public RawImage videoPlane;

    public float tweenDuration;
    public LeanTweenType tweenType;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        // videoCanvas.SetActive(false);
        videoCanvas.interactable = false;
        videoCanvas.blocksRaycasts = false;
        videoCanvas.alpha = 0;
        videoPlane.color = idleColor;
    }

    void Start()
    {
        if (SceneController.instance)
        {
            SceneController.instance.Open();
        }
    }

    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }

    public void ShowUI(bool visibility, VideoClip videoClip)
    {
        StopCoroutine(VideoCtrl(visibility, videoClip));
        StartCoroutine(VideoCtrl(visibility, videoClip));
    }

    public IEnumerator VideoCtrl(bool visibility, VideoClip videoClip)
    {
        yield return null;

        if (visibility)
        {
            LeanTween.alphaCanvas(videoCanvas, 1f, tweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                videoPlayer.clip = videoClip;
                videoCanvas.interactable = true;
                videoCanvas.blocksRaycasts = true;
            });

            yield return new WaitForSeconds(tweenDuration);

            videoPlayer.Play();

            yield return new WaitForSeconds(0.5f);

            animator.SetTrigger("Show");
        }
        else
        {
            LeanTween.alphaCanvas(videoCanvas, 0f, tweenDuration).setEase(tweenType).setOnStart(() =>
            {
                videoCanvas.interactable = false;
                videoCanvas.blocksRaycasts = false;

                videoPlayer.Stop();

                videoPlayer.clip = videoClip;
                videoPlane.color = idleColor;
            });
        }
    }
}

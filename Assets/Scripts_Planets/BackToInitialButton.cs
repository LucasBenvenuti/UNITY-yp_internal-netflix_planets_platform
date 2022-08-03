using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackToInitialButton : MonoBehaviour
{
    public static BackToInitialButton instance;

    public bool isAnimating;

    public bool isGoingBack;

    [SerializeField] Animator animator;

    [SerializeField] Vector3 originalCameraRotation = new Vector3(-0.389999986f, 5.30000019f, 68f);

    [SerializeField] float cameraRotationDuration = 1f;

    bool isRotatingCamera;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        isGoingBack = false;
        isAnimating = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        var button = GetComponent<Button>();
        if (button.enabled)
        {
            button.onClick.AddListener(BeginGoingBack);
        }
    }

    public void BeginGoingBack()
    {
        if (!StartSceneBttn.finishedAnimation)
        {
            return;
        }

        isGoingBack = true;

        if(MousePan.instance.target != null)
        {
            MousePan.instance.GoBack();
            StartCoroutine(WaitForCameraBack(GoBack));
        }
        else
        {
            GoBack();
        }
        MousePan.instance.StartCoroutine(MousePan.instance.ChangeLinesVisibility(true));
    }

    void GoBack()
    {
        StartCoroutine(BackToOriginalRotation(StartAnimationReverse));
    }

    IEnumerator WaitForCameraBack(Action callback)
    {
        while (MousePan.instance.isGoingBack)
        {
            yield return null;
        }

        callback();
    }

    void StartAnimationReverse()
    {
        
        animator.enabled = true;
        animator.SetTrigger("reverse");
        StartSceneBttn.finishedAnimation = false;

    }
    public void SetIsAnimatingToFalse()
    {
        isGoingBack = false;
    }

    public void SetAnimationFinished()
    {
        isAnimating = false;
        StartSceneBttn.finishedAnimation = false;
        isGoingBack = false;
    }

    IEnumerator BackToOriginalRotation(Action callback)
    {

        float time = 0;
        isAnimating = true;
        isRotatingCamera = true;
        Quaternion desiredRotation = Quaternion.Euler(originalCameraRotation);
        Quaternion originalRotation = Camera.main.transform.rotation;

        while(time < cameraRotationDuration)
        {
            time += Time.deltaTime;
            if(time > cameraRotationDuration)
            {
                time = cameraRotationDuration;
            }

            Camera.main.transform.rotation = Quaternion.Slerp(originalRotation, desiredRotation, time / cameraRotationDuration);


            yield return null;
        }
        isRotatingCamera = false;
        callback();
    }




}

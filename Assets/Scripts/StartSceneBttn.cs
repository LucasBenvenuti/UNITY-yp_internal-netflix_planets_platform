using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneBttn : MonoBehaviour
{
    public static bool finishedAnimation = false;

    bool hasSeenTutorials = false;

    [SerializeField] Animator animator;

    [SerializeField] GameObject Tutorial1, Tutorial2, Tutorial3, TutorialCanvas;

    public static StartSceneBttn instance;

    public bool hasClicked = false;
    public bool hasOpenedPlanet = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        finishedAnimation = false;
        hasSeenTutorials = PlayerPrefs.GetInt("hasSeenTutorials", 0) == 0 ? false : true;
    }


    void Update()
    {
        if (!finishedAnimation)
        {
            animator.SetBool("landscape", !StaticHelpers.IsPortrait());
            if (MousePan.instance.target)
            {
                //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, MousePan.instance.target.planetPosition.position, Time.deltaTime * 5); 

                //Adjust rotation
                Quaternion newRotation = Camera.main.transform.rotation;
                Vector3 dir = (MousePan.instance.target.transform.position - Camera.main.transform.position).normalized;
                newRotation.SetLookRotation(dir, Vector3.up);

                newRotation = Quaternion.Slerp(Camera.main.transform.rotation, newRotation, 10 * Time.deltaTime);
                Camera.main.transform.rotation = newRotation;
            }

        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.DeleteAll();
        }

    }

    public void StartTutorial()
    {
        if (!hasSeenTutorials)
        {
            PlayerPrefs.SetInt("hasSeenTutorials", 1);
            PlayerPrefs.Save();
            StartCoroutine(TutorialRoutine());
        }
        else
        {
            TutorialCanvas.SetActive(false);
        }
    }
    public void StartScene()
    {
        animator.SetTrigger("start");
        Tutorial1.SetActive(false);
    }

    public void SetBackToInitialPositionFinished()
    {
        BackToInitialButton.instance.SetAnimationFinished();
    }


    public void DisableAnimator()
    {
        finishedAnimation = true;
        animator.enabled = false;
        MousePan.instance.StartCoroutine(MousePan.instance.ChangeLinesVisibility(false));
        FixFov.instance.UpdateFov();

        Planet[] planetList = FindObjectsOfType<Planet>();
        for (int i = 0; i < planetList.Length; i++)
        {
            planetList[i].SetupInitialRot();
        }
    }

    IEnumerator TutorialRoutine()
    {
        float time = 0;


        while (time < 10)
        {
            time += Time.deltaTime;
            if (finishedAnimation)
                time = 11;
            yield return null;

        }

        if (finishedAnimation == false)
            Tutorial1.SetActive(true);

        while (finishedAnimation == false)
        {
            yield return null;
        }

        Tutorial1.SetActive(false);

        time = 0;

        while (time < 5)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasClicked = true;
                time = 6;
            }
            time += Time.deltaTime;
            yield return null;
        }


        Tutorial1.SetActive(false);
        if (!hasClicked)
            Tutorial2.SetActive(true);

        while (!hasClicked)
        {
            if (Input.GetMouseButtonDown(0))
                hasClicked = true;
            yield return null;
        }

        Tutorial2.SetActive(false);

        hasClicked = false;

        while (!hasOpenedPlanet)
        {
            yield return null;
        }


        time = 0;

        while (time < 4)
        {
            yield return null;
            if (Input.GetMouseButtonDown(0))
            {
                hasClicked = true;
                time = 6;
            }
            time += Time.deltaTime;
        }

        if (!hasClicked)
            Tutorial3.SetActive(true);

        while (!Input.GetMouseButtonDown(0) && hasClicked == false)
        {
            yield return null;
        }

        TutorialCanvas.SetActive(false);

    }
}
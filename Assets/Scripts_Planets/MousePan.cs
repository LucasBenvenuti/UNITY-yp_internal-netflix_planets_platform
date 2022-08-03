using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePan : MonoBehaviour
{
    public Planet target = null;
    public InterestPoint targetPoint = null;
    public NetflixButton netflixButton;
    public event System.Action changeScreenSize;
    public float minX = -33f;
    public float maxX = 28f;
    public bool isGoingBack;

    public float speed = 3.5f;
    private float X;
    private float Y;

    Vector3 initialPos;

    [SerializeField] MeshRenderer[] linesRenderer;
    [SerializeField] float visibleStrength = 0.05f;
    [SerializeField] float invisibleStrength = 0.00f;

    public static MousePan instance;


    [SerializeField] float speedFallof1 = 10;
    [SerializeField] float speedFallof2 = 10;

    [SerializeField] Canvas buttonBack;
    [SerializeField] Canvas buttonBackMobile;

    Vector3 accumulatedSpeed1;
    Vector3 accumulatedSpeed2;
    Vector2 resolution;

    bool isOnCooldown = true;

    private bool backButtonOn = false;

    private void Awake()
    {
        initialPos = transform.position;

        if (instance == null)
        {
            instance = this;

            Destroy(transform.parent.GetComponent<Animator>(), 13);
        }


        resolution.x = Screen.width;
        resolution.y = Screen.height;

    }

    private void CheckScreenChange()
    {
        if (Screen.width != resolution.x || Screen.height != resolution.y)
        {
            if (backButtonOn)
            {
                if (StaticHelpers.IsPortrait())
                {
                    buttonBack.GetComponent<Animator>().SetTrigger("Off");
                    buttonBackMobile.GetComponent<Animator>().SetTrigger("On");
                    buttonBack.enabled = false;
                    buttonBackMobile.enabled = true;
                }
                else
                {
                    buttonBackMobile.GetComponent<Animator>().SetTrigger("Off");
                    buttonBack.GetComponent<Animator>().SetTrigger("On");
                    buttonBack.enabled = true;
                    buttonBackMobile.enabled = false;
                }
            }
            resolution.x = Screen.width;
            resolution.y = Screen.height;
            changeScreenSize?.Invoke();
        }
    }

    public void EnteredPlanet()
    {
        ShowButtonBack();
        StopAllCoroutines();
        //StartCoroutine(ChangeLinesVisibility(true));
    }

    public void ShowButtonBack()
    {
        if (backButtonOn)
        {
            return;
        }
        backButtonOn = true;

        if (StaticHelpers.IsPortrait())
        {
            buttonBackMobile.GetComponent<Animator>().SetTrigger("On");
            buttonBack.enabled = false;
            buttonBackMobile.enabled = true;
        }
        else
        {
            buttonBack.GetComponent<Animator>().SetTrigger("On");
            buttonBack.enabled = true;
            buttonBackMobile.enabled = false;
        }

    }

    public IEnumerator ChangeLinesVisibility(bool visible)
    {
        for (float f = 0; f < 1.0f; f += Time.deltaTime)
        {
            float curValue = 0;

            if (visible)
                curValue = Mathf.Lerp(visibleStrength, invisibleStrength, f);
            else
                curValue = Mathf.Lerp(invisibleStrength, visibleStrength, f);

            for (int i = 0; i < linesRenderer.Length; i++)
            {
                linesRenderer[i].material.SetFloat("Vector1_C0514F40", curValue);
            }
            yield return null;
        }
    }

    public void HideButtonBack()
    {
        if (!backButtonOn)
        {
            return;
        }
        backButtonOn = false;
        if (StaticHelpers.IsPortrait())
        {
            buttonBackMobile.GetComponent<Animator>().SetTrigger("Off");
        }
        else
        {
            buttonBack.GetComponent<Animator>().SetTrigger("Off");
        }
    }

    public void GoBack()
    {
        if (!backButtonOn)
            return;

        isGoingBack = true;

        if (target != null)
        {
            target.HideInterestPoints();
            target.BackToOriginalRotation();
        }
        target.planetHover.canShowLogo = true;
        target = null;
        HideButtonBack();
        StopAllCoroutines();
        StartCoroutine(ChangeLinesVisibility(false));
        // netflixButton.Hide();
    }

    void Update()
    {
        if (BackToInitialButton.instance.isAnimating)
        {
            return;
        }

        accumulatedSpeed1 = Vector3.Lerp(accumulatedSpeed1, Vector3.zero, Time.deltaTime * speedFallof1);
        accumulatedSpeed2 = Vector3.Lerp(accumulatedSpeed2, Vector3.zero, Time.deltaTime * speedFallof2);
        CheckScreenChange();
        if (target == null)
        {
            if (Input.GetMouseButton(0) && Input.touchCount < 2)
            {
                if (isOnCooldown == false)
                    accumulatedSpeed1 += new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0);

                isOnCooldown = false;
            }
            else
            {
                isOnCooldown = true;
            }

            transform.Rotate(accumulatedSpeed1);
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            // if(X <= minX || X >= maxX)
            // {
            //     Debug.Log("X: " + X);
            // }
            X = StaticHelpers.ClampAngle(X, minX, maxX);
            transform.localEulerAngles = new Vector3(X, Y, 0);

            transform.position = Vector3.Lerp(transform.position, initialPos, Time.deltaTime * 5);

            if ((transform.position - initialPos).magnitude < 0.1f)
            {
                Debug.Log("Is going back = false");
                isGoingBack = false;
            }

        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (isOnCooldown == false)
                    accumulatedSpeed2 += new Vector3(Input.GetAxis("Mouse X") * speed * Mathf.Deg2Rad, Input.GetAxis("Mouse Y") * speed * Mathf.Deg2Rad, 0);

                isOnCooldown = false;
            }
            else
            {
                isOnCooldown = true;
            }

            target.planet.RotateAround(transform.up, -accumulatedSpeed2.x);
            target.planet.RotateAround(transform.right, accumulatedSpeed2.y);

            transform.position = Vector3.Lerp(transform.position, target.planetPosition.position, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.planetPosition.rotation, Time.deltaTime * 5);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

}
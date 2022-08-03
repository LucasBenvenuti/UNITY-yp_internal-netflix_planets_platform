using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Transform planetPosition;
    public Transform planet;
    public PlanetHover planetHover;
    public string planetName = "";
    public string netflixUrl = "http://www.google.com";

    public Quaternion originalRotation;
    public float timeToBackToRotation = .5f;

    public Animator[] interestPointList;

    public bool showingInterestPoints = false;

    bool hasSpoken = false;

    [SerializeField] bool hasLegalText = false;

    public void SetupInitialRot()
    {
        originalRotation = planet.rotation;
    }

    public void BackToOriginalRotation()
    {
        StartCoroutine(BackToRotation());
    }

    private IEnumerator BackToRotation()
    {
        if (hasLegalText)
            LegalText.Instance.HideText();

        HideInterestPoints();

        float timer = 0;
        Quaternion oldRotation = planet.rotation;
        while (timer < timeToBackToRotation)
        {
            timer += Time.deltaTime;
            planet.rotation = Quaternion.Slerp(oldRotation, originalRotation, Mathf.SmoothStep(0, 1, timer / timeToBackToRotation));
            yield return null;
        }
        planet.rotation = originalRotation;
    }

    public void ShowInterestPoints()
    {
        if (showingInterestPoints)
        {
            return;
        }
        showingInterestPoints = true;

        for (int i = 0; i < interestPointList.Length; i++)
        {
            interestPointList[i].SetTrigger("On");
        }
    }

    public void HideInterestPoints()
    {
        if (!showingInterestPoints)
        {
            return;
        }
        showingInterestPoints = false;

        for (int i = 0; i < interestPointList.Length; i++)
        {
            interestPointList[i].SetTrigger("Off");
        }
    }

    public void SelectPlanet()
    {

        if (Input.touchCount > 1)
        {
            return;
        }

        if (MousePan.instance.target == this)
        {
            return;
        }

        if (BackToInitialButton.instance.isGoingBack)
        {
            return;
        }



        // #if !UNITY_EDITOR && UNITY_WEBGL
        //         GoogleAnalytics.SendAnalytics("navegacao", "planeta", "seleciona:" + planetName);
        // #endif

        MousePan.instance.target = this;
        MousePan.instance.EnteredPlanet();
        // MousePan.instance.netflixButton.Show(netflixUrl);
        planetHover.HideLogo();
        planetHover.canShowLogo = false;
        ShowInterestPoints();

        if (hasLegalText)
            LegalText.Instance.ShowText();

        if (!hasSpoken)
        {
            // transform.parent.GetComponent<AudioSource>().Play();
            // hasSpoken = true;
        }
    }
}
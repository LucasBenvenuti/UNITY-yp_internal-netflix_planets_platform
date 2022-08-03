using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3DPortrait : MonoBehaviour
{
    [SerializeField] GameObject portraitTutorial, landscapeTutorial;

    void Update()
    {
        UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        if(StaticHelpers.IsPortrait())
        {
            portraitTutorial.SetActive(true);
            landscapeTutorial.SetActive(false);
        }
        else
        {
            portraitTutorial.SetActive(false);
            landscapeTutorial.SetActive(true);
        }
    }
}

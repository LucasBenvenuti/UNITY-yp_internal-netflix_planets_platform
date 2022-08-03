using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixFov : MonoBehaviour
{
    public static FixFov instance;

    public float fovLandscape = 42;
    public float fovPortrait = 55;

    float baseFov = 0;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MousePan.instance.changeScreenSize += UpdateFov;
        Zoom.instance.zoom += UpdateFov;
        UpdateFov();
    }

    public void UpdateFov()
    {
        if (!StartSceneBttn.finishedAnimation)
        {
            return;
        }
        baseFov = StaticHelpers.IsPortrait() ? fovPortrait : fovLandscape;

        Camera.main.fieldOfView = baseFov + Zoom.instance.fovDelta;
    }

}

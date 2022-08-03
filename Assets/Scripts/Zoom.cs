using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Zoom : MonoBehaviour
{
    public static Zoom instance;
    public float fovDelta;
    public event System.Action zoom;

    [SerializeField]
    float minZoom = -20;

    [SerializeField]
    float maxZoom = 20;

    [SerializeField]
    float scrollFactor = 10;

    [SerializeField]
    float pinchFactor = 0.1f;

    [SerializeField]
    bool enableMouseScrollZoom = false;

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableMouseScrollZoom)
        {
            ApplyZoom(-Input.GetAxis("Mouse ScrollWheel") * scrollFactor);
        }

        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ApplyZoom(difference * pinchFactor);
        }
    }

    void ApplyZoom(float increment)
    {
        if (!StartSceneBttn.finishedAnimation)
        {
            return;
        }

        fovDelta += increment;

        fovDelta = Mathf.Clamp(fovDelta, minZoom, maxZoom);

        zoom?.Invoke();
    }
}

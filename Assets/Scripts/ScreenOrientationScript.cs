using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationScript : MonoBehaviour
{
    public ScreenOrientation screenOrientation;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = screenOrientation;
    }
}

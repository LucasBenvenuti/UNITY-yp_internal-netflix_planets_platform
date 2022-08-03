using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoFuncs : MonoBehaviour
{
    public void CloseUI()
    {
        VideoController.instance.ShowUI(false, null);
    }
}

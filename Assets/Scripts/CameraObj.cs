using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CameraObj : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public static bool volumeEnabled = true;

    public static CameraObj Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
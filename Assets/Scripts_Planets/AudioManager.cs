using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    //Do not change, must be this name so that react can access
    public void Mute()
    {
        Debug.Log("Muted");
        audioMixer.SetFloat("Volume", -80);
    }

    //Do not change, must be this name so that react can access  
    public void UnMute()
    {
        Debug.Log("Unmuted");
        audioMixer.SetFloat("Volume", 0);
    }
}

using PWE.Audio;
using UnityEngine;

public class MuteButtonUpdater : MonoBehaviour
{
    public AudioManager AudioManager;
    public ToggleTrigger ToggleTrigger;

    private void OnEnable()
    {
        // ToggleTrigger.SetToggle(AudioManager.MasterVolume != 0);
    }
}
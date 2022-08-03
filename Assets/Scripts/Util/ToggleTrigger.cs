using UnityEngine;
using UnityEngine.Events;

public class ToggleTrigger : MonoBehaviour
{
    public bool Toggled;

    public UnityEvent ToggleOn;
    public UnityEvent ToggleOff;

    private void OnValidate()
    {
        if (Toggled)
        {
            ToggleOn?.Invoke();
        }
        else
        {
            ToggleOff?.Invoke();
        }
    }

    public void Toggle()
    {
        if (Toggled)
        {
            Toggled = false;
            ToggleOff?.Invoke();
        }
        else
        {
            Toggled = true;
            ToggleOn?.Invoke();
        }
    }

    public void SetToggle(bool toggled)
    {
        Toggled = toggled;
        if (toggled)
        {
            ToggleOn?.Invoke();
        }
        else
        {
            ToggleOff?.Invoke();
        }
    }
}
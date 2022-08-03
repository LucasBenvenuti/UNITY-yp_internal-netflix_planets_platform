using UnityEngine;
using UnityEngine.UI;

public class AlphaButton : MonoBehaviour
{
    public float AlphaThreshold = 0.1f;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
        Destroy(this);
    }
}
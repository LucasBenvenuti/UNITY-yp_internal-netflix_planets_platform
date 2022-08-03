using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTutorial : MonoBehaviour
{
    [SerializeField] int counts = 0;
    [SerializeField] int requiredCounts = 2;

    public void ClosedHint()
    {
        counts++;
        if(counts >= 2)
        {
            gameObject.SetActive(false);
        }
    }
}

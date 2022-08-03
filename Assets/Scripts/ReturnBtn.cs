using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBtn : MonoBehaviour
{
    public void ReturnToPlanets()
    {
        if (SceneController.instance)
        {
            SceneController.instance.Close("Main", 1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform camera;

    private void Awake()
    {
        camera = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(camera);
    }
}
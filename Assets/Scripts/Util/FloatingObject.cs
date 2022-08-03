using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour
{
    [Header("Position")]
    public float amplitudeX = 0.5f;
    public float frequencyX = 1f;

    public float amplitudeY = 0.5f;
    public float frequencyY = 1f;

    public float amplitudeZ = 0.5f;
    public float frequencyZ = 1f;

    [Header("Rotation")]
    public float degreesPerSecondX;
    public float degreesPerSecondY;
    public float degreesPerSecondZ;

    Vector3 posOffset;
    Vector3 tempPos;

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Spin
        transform.Rotate(new Vector3(   Time.deltaTime * degreesPerSecondX, 
                                        Time.deltaTime * degreesPerSecondY, 
                                        Time.deltaTime * degreesPerSecondZ), Space.World);

        //Mov
        tempPos = posOffset;
        tempPos.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequencyX) * amplitudeX;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequencyY) * amplitudeY;
        tempPos.z += Mathf.Sin(Time.fixedTime * Mathf.PI * frequencyZ) * amplitudeZ;

        transform.position = tempPos;
    }
}

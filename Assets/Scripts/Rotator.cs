using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed;
    public Vector3 rotateVector;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotateVector * rotateSpeed * Time.deltaTime);
    }
}

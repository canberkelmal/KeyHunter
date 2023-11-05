using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    Vector3 cameraOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.position + cameraOffset;
    }
}

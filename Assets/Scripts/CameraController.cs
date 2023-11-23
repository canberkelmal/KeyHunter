using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public Vector3 cameraOffset = Vector3.zero;

    

    public void SetPlayerOffset()
    {
        cameraOffset = transform.position - player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.position + cameraOffset;
    }
}

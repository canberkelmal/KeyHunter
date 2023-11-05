using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
        if (floatingJoystick.Vertical != 0 || floatingJoystick.Horizontal != 0)
        {
            Vector3 direction = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical).normalized;
            Vector3 desiredVelocity = direction * speed;
            rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            rb.MoveRotation(targetRotation);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
 
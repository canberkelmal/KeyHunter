using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;

    public bool isRanged = false;
    public float range = 1f;
    public bool isAttacking = false;
    public LayerMask attackableLayerMask;
    public GameObject attackingObject;

    public void FixedUpdate()
    {
        // Check for any attackable around
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, attackableLayerMask);
        hitColliders = hitColliders.OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position)).ToArray();

        if (hitColliders.Length > 0)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }


        // Movement and rotation
        if (floatingJoystick.Vertical != 0 || floatingJoystick.Horizontal != 0)
        {
            Vector3 direction = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical).normalized;

            // Movement
            Vector3 desiredVelocity = direction * speed;
            rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

            // Rotation
            if (!isAttacking)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.MoveRotation(targetRotation);
                attackingObject = null;
            }
            else
            {
                Collider closestCollider = hitColliders[0];
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(closestCollider.transform.position.x, rb.transform.position.y, closestCollider.transform.position.z) - rb.transform.position, Vector3.up);
                rb.MoveRotation(Quaternion.Euler(0, targetRotation.eulerAngles.y, 0));
                AttackTo(closestCollider.gameObject);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }


    }

    void AttackTo(GameObject targetObject)
    {
        if(targetObject != attackingObject)
        {
            attackingObject = targetObject;
        }
    }
}
 
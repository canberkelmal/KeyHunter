using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;

    public bool isRanged = false;
    public float range = 1f;
    // Delta t between attacks
    public float attackSpeed = 1f;
    public bool isAttacking = false;
    public GameObject attackingObject;

    LayerMask attackableLayerMask, blockAttackLayerMask;
    float attackTimer = 0f;
    GameManager gameManager;
    bool isMoving = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        attackableLayerMask = gameManager.attackableLayerMask;
        blockAttackLayerMask = gameManager.blockAttackLayerMask;
    }
    public void FixedUpdate()
    {    
        // Movement and rotation
        if (floatingJoystick.Vertical != 0 || floatingJoystick.Horizontal != 0)
        {
            isMoving = true;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking",true);
            //CheckAround();
            Vector3 direction = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical).normalized;

            // Movement
            Vector3 desiredVelocity = direction * speed;
            rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

            // Rotation
            if (!CheckAround())
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.MoveRotation(targetRotation);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(attackingObject.transform.position.x, rb.transform.position.y, attackingObject.transform.position.z) - rb.transform.position, Vector3.up);
                rb.MoveRotation(Quaternion.Euler(0, targetRotation.eulerAngles.y, 0));

                AttackToNearest();
            }
        }
        else if(isMoving)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
            isMoving = false;
            rb.velocity = Vector3.zero;
            if(isAttacking) 
            {
                isAttacking = false;
                StopAttacking();
            }
        }
    }

    // Check for any attackable around
    bool CheckAround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, attackableLayerMask);
        List<Collider> collidersToRemove = new List<Collider>();

        foreach (Collider collider in hitColliders)
        {
            if (Physics.Linecast(transform.position, collider.transform.position, blockAttackLayerMask))
            {
                collidersToRemove.Add(collider);
            }
        }

        foreach (Collider colliderToRemove in collidersToRemove)
        {
            hitColliders = hitColliders.Where(val => val != colliderToRemove).ToArray();
        }

        if (hitColliders.Length > 0)
        {
            hitColliders = hitColliders.OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position)).ToArray();
            GameObject nearestObject = hitColliders[0].gameObject;

            if (attackingObject != nearestObject)
            {
                attackingObject = nearestObject;
            }
            isAttacking = true;
        }
        else if(isAttacking)
        {
            isAttacking = false;
            StopAttacking();
            attackingObject = null;
        }
        return isAttacking;
    }

    void AttackToNearest()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = attackSpeed;

            // For development process
            if (attackingObject.GetComponent<ObjSc>())
            {
                attackingObject.GetComponent<ObjSc>().Attacked();
            }

            // Attack from distance
            if (isRanged)
            {

            }
            // Attack up close
            else
            {

            }

        }
    }

    void StopAttacking()
    {
        Debug.Log("Attack stopped");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
 
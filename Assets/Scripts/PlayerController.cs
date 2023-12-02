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
    public Transform weaponPoint;
    public GameObject bullet;
    public GameObject rangeCircleImage;

    public bool isRanged = false;
    public float nearAttackRange = 1.5f;
    public float attackRange = 1.5f;
    // Delta t between attacks
    public float attackSpeed = 1f;
    public bool isAttacking = false;
    public bool attackAnim = false;


    public GameObject attackingObject;

    public Weapon currentWeapon;


    LayerMask attackableLayerMask, blockAttackLayerMask;
    GameManager gameManager;
    bool isMoving = false;
    bool playerController = true;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        attackableLayerMask = gameManager.attackableLayerMask;
        blockAttackLayerMask = gameManager.blockAttackLayerMask;
    }
    public void FixedUpdate()
    {    
        // Movement and rotation
        if ((floatingJoystick.Vertical != 0 || floatingJoystick.Horizontal != 0) && playerController)
        {
            isMoving = true;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking",true);
            Vector3 direction = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical).normalized;

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
                if (!attackAnim)
                {
                    attackAnim = true;
                    AttackToNearest();
                }
            }

            // Movement
            Vector3 desiredVelocity = attackingObject != null ? direction * speed / 2 : direction * speed;
            rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

            /*// Rotation
            if (!CheckAround())
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.MoveRotation(targetRotation);

                // Movement
                Vector3 desiredVelocity = direction * speed;
                rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(attackingObject.transform.position.x, rb.transform.position.y, attackingObject.transform.position.z) - rb.transform.position, Vector3.up);
                rb.MoveRotation(Quaternion.Euler(0, targetRotation.eulerAngles.y, 0));

                // Movement
                Vector3 desiredVelocity = transform.forward * speed / 2;
                rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

                AttackToNearest();
            }*/
        }
        else if(isMoving)
        {
            isMoving = false;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
            rb.velocity = Vector3.zero;
            if(isAttacking) 
            {
                isAttacking = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinalWall"))
        {
            gameManager.ReachToFinalGate();
        }
    }

    public void SetController(bool cntrl)
    {
        playerController = cntrl;
    }
    public void SetWeapon(Weapon setWeapon)
    {
        Weapon selectedWeapon = setWeapon;

        // Set the desired weapon
        currentWeapon = selectedWeapon;
        GameObject weaponObject = Instantiate(selectedWeapon.prefab, weaponPoint.transform);
        attackRange = selectedWeapon.range;
        rangeCircleImage.transform.localScale = Vector3.one * attackRange;
        isRanged = selectedWeapon.ranged;
        bullet = selectedWeapon.bullet != null ? selectedWeapon.bullet : null;
        SetAttackSpeed();
        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = selectedWeapon.animator;
    }

    public void SetAttackSpeed()
    {
        attackSpeed = gameManager.baseAttackSpeed * currentWeapon.weaponSpeed;
    }


    // Check for any attackable around
    bool CheckAround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, attackableLayerMask);
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
            attackingObject = null;
        }
        return isAttacking;
    }
    public void AttackToNearest()
    {
        // For development process
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
        transform.GetChild(0).GetComponent<Animator>().speed = attackSpeed;

        if (!isRanged)
        {
            weaponPoint.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().emitting = true;
        }
    }

    public void AttackAnimFinished()
    {
        attackAnim = false; 

        transform.GetChild(0).GetComponent<Animator>().speed = 1;

        if (!isRanged)
        {
            weaponPoint.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().emitting = false; 
        }
    }
    /*void AttackToNearest()
    { 
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = attackSpeed;

            // For development process
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
            transform.GetChild(0).GetComponent<Animator>().speed =

            if (!isRanged)
            {
                weaponPoint.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().emitting = true;
            }
        }
    }*/

    public void ThrowABullet()
    {
        if(attackingObject != null)
        {
            attackingObject.layer = gameManager.defaultLayerMask;
            Vector3 spawnPoint = weaponPoint.position - Vector3.up / 2;
            GameObject throwedBullet = Instantiate(bullet, spawnPoint, Quaternion.identity);
            throwedBullet.GetComponent<BulletSc>().Init(attackingObject.transform);
        }
    }

    public void HitToEnemy()
    {
        attackingObject.layer = gameManager.deathLayerMask;
        if (attackingObject.GetComponent<EnemySc>())
        {
            attackingObject.GetComponent<EnemySc>().Attacked();
        }
        AttackAnimFinished();
    }

    public void SetRange(bool ranged, float range)
    {
        isRanged = ranged;
        attackRange = ranged ? range : nearAttackRange;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
 
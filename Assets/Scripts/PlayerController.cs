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
    public HealthBarSc healthBar;
    public float maxHealth = 80f;
    public float currentHealth = 80f;

    public bool isRanged = false;
    public float nearAttackRange = 1.5f;
    public float damage = 0;
    public float attackRange = 1.5f;
    // Delta t between attacks
    public float attackSpeed = 1f;
    public bool isAttacking = false;
    //public bool attackAnim = false;


    public GameObject attackingObject;

    public Weapon currentWeapon;

    Transform bulletPoint;
    LayerMask attackableLayerMask, blockAttackLayerMask;
    GameManager gameManager;
    bool isMoving = false;
    public bool isDeath = false;
    bool playerController = true;
    bool hitToObs = false;
    float attackTimer = 0f;
    bool loadingToPlatform = false;
    Transform tempParent;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        attackableLayerMask = gameManager.attackableLayerMask;
        blockAttackLayerMask = gameManager.blockAttackLayerMask;
        //healthBar.SetFillAmountDirect(1);
    }
    public void FixedUpdate()
    {    
        // Movement and rotation
        if ((floatingJoystick.Vertical != 0 || floatingJoystick.Horizontal != 0) && playerController && !hitToObs)
        {
            isMoving = true;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking",true);
            Vector3 direction = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical).normalized;

            // Rotation
            if (!CheckAround())
            {
                attackTimer = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.MoveRotation(targetRotation);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(attackingObject.transform.position.x, rb.transform.position.y, attackingObject.transform.position.z) - rb.transform.position, Vector3.up);
                rb.MoveRotation(Quaternion.Euler(0, targetRotation.eulerAngles.y, 0));
                AttackToNearest();
                /*if (!attackAnim)
                {
                    attackAnim = true;
                    AttackToNearest();
                }*/
            }

            // Movement
            Vector3 desiredVelocity = attackingObject != null ? direction * speed / 1.2f : direction * speed;
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
        else if (hitToObs)
        {
            isMoving = false;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
            //rb.velocity = Vector3.zero;
            if (isAttacking)
            {
                isAttacking = false; 
            }
        }
        else if (loadingToPlatform)
        {
            transform.LookAt(transform.parent.position);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed*Time.deltaTime * 1.2f);
            if(transform.localPosition == Vector3.zero)
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
                transform.parent.parent.GetComponent<GateSc>().MoveThePlatform();
                loadingToPlatform = false;
            }
        }
        else
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

    public void TakeHit(float damage)
    {
        currentHealth -= damage;
        SetHp();
    }
    public void Heal(float heal)
    {
        currentHealth += heal;
        SetHp();
    }

    public void SetHp()
    {
        if (!isDeath)
        {
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                PlayerDeath();
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            PlayerPrefs.SetFloat("PlayerHp", currentHealth);
            healthBar.SetFillAmount(currentHealth / maxHealth, true);
        }
    }

    public void PlayerDeath()
    {
        isDeath = true;
        SetController(false);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Death");
        gameManager.PlayerIsDeath();
    }

    public void LoadToPlatform(GameObject platform)
    {
        tempParent = transform.parent != null ? transform.parent : null;
        transform.parent = platform.transform.Find("PlatformObj");
        playerController = false;
        loadingToPlatform = true;
    }
    public void UnloadFromPlatform()
    {
        transform.parent = tempParent != null ? tempParent : null;
        playerController = true;
    }

    public void SetController(bool cntrl)
    {
        playerController = cntrl;
        if (cntrl)
        {
            hitToObs = false;
        }
    }

    public void HitToObstacle()
    {
        hitToObs = true;
    }
    public void SetWeapon(Weapon setWeapon)
    {
        Weapon selectedWeapon = setWeapon;

        // Set the desired weapon
        currentWeapon = selectedWeapon;
        GameObject weaponObject = Instantiate(selectedWeapon.prefab, weaponPoint.transform); //weapon childý oluþturulup, bu satýrdan sonra mermi çýkýþ mekaniði eklenecek
        bulletPoint = weaponObject.transform.Find("BulletPoint");
        attackRange = selectedWeapon.Range();
        rangeCircleImage.transform.localScale = Vector3.one * attackRange;
        isRanged = selectedWeapon.ranged;
        if(selectedWeapon.bullet != null)
        {
            bullet = selectedWeapon.bullet;
            bullet.GetComponent<BulletSc>().damage = selectedWeapon.Damage();
        }
        SetDamage();
        SetAttackSpeed();
        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = selectedWeapon.animator;
    }

    public void SetAttackSpeed()
    {
        if(currentWeapon != null)
        {
            attackSpeed = gameManager.baseAttackSpeedMultiplier * currentWeapon.AttackSpeed();
        }
        else
        {
            attackSpeed = gameManager.baseAttackSpeedMultiplier;
        }
    }

    public void SetDamage()
    {
        if (currentWeapon != null)
        {
            damage = gameManager.baseDamageMultiplier * currentWeapon.Damage();
        }
        else
        {
            attackSpeed = gameManager.baseDamageMultiplier;
        }
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
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            attackTimer = attackSpeed;
            
            // For development process
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
            transform.GetChild(0).GetComponent<Animator>().speed = currentWeapon.minAttackSpeed/attackSpeed;

            if (!isRanged)
            {
                weaponPoint.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().emitting = true;
            }
        }        
    }

    public void AttackAnimFinished()
    {
        //attackAnim = false; 

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
            Vector3 spawnPoint = bulletPoint.position;
            GameObject throwedBullet = Instantiate(bullet, spawnPoint, Quaternion.identity);
            throwedBullet.GetComponent<BulletSc>().Init(attackingObject.transform, damage);
        }
    }

    public void HitToEnemy()
    {
        if (attackingObject.GetComponent<EnemySc>())
        {
            attackingObject.GetComponent<EnemySc>().TakeHit(damage);
        }
        else if (attackingObject.GetComponent<BossSc>())
        {
            attackingObject.GetComponent<BossSc>().TakeHit(damage);
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
 
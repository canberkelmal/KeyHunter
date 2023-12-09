using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    GameManager gameManager;
    public float speed = 2f;
    public float damage = 10f;
    public float waitDuration = 3f;
    public float throwForce;
    public GameObject[] dropObjects;
    public HealthBarSc healthBar;
    public GameObject throwObjPrefab;
    public Transform handObjectPoint;
    public float currentHealth = 30f;
    public float maxHealth = 30f;
    public bool isWalker = false;
    public float attackSpeed = 2f;
    public float fallowingDistance = 1f; 


    public List<Vector3> tilePoints;
    bool isMoving = false;
    bool isWaiting = false;
    bool isDeath = false;
    int currentTile = 0;
    float attackTimer = 1;
    float waitTimer = 0;
    public GameObject throwedObj;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        healthBar.SetFillAmountDirect(1);
        foreach(Transform t in transform.Find("Tiles"))
        {
            tilePoints.Add(t.position);
            t.GetComponent<MeshRenderer>().enabled = false;
        }
        isMoving = isWalker;
        if(isWalker)
        {
            transform.LookAt(tilePoints[currentTile]);
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
        }
        attackTimer = 0;
        waitTimer = waitDuration;
    }

    private void FixedUpdate()
    {
        if (!isDeath)
        {
            if (isWalker && isMoving && currentHealth > 0 && !isWaiting)
            {
                transform.position = Vector3.MoveTowards(transform.position, tilePoints[currentTile], speed * Time.deltaTime);
                if (transform.position == tilePoints[currentTile])
                {
                    if (currentTile < tilePoints.Count - 1)
                    {
                        currentTile++;
                    }
                    else
                    {
                        currentTile = 0;
                    }
                    WaitOnTile(true);
                    transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
                }
            }

            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    waitTimer = waitDuration;
                    WaitOnTile(false);
                    transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
                }
            }
        }
    }

    void WaitOnTile(bool waitStatu)
    {
        isWaiting = waitStatu;
        if(!isWaiting)
        {
            transform.LookAt(tilePoints[currentTile]);
        }        
    }

    public void AttackToPlayer()
    {
        if(!isDeath)
        {
            isWaiting = false;
            isMoving = false;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackTimer = attackSpeed;

                SpawnThrowObject();
                transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
            }

            transform.LookAt(gameManager.player.transform.position);

            float currentDistance = Vector3.Distance(transform.position, gameManager.player.transform.position);

            // Eðer mesafe, belirlenen uzaklýktan daha büyükse takip et
            if (currentDistance > fallowingDistance)
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
                // B objesinin pozisyonunu al
                Vector3 targetPosition = gameManager.player.transform.position;

                // A ve B objeleri arasýndaki mesafeyi belirt
                Vector3 direction = transform.position - targetPosition;
                direction.Normalize();
                Vector3 newPosition = targetPosition + direction * fallowingDistance;

                // A objesinin pozisyonunu güncelle
                transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
            }
            else
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
            }
        }
    }

    public void StopAttack()
    {
        if (isWalker)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
            transform.LookAt(tilePoints[currentTile]);
            isMoving = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
        }
    }

    public void SpawnThrowObject()
    {
        throwedObj = Instantiate(throwObjPrefab, handObjectPoint.position, Quaternion.identity, handObjectPoint);
    }

    public void ThrowObject()
    {
        throwedObj.GetComponent<ThrowToPlayerObjSc>().Init(gameManager.player.transform, damage);
    }

    public void TakeHit(float damage)
    {
        AttackToPlayer();
        currentHealth -= damage;
        SetHp();
    }

    public void SetHp()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EnemyDeath();
        }
        healthBar.SetFillAmount(currentHealth / maxHealth, false);
    }

    public void EnemyDeath()
    {
        isDeath = true;
        transform.Find("PlayerDedector").gameObject.SetActive(false);
        gameObject.layer = gameManager.defaultLayerMask;
        GetComponent<CapsuleCollider>().enabled = false;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        foreach (Transform boxPart in transform.GetChild(1))
        {
            Vector3 throwVec = (gameManager.player.transform.position - boxPart.position).normalized * throwForce;
            boxPart.GetComponent<Rigidbody>().AddForce(throwVec, ForceMode.Impulse);
        }
        DropObject();
        Destroy(gameObject, 2f);
        gameManager.EnemyDeath();
    }

    public void DropObject()
    {
        foreach (GameObject dropObj in dropObjects)
        {
            GameObject droped = Instantiate(dropObj, transform.position, Quaternion.identity, gameManager.collectablesParent);
            droped.GetComponent<CollectableSc>().ThrowObject(false);
        }
    }
}

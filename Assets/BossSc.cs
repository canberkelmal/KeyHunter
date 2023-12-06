using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSc : MonoBehaviour
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
    public bool isMoving = false;
    bool isWaiting = false;
    int currentTile = 0;
    float attackTimer = 1;
    float waitTimer = 0;
    bool isDeath = false;
    public GameObject throwedObj;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetBossLevel();
        maxHealth = gameManager.playerController.maxHealth;
        currentHealth = maxHealth;
        healthBar.SetFillAmountDirect(1);
        foreach (Transform t in transform.Find("Tiles"))
        {
            tilePoints.Add(t.position);
            t.GetComponent<MeshRenderer>().enabled = false;
        }
        isMoving = isWalker;
        if (isWalker)
        {
            transform.LookAt(tilePoints[currentTile]);
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
        }
        attackTimer = 0;
        waitTimer = waitDuration;
    }

    private void FixedUpdate()
    {
        if(!isDeath)
        {
            if (isWalker && isMoving && currentHealth > 0)
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
                    AttackToPlayer();
                }
            }
        }
    }

    public void AttackToPlayer()
    {
        isMoving = false;

        transform.LookAt(gameManager.player.transform.position);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");

    }

    public void ThrowObject()
    {
        throwedObj = Instantiate(throwObjPrefab, handObjectPoint.position, Quaternion.identity, null);
    }

    public void AnimFinished()
    {
        transform.LookAt(tilePoints[currentTile]);
        isMoving = true;
    }

    public void TakeHit(float damage)
    {
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
        gameObject.layer = gameManager.defaultLayerMask;
        GetComponent<CapsuleCollider>().enabled = false;
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Death");

        //transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(true);

        /*foreach (Transform boxPart in transform.GetChild(1))
        {
            Vector3 throwVec = (gameManager.player.transform.position - boxPart.position).normalized * throwForce;
            boxPart.GetComponent<Rigidbody>().AddForce(throwVec, ForceMode.Impulse);
        }*/
        DropObject();
        Destroy(gameObject, 2f);
        gameManager.EnemyDeath();
    }

    public void DropObject()
    {
        foreach (GameObject dropObj in dropObjects)
        {
            Instantiate(dropObj, transform.position, Quaternion.identity, gameManager.collectablesParent);
        }
    }
}

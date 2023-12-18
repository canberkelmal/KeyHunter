using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using TMPro;

public class CollectableSc : MonoBehaviour
{
    public enum CollectableTypes 
    {
        coin,
        buff,
        cross,
        key
    }

    public float moveSpeed = 1f;
    public CollectableTypes type;
    public int amount = 1;
    public Buff buff1, buff2;
    
    GameManager gameManager;
    bool moveToPlayer = false;
    Transform movingTarget;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        //ThrowObject();
    }

    /*private void FixedUpdate()
    {
        if(moveToPlayer && type != CollectableTypes.buff)
        {
            transform.position = Vector3.MoveTowards(transform.position, movingTarget.position, moveSpeed*Time.deltaTime);
        }
    }*/
    public void ThrowObject(bool toPlayer)
    {
        float radius = 0.7f;
        Vector3 randomCircle = Random.insideUnitCircle * radius;

        Vector3 targetPosition = new Vector3(transform.position.x + randomCircle.x, gameManager.dropHeight, transform.position.z + randomCircle.y);
        if (toPlayer)
        {
            targetPosition = transform.position + (gameManager.player.transform.position - transform.position).normalized * 2;
            targetPosition.y = gameManager.dropHeight;
            transform.DOJump(targetPosition, 1.5f, 1, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(JumpDone);
        }
        transform.DOJump(targetPosition, 1.5f, 1, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(JumpDone);
    }
    public void MoveToPlayer(Transform playerTransform)
    {
        //JumpDone();
        movingTarget = playerTransform;
        moveToPlayer = true;
    } 
    public void JumpDone()
    {
        gameObject.GetComponents<Collider>()[0].enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collected();
        }
    }

    void Collected()
    {
        bool a = false;
        bool b = false;
        switch (type)
        {
            case CollectableTypes.coin:
                gameManager.SetCoinAmount(1);
                a = true;
                //CreateUIObject();
                break;
            case CollectableTypes.cross:
                gameManager.SetCrossAmount(1);
                a = true;
                //CreateUIObject();
                break;
            case CollectableTypes.buff:
                gameManager.SetBuff(buff1,buff2);
                break;
            case CollectableTypes.key:

                gameManager.GetKey();
                CreateUIObject();
                b = true;
                break;
        }

        if (a)
        {
            GameObject takePart = Instantiate(gameManager.takeParticle, transform.position, Quaternion.identity);
            Destroy(takePart, 1.5f);
        }
        if (b)
        {
            GameObject takePart = Instantiate(gameManager.getKeyParticle, transform.position, Quaternion.identity);
            Destroy(takePart, 1.5f);
        }
        Destroy(gameObject);
    }

    void CreateUIObject()
    {
        Vector3 cupScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        switch (type)
        {
            case CollectableTypes.coin:
                Instantiate(gameManager.coinUIPrefab, cupScreenPos, Quaternion.identity, gameManager.coinText.transform.parent.Find("Icon"));
                break;
            case CollectableTypes.cross:
                Instantiate(gameManager.crossUIPrefab, cupScreenPos, Quaternion.identity, gameManager.crossText.transform.parent.Find("Icon"));
                break;
            case CollectableTypes.key:
                Instantiate(gameManager.keyUIPrefab, cupScreenPos, Quaternion.identity, gameManager.keyUI.transform);
                break;
        }
    }
}

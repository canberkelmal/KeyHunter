using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

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
    public Buff buff1, buff2, buff3;
    
    GameManager gameManager;
    bool moveToPlayer = false;
    Transform movingTarget;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ThrowObject();
    }

    private void FixedUpdate()
    {
        if(moveToPlayer && type != CollectableTypes.buff)
        {
            transform.position = Vector3.MoveTowards(transform.position, movingTarget.position, moveSpeed*Time.deltaTime);
        }
    }
    public void ThrowObject()
    {
        if(type == CollectableTypes.buff)
        {
            Vector3 targetPosition = transform.position + (gameManager.player.transform.position - transform.position).normalized * 2;
            targetPosition.y = gameManager.dropHeight;
            transform.DOJump(targetPosition, 1.5f, 1, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(JumpDone);
        }
        else
        {
            float radius = 0.7f;
            Vector3 randomCircle = Random.insideUnitCircle * radius;

            Vector3 targetPosition = new Vector3(transform.position.x + randomCircle.x, gameManager.dropHeight, transform.position.z + randomCircle.y);
            //targetPosition.y = gameManager.dropHeight; 
            transform.DOJump(targetPosition, 1.5f, 1, 0.5f)
                .SetEase(Ease.Linear);
        }
    }
    public void MoveToPlayer(Transform playerTransform)
    {
        JumpDone();
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
        switch (type)
        {
            case CollectableTypes.coin:
                gameManager.SetCoinAmount(amount);
                break;
            case CollectableTypes.cross:
                gameManager.SetCrossAmount(amount);
                break;
            case CollectableTypes.buff:
                gameManager.SetBuff(buff1,buff2);
                break;
            case CollectableTypes.key:
                gameManager.GetKey();
                break;
        }
        Destroy(gameObject);
    }
}

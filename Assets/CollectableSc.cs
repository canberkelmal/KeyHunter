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
        cross
    }

    public CollectableTypes type;
    public int amount = 1;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ThrowObject();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collected();
        }
    }
    public void ThrowObject()
    {
        Vector3 targetPosition = transform.position + (gameManager.player.transform.position - transform.position).normalized * 2;
        targetPosition.y = gameManager.dropHeight;
        transform.DOJump(targetPosition, 1.5f, 1, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(JumpDone);
    }

    public void JumpDone()
    {
        gameObject.GetComponents<Collider>()[0].enabled = true;
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
                gameManager.SetBuff();
                break;
        }
        Destroy(gameObject);
    }
}

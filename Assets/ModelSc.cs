using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSc : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = transform.parent.gameObject;
    }

    public void TouchToEnemy()
    {
        player.GetComponent<PlayerController>().HitToEnemy();
    }

    public void ThrowBullet()
    {
        player.GetComponent<PlayerController>().ThrowABullet();
    }

    public void AttackAnimFinished() 
    {
        Debug.Log("AttackAnimFinished from model");
        player.GetComponent<PlayerController>().AttackAnimFinished();
    }
}

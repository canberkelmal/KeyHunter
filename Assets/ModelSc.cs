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
        transform.parent.GetComponent<PlayerController>().HitToEnemy();
    }

    public void ThrowBullet()
    {
        player.GetComponent<PlayerController>().ThrowABullet();
    }
}

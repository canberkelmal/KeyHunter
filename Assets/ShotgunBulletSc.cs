using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletSc : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        transform.LookAt(gameManager.player.transform.position);
        foreach(Transform miniBullet in transform)
        {
            miniBullet.GetComponent<ShotgunMiniBulletSc>().Init();
        }
    }

    private void FixedUpdate()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }


}

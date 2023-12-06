using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalashBulletSc : MonoBehaviour
{
    GameManager gameManager;
    float delay = 0.3f;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(FireMiniBulletsWithDelay());
    }
    private IEnumerator FireMiniBulletsWithDelay()
    {
        while(transform.childCount != 0)
        {
            Transform miniBullet = transform.GetChild(0);
            transform.LookAt(gameManager.player.transform.position);
            miniBullet.parent = null;
            miniBullet.GetComponent<ShotgunMiniBulletSc>().Init();
            yield return new WaitForSeconds(delay);
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        /*if (transform.childCount == 0)
        {
            Destroy(gameObject, delay);
        }*/
    }
}

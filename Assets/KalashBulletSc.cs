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
        transform.LookAt(gameManager.player.transform.position);
        StartCoroutine(FireMiniBulletsWithDelay(delay));
    }
    private IEnumerator FireMiniBulletsWithDelay(float t)
    {
        foreach (Transform miniBullet in transform)
        {
            miniBullet.GetComponent<ShotgunMiniBulletSc>().Init();
            yield return new WaitForSeconds(t);
        }
    }

    private void FixedUpdate()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}

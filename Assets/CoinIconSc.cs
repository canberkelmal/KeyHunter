using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinIconSc : MonoBehaviour
{
    public float speed = 10;

    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        if(transform.localPosition == Vector3.zero)
        {
            gameManager.SetCoinAmount(1);
            Destroy(gameObject);
        }
    }
}

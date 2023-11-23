using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Attacked() 
    {
        DropObject();
        Destroy(gameObject);
        /*
        GetComponent<Renderer>().material = gameManager.attackedMat;
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = gameManager.deathLayerMask;*/
    }
    public void DropObject()
    {
        Instantiate(gameManager.collectables[0], transform.position, Quaternion.identity);
    }
}

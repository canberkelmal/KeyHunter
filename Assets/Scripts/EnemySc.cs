using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    GameManager gameManager;
    LayerMask deathLayerMask;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Attacked() 
    {
        GetComponent<Renderer>().material = gameManager.attackedMat;
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = gameManager.deathLayerMask;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSc : MonoBehaviour
{
    GameManager gameManager;
    public bool isDoorAvailable = false;
    public GameObject particle, lockObj;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddToGates(GetComponent<GateSc>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.LockedTx();
        }
    }
    public void SetGateAvailable(bool keyStatu)
    {
        isDoorAvailable = keyStatu;

        GetComponent<BoxCollider>().enabled = !isDoorAvailable;
        //GetComponent<BoxCollider>().isTrigger = isDoorAvailable;
        //particle.SetActive(isDoorAvailable);
        lockObj.SetActive(!isDoorAvailable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGateScript : MonoBehaviour
{
    GameManager gameManager;
    public bool isDoorAvailable = false;
    public GameObject particle, lockObj;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetFinalGate(gameObject);
    }

    public void SetGateAvailable(bool available)
    {
        isDoorAvailable = available;

        GetComponent<BoxCollider>().isTrigger = isDoorAvailable;
        particle.SetActive(isDoorAvailable);
        lockObj.SetActive(!isDoorAvailable);
    }
}

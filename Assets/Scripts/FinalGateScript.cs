using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGateScript : MonoBehaviour
{
    GameManager gameManager;
    public bool isDoorAvailable = false;
    public GameObject particle, lockObj;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetFinalGate(gameObject);
    }



    public void SetGateAvailable(bool keyStatu, bool enemyStatu)
    {
        isDoorAvailable = keyStatu && enemyStatu;

        GetComponent<BoxCollider>().isTrigger = isDoorAvailable;
        particle.SetActive(isDoorAvailable);
        lockObj.SetActive(!keyStatu);
    }
}

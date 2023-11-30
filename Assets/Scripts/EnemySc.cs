using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    GameManager gameManager;
    public float throwForce;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Attacked()
    {
        GetComponent<CapsuleCollider>().enabled = false;
         
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        foreach (Transform boxPart in transform.GetChild(1))
        {
            Vector3 throwVec = (gameManager.player.transform.position - boxPart.position).normalized * throwForce;
            boxPart.GetComponent<Rigidbody>().AddForce(throwVec, ForceMode.Impulse);
        }
        DropObject();
        Destroy(gameObject, 2f);
    }

    public void DropObject()
    {
        Instantiate(gameManager.collectables[0], transform.position, Quaternion.identity);
    }
}

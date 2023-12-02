using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CollectableSc;

public class ObjSc : MonoBehaviour
{
    bool isAttacked = false;

    GameManager gameManager;
    public float throwUpForce = 1;
    public GameObject[] dropObjects;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        foreach (GameObject obj in dropObjects)
        {
            if(obj.GetComponent<CollectableSc>().type == CollectableTypes.key)
            {
                gameManager.SetFinalGateStatu(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacked)
        { 
            Attacked();
        }
    }

    public void Attacked()
    {
        isAttacked = true;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        foreach(Transform boxPart in transform.GetChild(1))
        {
            boxPart.GetComponent<Rigidbody>().AddForce(Vector3.up * throwUpForce, ForceMode.Impulse);
        }
        DropObject();
        Destroy(gameObject, 2f);
    }

    public void DropObject()
    {
        foreach(GameObject dropObj in dropObjects)
        {
            Instantiate(dropObj, transform.position, Quaternion.identity, gameManager.collectablesParent);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSc : MonoBehaviour
{
    bool isAttacked = false;

    GameManager gameManager;
    public float throwUpForce = 1;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        GameObject obj = Random.Range(0, 2) > 0 ? gameManager.collectables[0] : gameManager.collectables[2];

        Instantiate(obj, transform.position, Quaternion.identity);
    }
}

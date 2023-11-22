using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSc : MonoBehaviour
{
    GameManager gameManager;
    public float throwUpForce = 1;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Attacked()
    {
        gameObject.tag = "Untagged";

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        foreach(Transform boxPart in transform.GetChild(1))
        {
            boxPart.GetComponent<Rigidbody>().AddForce(Vector3.up * throwUpForce, ForceMode.Impulse);
        }

        Destroy(gameObject, 3f);
        //GetComponent<Renderer>().material = gameManager.attackedMat;
    }
}

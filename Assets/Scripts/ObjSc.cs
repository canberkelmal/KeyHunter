using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSc : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Attacked()
    {
        gameObject.tag = "Untagged";
        GetComponent<Renderer>().material = gameManager.attackedMat;
    }
}

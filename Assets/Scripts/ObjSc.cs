using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSc : MonoBehaviour
{
    GameManager gameManager;
    LayerMask defLayerMask;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        defLayerMask = gameManager.defaultLayerMask;
    }

    public void Attacked()
    {
        gameObject.layer = defLayerMask;
        GetComponent<Renderer>().material = gameManager.attackedMat;
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CollectableSc;

public class ChestSc : MonoBehaviour
{
    bool isAttacked = false;

    GameManager gameManager;
    public GameObject[] dropObjects;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        foreach (GameObject obj in dropObjects)
        {
            if (obj.GetComponent<CollectableSc>().type == CollectableTypes.key)
            {
                gameManager.SetKeyLevel(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacked)
        {
            isAttacked = true;
            Attacked();
        }
    }

    public void Attacked()
    {     
        DOTween.To(() => transform.GetChild(0).GetChild(1).localRotation.eulerAngles.x, x => {
            Vector3 newRotation = transform.GetChild(0).GetChild(1).localRotation.eulerAngles;
            newRotation.x = x;
            transform.GetChild(0).GetChild(1).localRotation = Quaternion.Euler(newRotation);
        },
            40f,
            0.5f
        );

        DropObject();
        //Destroy(gameObject, 2f);
    }

    public void DropObject()
    {
        foreach (GameObject dropObj in dropObjects)
        {
            GameObject droped = Instantiate(dropObj, transform.position, Quaternion.identity, gameManager.collectablesParent);
            droped.GetComponent<CollectableSc>().ThrowObject(true);
        }
    }
}

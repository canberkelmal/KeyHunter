using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDedectorSc : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Physics.Linecast(transform.position, other.transform.position, gameManager.blockAttackLayerMask))
            {
                transform.parent.GetComponent<EnemySc>().StopAttack();
            }
            else
            {
                transform.parent.GetComponent<EnemySc>().AttackToPlayer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<EnemySc>().StopAttack();
        }
    }
}

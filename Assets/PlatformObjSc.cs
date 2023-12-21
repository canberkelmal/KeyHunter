using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObjSc : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<GateSc>().LoadThePlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<GateSc>().UnlockGate();
        }
    }
}

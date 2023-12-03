using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSc : MonoBehaviour
{
    public float damage = 0;
    public float pushbackForce = 0;
    public PlayerController controller;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            controller = collision.gameObject.GetComponent<PlayerController>();
            controller.TakeHit(damage);

            if (pushbackForce > 0)
            {
                controller.SetController(false);
                Invoke("OpenController", 1);
                Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 pushDirection = collision.contacts[0].point - collision.transform.position;
                pushDirection = -pushDirection.normalized; // Çarpýþma noktasýndan oyuncuya doðru olan vektörü al
                //pushDirection = new Vector3(pushDirection.z, 0, pushDirection.x);
                pushDirection.y = 0;
                playerRigidbody.AddForce(pushDirection * pushbackForce, ForceMode.Impulse); // Ýtmeyi uygula
                Debug.Log("Pushback dir :" + pushDirection);
            }
        }
    }

    void OpenController()
    {
        controller.SetController(true);
    }
}

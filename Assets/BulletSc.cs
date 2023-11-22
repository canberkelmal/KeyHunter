using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSc : MonoBehaviour
{
    public float speed = 1f;
    public bool throwed = false;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (throwed)
        {
            if(target == null)
            {
                Destroy(gameObject);
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    public void Init(Transform targetObj)
    {
        target = targetObj;
        throwed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemySc>().Attacked();
            Destroy(gameObject);
        }
    }
}

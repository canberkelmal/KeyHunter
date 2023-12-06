using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowToPlayerObjSc : MonoBehaviour
{
    public float speed = 1f;
    public float lifeTime = 2.5f;
    public bool throwed = false;
    public Transform target;
    public Vector3 throwedPlace;
    public float damage = 10f;

    float lifeTimer = 0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (throwed)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
        }

        lifeTimer += Time.deltaTime;
         
        if(lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Transform targetObj, float damage)
    {
        lifeTimer = 0;
        target = targetObj;
        throwed = true;
        transform.parent = null;
        transform.LookAt(target.transform.position);
        this.damage = damage;
        transform.GetChild(0).GetComponent<Rotator>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeHit(damage);
            Destroy(gameObject);
        }
    }
}

using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSc : MonoBehaviour
{
    public float speed = 1f;
    public bool throwed = false;
    public Transform target;
    public float damage = 8f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (throwed)
        {
            if(target.gameObject.GetComponent<CapsuleCollider>().enabled == false)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
    } 

    public void Init(Transform targetObj, float damage)
    {
        target = targetObj;
        throwed = true;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemySc>().TakeHit(damage);
            Destroy(gameObject);
        }
    }
}

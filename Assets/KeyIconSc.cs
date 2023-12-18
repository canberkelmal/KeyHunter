using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyIconSc : MonoBehaviour
{
    public float speed = 500;

    void FixedUpdate()
    {
        speed *= (1 + Time.deltaTime * 2);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        if (transform.localPosition == Vector3.zero)
        {
            transform.parent.GetComponent<Image>().color = Color.green;
            GetComponent<KeyIconSc>().enabled = false; 
        }
    }
}

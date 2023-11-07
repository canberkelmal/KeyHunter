using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSc : MonoBehaviour
{
    
    public void TouchToEnemy()
    {
        transform.parent.GetComponent<PlayerController>().HitToEnemy();
    }
}

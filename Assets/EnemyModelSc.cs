using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelSc : MonoBehaviour
{
    EnemySc enemyController;

    private void Start()
    {
        enemyController = transform.parent.GetComponent<EnemySc>();
    }

    public void ThrowBone()
    {
       enemyController.ThrowObject();
    }
}

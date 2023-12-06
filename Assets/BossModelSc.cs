using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModelSc : MonoBehaviour
{
    BossSc enemyController;

    private void Start()
    {
        enemyController = transform.parent.GetComponent<BossSc>();
    }

    public void ThrowObject()
    {
        enemyController.ThrowObject();
    }

    public void AnimationDone()
    {
        enemyController.AnimFinished();
    }
}

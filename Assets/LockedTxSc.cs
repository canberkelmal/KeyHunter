using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedTxSc : MonoBehaviour
{
    public float showDur = 1.5f;

    float timer = 0;

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > showDur)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    public void ShowTx()
    {
        timer = 0;
    }
}
